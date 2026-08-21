# Multiplicity

`Multiplicity.Packets` is a low-level C# library for reading and writing Terraria network packets.

NuGet package id: `TZ.Multiplicity`. The assembly and public namespace remain `Multiplicity.Packets`.

## Protocol support

The current baseline is **Terraria 1.4.5.7 / protocol 325**.

- connection handshake: `Terraria325`;
- vanilla desktop `MessageID.Count`: `163`;
- vanilla desktop packet ids: `0..162`;
- highest vanilla packet id: `DamageNPCAck (162)`;
- `ServerInfo (163)` and `PlayerPlatformInfo (164)` are Multiplicity/TSAPI-family extension ids and are not part of vanilla desktop `MessageID`.

Important protocol-325 changes covered by the library include:

- NPC slot + generation identity in NPC synchronization and damage packets;
- 32-bit `ProjectileKey` identity in projectile create/destroy synchronization;
- current world-item sync and item ownership layouts;
- `RemoveItemOwner (39)` force-assignment flag;
- `DamageNPCAck (162)`;
- additional `WorldInfo` seed/world flags used by 1.4.5.7;
- current `NetManager` module registration order, including restored `NetCreativeUnlocksModule`;
- packet ids `145` and `148` are treated as deprecated in protocol 325 and are not registered for normal runtime deserialization.

`KillPortal (95)` uses the vanilla wire layout `ushort PlayerId + byte PortalColor`, so its payload is 3 bytes. The old `ProjectileIndex` property is retained only as an obsolete source-compatibility alias.

## What the library provides

- packet serialization and deserialization;
- typed packet models;
- a zero-copy `ReadOnlySpan<byte>` view layer for server hot paths;
- packet and view names aligned with the TSAPI/Terraria naming where practical;
- no external runtime dependencies.

Target framework: `net9.0`.

## Install

```powershell
Install-Package TZ.Multiplicity
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

Main entry points:

- `TerrariaPacket.Deserialize(...)`;
- `TerrariaPacket.DeserializePayload(...)`;
- `TerrariaPacket.ToArray(...)`;
- `TerrariaPacket.ToPayloadArray()`;
- `TerrariaPacket.ToStream(...)`.

For hooks such as TSAPI/TShock `NetGetData`, where the packet id is already known and the receive buffer contains a payload slice:

```csharp
PacketTypes packetType = PacketTypes.ProjectileNew;
TerrariaPacket packet = TerrariaPacket.DeserializePayload(
    packetType,
    readBuffer,
    payloadOffset,
    payloadLength);
```

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

`LoadNetModule` and `TerrariaNetModule` use the Terraria 1.4.5.7 registration order. The currently registered module ids are:

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

The older `TagEffectModule` is not part of the protocol-325 registration table and has been removed from the active API.

## Project layout

- `Multiplicity.Packets/Core` - packet framing, metadata and protocol registration;
- `Multiplicity.Packets/Packets` - typed packet object models;
- `Multiplicity.Packets/NetModules` - `NetManager` module models;
- `Multiplicity.Packets/Views` - low-allocation packet views;
- `Multiplicity.Packets.Tests` - protocol and serialization regression tests.

## Summary

Multiplicity provides both a conventional packet object model and a low-allocation view API, with the current wire baseline set to **Terraria 1.4.5.7 / protocol 325**.
