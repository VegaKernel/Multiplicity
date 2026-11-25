# Multiplicity

`Multiplicity.Packets` is a low-level C# library for reading and writing Terraria network packets.

NuGet package id: `Multiplicity`. The assembly and public namespace remain `Multiplicity.Packets`.

Current package version: **4.0.0**.

## Dependency policy

`Multiplicity.Packets` is intentionally a **BCL-only runtime/package library**. This is an architectural
invariant, not an accident of the current implementation:

- do not add runtime `PackageReference` dependencies to `Multiplicity.Packets`;
- do not depend on TerraRuntime, TShock, Terraria/XNA/MonoGame, ASP.NET Core or a transport framework;
- protocol models, packet views, buffer abstractions and codecs must remain usable independently;
- compiler/test tooling may have build-time dependencies, but those must never become dependencies of the
  shipped `Multiplicity` NuGet package or `Multiplicity.Packets.dll`.

Version 3.0 also removes the generator's NuGet Roslyn dependency: the generator targets .NET 11 and references
the Roslyn assemblies supplied by the active .NET SDK. xUnit remains test-only.

## Protocol support

The current baseline is **Terraria 1.4.6.1 / protocol 329**. The connection banner is `Terraria329`;
the vanilla desktop `MessageID.Count` is `166` (ids `0..165`, including reserved ids).

See [CHANGELOG.md](CHANGELOG.md) for packet changes and migration notes. Constructing a banner with
`ConnectRequest.BuildVersion` does not switch the codecs to an older protocol. The library does not
implement the game's socket transports or platform matchmaking.

`KillPortal (95)` uses the vanilla wire layout `ushort PlayerId + byte PortalColor`, so its payload is 3 bytes. The old `ProjectileIndex` property is retained only as an obsolete source-compatibility alias.

## What the library provides

- packet serialization and deserialization;
- typed packet models;
- a zero-copy `ReadOnlySpan<byte>` view layer for server hot paths;
- opt-in packet-extension overlays without mixing extension models into the vanilla packet folder;
- a source-generated vanilla packet registry with no runtime assembly scanning;
- packet and view names aligned with the Terraria naming where practical;
- no external runtime dependencies; the shipped runtime assembly depends only on the .NET BCL.

Wire compatibility is intentionally dependency-free. Terraria/XNA/MonoGame implementation types are not runtime dependencies of the parser: `Vector2` values are represented by their two `Single` wire components, RGB values use the library's own `ColorStruct`, and protocol-specific compound values such as player death reasons, sound info, item-sync data and raw tile payloads use Multiplicity-owned models with the same byte layout.

Target frameworks: `net9.0` and `net11.0`. One NuGet package contains both builds of the protocol 329 library.
Building the repository requires the .NET 11 SDK for the source generator. Consuming the `net9.0` package
asset does not require .NET 11. Running all tests requires both .NET 9 and .NET 11 runtimes.

The package is marked as AOT/trimming compatible and enables the .NET AOT and trimming analyzers:

```xml
<IsAotCompatible>true</IsAotCompatible>
<IsTrimmable>true</IsTrimmable>
<EnableTrimAnalyzer>true</EnableTrimAnalyzer>
<EnableAotAnalyzer>true</EnableAotAnalyzer>
```

A NativeAOT application can therefore consume the same NuGet package; a separate AOT NuGet package is not required.

### AOT-safe packet registration

Vanilla packet registration is generated at compile time by `Multiplicity.Packets.Generators`.

The generator discovers concrete packet models during compilation and emits direct code equivalent to:

```csharp
PacketRegistration.Fixed(
    PacketTypes.Zones,
    static reader => new Zones(reader),
    static () => new Zones());
```

There is no runtime `Assembly.GetTypes()`, constructor lookup, `Activator` or reflected `PayloadLength` lookup in the library registry path. The generated registry supplies both deserializers and packet metadata from the same compile-time source of truth. Legacy packet classes that intentionally remain only for source compatibility are marked `[PacketRegistrationIgnore]` and are excluded from generated vanilla registration.

## Install

```powershell
Install-Package Multiplicity
```

## Object model

Use the object model when a packet needs to be stored, modified or serialized again.

```csharp
using Multiplicity.Packets;

byte[] receiveBuffer = GetPacketBytes();
TerrariaPacket packet = TerrariaPacket.Deserialize(receiveBuffer, 0, receiveBuffer.Length);

switch (packet)
{
    case PlayerInfo playerInfo:
        Console.WriteLine($"Player #{playerInfo.PlayerId}: {playerInfo.Name}");
        break;

    case PlayerUpdate update:
        Console.WriteLine($"Player #{update.PlayerId}: X={update.PositionX}, Y={update.PositionY}");
        break;
}
```

Main vanilla entry points:

- `TerrariaPacket.Deserialize(...)`;
- `TerrariaPacket.DeserializePayload(...)`;
- `TerrariaPacket.TryDeserialize(...)`;
- `TerrariaPacket.TryDeserializePayload(...)`;
- `TerrariaPacket.ToArray(...)`;
- `TerrariaPacket.ToPayloadArray()`;
- `TerrariaPacket.TrySerialize(...)`;
- `TerrariaPacket.WriteTo(IBufferWriter<byte>)`;
- `TerrariaPacket.WritePayloadTo(IBufferWriter<byte>)`;
- `TerrariaPacket.ToStream(...)`.

### Bounded buffers and segmented input

Multiplicity 3.0 moves generic packet-buffer mechanics into the packet library itself so consumers do not need
to wrap `Stream` or coalesce segmented network buffers in their own adapter layers. These APIs use only BCL
types and do not add runtime package dependencies.

`WriteTo(IBufferWriter<byte>)` reserves the packet's declared final size, serializes directly into that memory and
advances the writer only when the serializer wrote exactly that many bytes. Under-write and over-write therefore
fail closed instead of publishing a partial or padded packet:

```csharp
using System.Buffers;
using Multiplicity.Packets;

var output = new ArrayBufferWriter<byte>();
var packet = new Ping();
packet.WriteTo(output);
ReadOnlySpan<byte> frame = output.WrittenSpan;
```

`TrySerialize(...)` provides the same exact-length invariant when an owned final array is wanted. `ToArray(...)`
and `ToPayloadArray()` now use that exact-size path as well.

For receive paths built on pipelines or scatter/gather buffers, framed packets and payloads can be decoded
directly from `ReadOnlySequence<byte>`. Single-segment sequences are borrowed without a copy; multi-segment
sequences are coalesced through a bounded `ArrayPool<byte>` lease that is returned before the decoded object
escapes the call:

```csharp
ReadOnlySequence<byte> frame = GetFrame();
if (TerrariaPacket.TryDeserialize(in frame, out TerrariaPacket packet))
{
    // typed packet model
}
```

The library remains transport-agnostic: it does not depend on `System.IO.Pipelines`, sockets, ASP.NET Core,
TerraRuntime, Terraria/XNA/MonoGame or any other external runtime package. `IBufferWriter<byte>` and
`ReadOnlySequence<byte>` come from the .NET BCL. The source-generator project also targets `net11.0` in 3.0 and references the Roslyn assemblies shipped with
the active .NET SDK rather than restoring `Microsoft.CodeAnalysis` packages. It is a compiler-time build tool,
not a runtime dependency of the `Multiplicity` package.

For hooks such as TSAPI/TShock `NetGetData`, where the packet id is already known and the receive buffer contains a payload slice:

```csharp
PacketTypes packetType = PacketTypes.ProjectileNew;
TerrariaPacket packet = TerrariaPacket.DeserializePayload(
    packetType,
    readBuffer,
    payloadOffset,
    payloadLength);
```

## Packet extensions

Extension protocols live under `Multiplicity.Packets/Extensions` and are kept separate from vanilla packet models.

The extension-aware decoder applies registered extension decoders **before** the vanilla decoder for the same byte id:

```text
packet id
   ↓
extension overlay
   ↓ if not claimed
vanilla decoder
```

This matters for protocols such as Dimensions, which intentionally reuses vanilla-unused packet id `67`.

```csharp
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Extensions.Dimensions;

TerrariaPacket packet = PacketDecoder.CommonExtensions.Decode(receiveBuffer);

if (packet is DimensionsMessage dimensions && dimensions.IsRealIp)
{
    Console.WriteLine(dimensions.ReportedRealIp);
}
```

The upstream Dimensions extension carried on packet `67` has payload:

```text
Int16 messageType
string payload
```

`messageType = 0` reports the client IP observed by the Dimensions proxy. This value is an extension report and is not authenticated by the vanilla Terraria protocol, so consumers should only trust it from a trusted proxy path.

The legacy opaque mobile packet models are still available behind an explicit overlay:

- `Extensions.Mobile.ServerInfo`, packet id `163`;
- `Extensions.Mobile.PlayerPlatformInfo`, packet id `164`.

Use `new PacketDecoder(PacketExtensions.Mobile)` only for legacy traffic you want to preserve as opaque bytes.
For protocol 329, use the vanilla packet models and views. `PacketDecoder.CommonExtensions` still
enables Dimensions, but no longer overrides vanilla ids `163/164`.

## Zero-copy packet views

Use `Multiplicity.Packets.Views` when the receive path only needs to inspect a few fields and should avoid materializing a full packet object.

```csharp
using Multiplicity.Packets;
using Multiplicity.Packets.Views;

ReadOnlySpan<byte> buffer = receiveBuffer.AsSpan(offset, availableBytes);

if (PacketViewParser.TryParse(buffer, out PacketView packetView, out int consumed))
{
    if (packetView.PacketType == PacketTypes.PlayerUpdate)
    {
        PlayerUpdateView view = packetView.AsPlayerUpdateView();
        Console.WriteLine($"Player #{view.PlayerId}: X={view.PositionX}, Y={view.PositionY}");
    }
}
```

For payload-only buffers:

```csharp
PacketView packetView = PacketViewParser.ParsePayload(
    PacketTypes.PlayerUpdate,
    receiveBuffer,
    payloadOffset,
    payloadLength);

PlayerUpdateView view = packetView.AsPlayerUpdateView();
```

`TryParsePayload(...)` treats the supplied length as the exact payload length. `TryParsePayloadBounded(...)` treats it as an upper bound, which is useful for receive buffers that may contain trailing data.

A payload-only `PacketView` has no complete Terraria packet header, so `HasPacketSpan` is `false`. Use `SourceSpan` when code should work with either full-packet or payload-only views.

## Net modules

`LoadNetModule` and `TerrariaNetModule` use the Terraria 1.4.6.1 registration order, unchanged from the audited 1.4.5.8 baseline. The currently registered module ids are:

| Id | Module |
|---:|---|
| 0 | `NetLiquidModule` |
| 1 | `NetTextModule` |
| 2 | `NetPingModule` |
| 3 | `NetAmbienceModule` |
| 4 | `NetBestiaryModule` |
| 5 | `NetCreativeUnlocksModule` |
| 6 | `NetCreativePowersModule` |
| 7 | `NetCreativeUnlocksPlayerReportModule` |
| 8 | `NetTeleportPylonModule` |
| 9 | `NetParticlesModule` |
| 10 | `NetCreativePowerPermissionsModule` |
| 11 | `NetBannersModule` |
| 12 | `NetCraftingRequestsModule` |
| 13 | `LeashedEntityModule` |
| 14 | `UnbreakableWallScanModule` |

The older `TagEffectModule` is not part of the current registration table and has been removed from the active API.

## Project layout

- `Multiplicity.Packets/Core` - packet framing and generated-registry runtime support;
- `Multiplicity.Packets/Packets` - vanilla typed packet object models;
- `Multiplicity.Packets/Extensions` - opt-in extension overlays such as Dimensions and mobile-family packets;
- `Multiplicity.Packets/NetModules` - `NetManager` module models;
- `Multiplicity.Packets/Views` - low-allocation packet views;
- `Multiplicity.Packets.Generators` - compile-time vanilla packet registry generator;
- `Multiplicity.Packets.Tests` - protocol and serialization regression tests.

## Summary

Multiplicity provides both a conventional packet object model and a low-allocation view API, with the current wire baseline set to **Terraria 1.4.6.1 / protocol 329** and non-vanilla packet protocols isolated behind opt-in extension overlays.
