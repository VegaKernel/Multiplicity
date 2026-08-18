# Multiplicity

`Multiplicity.Packets` - это низкоуровневая библиотека для чтения и формирования сетевых пакетов Terraria.

NuGet package identifier:

- `TZ.Multiplicity`

При этом namespace и имя основной сборки остаются прежними:

- `Multiplicity.Packets`

Она нужна в двух типовых сценариях:

- когда удобно работать с пакетами как с обычными C#-объектами;
- когда нужен максимально дешёвый разбор входящего буфера без лишних копий и без создания object-модели.

Библиотека написана на чистом C#, без внешних зависимостей.

## Что умеет

- сериализация и десериализация пакетов Terraria;
- enum и typed-модели пакетов `1..163`;
- поддержка desktop-протокола `319` (`Terraria 1.4.5.6`);
- zero-copy разбор через `ReadOnlySpan<byte>` для server hot-path;
- публичные имена `PacketTypes`, packet classes и packet views выровнены под `TerrariaApi.Server.PacketTypes` из TSAPI.

## Protocol 319

В `Terraria 1.4.5.6` сетевой handshake использует строку `Terraria319`. `ConnectRequest` по умолчанию формирует именно её через `ConnectRequest.CurrentProtocol`.

При сравнении desktop-протоколов `318` (`Terraria 1.4.5.5`) и `319` (`Terraria 1.4.5.6`) таблица `MessageID` не расширилась: последний desktop packet id остаётся `HostToken (161)`, а `MessageID.Count` равен `162`.

В рамках перехода на protocol 319 также исправлена ранее существовавшая ошибка модели `KillPortal (95)`: wire-layout этого пакета состоит из `ushort PlayerId` и `byte PortalColor`, то есть payload имеет длину 3 байта. Старое свойство `ProjectileIndex` сохранено как obsolete-алиас к `PlayerId` для исходной совместимости.

## Ограничения

- `ServerInfo (162)` и `PlayerPlatformInfo (163)` являются mobile-only расширениями списка `PacketTypes` в TSAPI и не входят в desktop `MessageID` Terraria 1.4.5.6;
- эти два mobile-пакета пока остаются raw-placeholder пакетами, поскольку их wire-layout не подтверждён по desktop-исходнику Terraria.

## Установка

NuGet-пакет:

```powershell
Install-Package TZ.Multiplicity
```

Текущий target framework проекта:

- `net9.0`

## Два слоя API

В библиотеке есть два разных слоя, и у каждого своя задача.

### 1. Object model

Namespace:

```csharp
using Multiplicity.Packets;
```

Этот слой удобен, когда нужно:

- десериализовать пакет в обычный класс;
- изменить поля;
- сериализовать пакет обратно;
- работать не в самом горячем участке серверного кода.

Главная точка входа:

- `TerrariaPacket.Deserialize(...)`

Поддерживаемые варианты:

- `TerrariaPacket.Deserialize(BinaryReader br, byte id)`
- `TerrariaPacket.Deserialize(ReadOnlyMemory<byte> packetBuffer)`
- `TerrariaPacket.Deserialize(byte[] buffer, int offset, int length)`
- `TerrariaPacket.DeserializePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer)`
- `TerrariaPacket.DeserializePayload(PacketTypes packetType, byte[] buffer, int offset, int length)`

### 2. Packet views / zero-copy layer

Namespace:

```csharp
using Multiplicity.Packets.Views;
```

Этот слой нужен для server hot-path, когда важно:

- не копировать весь пакет;
- не создавать `TerrariaPacket`-объект;
- быстро читать только нужные поля.

Главные типы:

- `PacketViewParser`
- `PacketView`
- `PacketReader`
- typed views вида `PlayerUpdateView`, `PlayerInfoView`, `ProjectileNewView` и т.д.

`PacketView` работает поверх исходного буфера.  
Это значит, что `ReadOnlySpan<byte>` внутри views живёт столько же, сколько живёт исходный packet slice.

Для server-side `NetGetData`, где у тебя часто есть только payload без Terraria header, есть отдельный вход:

- `PacketViewParser.ParsePayload(PacketTypes packetType, ReadOnlySpan<byte> payloadBuffer)`
- `PacketViewParser.ParsePayload(PacketTypes packetType, byte[] buffer, int offset, int length)`
- `PacketViewParser.TryParsePayload(...)`
- `PacketViewParser.TryParsePayloadBounded(...)` for upper-bound payload windows such as `NetGetData`

Important:
- `TryParsePayload(..., buffer, offset, length, ...)` treats `length` as an exact payload length.
- `TryParsePayloadBounded(...)` treats `availableLength` as an upper bound and allows trailing bytes after the parsed payload.

## Покрытие typed views

Сейчас через `Multiplicity.Packets.Views` покрыты:

- базовые hot-path пакеты:
  - `PlayerInfo`
  - `PlayerSlot`
  - `TileGetSection`
  - `PlayerUpdate`
  - `ProjectileNew`
  - `NpcUpdate`
  - `ChatMessage`
  - `Tile`
  - `PlaceObject`
  - `LoadNetModule`
  - `WorldItemSync`
  - `PlayerHurtV2`
  - `PlayerDeathV2`
- player state пакеты:
  - `PlayerSpawn`
  - `PlayerActive`
  - `PlayerHp`
  - `PlayerMana`
  - `PlayerTeam`
  - `TogglePvp`
  - `Zones`
  - `PlayerAnimation`
  - `PlayerDamage`
- modern typed packets:
  - `Emoji`
  - `TileEntityDisplayDollItemSync`
  - `RequestTileEntityInteraction`
  - `TileEntityHatRackItemSync`
  - `SyncTilePicking`
  - `SyncRevengeMarker`
  - `RemoveRevengeMarker`
  - `LandGolfBallInCup`
  - `FishOutNPC`
  - `TamperWithNPC`
  - `PlayLegacySound`
  - `UpdatePlayerLuckFactors`
  - `DeadPlayer`
  - `SyncCavernMonsterType`
  - `RequestNPCBuffRemoval`
  - `SetCountsAsHostForGameplay`
  - `SetMiscEventValues`
  - `RequestLucyPopup`
  - `SyncProjectileTrackers`
  - `ShimmerActions`
  - `SyncLoadout`
  - `SpectatePlayer`
  - `SyncItemDespawn`
  - `ItemUseSound`
  - `NpcHurtByDebuff`
  - `TELeashedEntityAnchorPlaceItem`
  - `TeamChangeFromUI`
  - `ExtraSpawnSectionLoaded`
  - `RequestSection`
  - `SyncItemPosition`
- generic view-patterns:
  - `StringPacketView`
  - `Utf8StringPacketView`
  - `EmptyPayloadPacketView`
  - `TileEntityItemSyncPacketView`

## Пример 1. Разобрать пакет как объект

Когда нужна обычная object-модель:

```csharp
using Multiplicity.Packets;

byte[] receiveBuffer = GetPacketBytes();

TerrariaPacket packet = TerrariaPacket.Deserialize(receiveBuffer, 0, receiveBuffer.Length);

switch (packet)
{
    case PlayerInfo playerInfo:
        Console.WriteLine($"Player #{playerInfo.PlayerId}: {playerInfo.Name}");
        break;

    case PlayerUpdate updatePlayer:
        Console.WriteLine($"Player #{updatePlayer.PlayerId}: X={updatePlayer.PositionX}, Y={updatePlayer.PositionY}");
        break;
}
```

Этот вариант проще для логики, где пакет потом нужно хранить, менять и сериализовать назад.

## Пример 1.1. Разобрать только payload, если тип пакета уже известен

Это полезно для `TSAPI/TShock NetGetData`, где хук уже дал `MsgID`, `Index` и `Length`, а в буфере лежит только payload-срез:

```csharp
using Multiplicity.Packets;

PacketTypes packetType = PacketTypes.ProjectileNew;
byte[] readBuffer = GetReceiveBuffer();
int offset = payloadOffset;
int length = payloadLength;

TerrariaPacket packet = TerrariaPacket.DeserializePayload(packetType, readBuffer, offset, length);

if (packet is ProjectileNew projectile)
{
    Console.WriteLine($"Projectile #{projectile.Identity}: type={projectile.Type}, owner={projectile.Owner}");
}
```

## Пример 2. Zero-copy разбор входящего буфера

Когда пакет приходит в server receive buffer и важна производительность:

```csharp
using Multiplicity.Packets;
using Multiplicity.Packets.Views;

ReadOnlySpan<byte> buffer = receiveBuffer.AsSpan(offset, availableBytes);

if (PacketViewParser.TryParse(buffer, out PacketView packetView, out int consumed))
{
    switch (packetView.PacketType)
    {
        case PacketTypes.PlayerUpdate:
        {
            PlayerUpdateView view = packetView.AsPlayerUpdateView();
            Console.WriteLine($"Player #{view.PlayerId}: X={view.PositionX}, Y={view.PositionY}");
            break;
        }

        case PacketTypes.PlayerInfo:
        {
            PlayerInfoView view = packetView.AsPlayerInfoView();
            Console.WriteLine($"Player name: {view.GetName()}");
            break;
        }
    }

    // consumed = длина первого пакета в буфере
}
```

Здесь:

- весь пакет не копируется;
- `TerrariaPacket`-объект не создаётся;
- строки можно не материализовывать до последнего момента.

## Пример 2.1. Zero-copy разбор только payload без Terraria header

```csharp
using Multiplicity.Packets;
using Multiplicity.Packets.Views;

PacketTypes packetType = PacketTypes.PlayerUpdate;
ReadOnlySpan<byte> payload = receiveBuffer.AsSpan(payloadOffset, payloadLength);

PacketView packetView = PacketViewParser.ParsePayload(packetType, payload);
PlayerUpdateView view = packetView.AsPlayerUpdateView();

Console.WriteLine($"Player #{view.PlayerId}: X={view.PositionX}, Y={view.PositionY}");
```

Важно:

- у payload-only `PacketView` нет полного Terraria header;
- `packetView.HasPacketSpan` в таком случае будет `false`;
- `packetView.GetPacketSpan()` бросит исключение;
- если нужен исходный slice без различия packet/payload, используй `packetView.SourceSpan` или `view.SourceSpan`.

Это основной сценарий для серверных inbound-хуков, где тип пакета уже известен, а лишний `MemoryStream/BinaryReader` не нужен.

## Пример 3. Сформировать пакет и отправить

```csharp
using Multiplicity.Packets;

var packet = new LoadNetModule
{
    LoadedModule = new NetTextModule
    {
        PayloadKind = NetTextModulePayloadKind.ServerChatMessage,
        AuthorId = 255,
        ServerText = new NetworkText
        {
            Text = "Hello from Multiplicity",
            TextMode = NetworkText.Mode.Literal
        },
        MessageColor = new ColorStruct
        {
            R = 255,
            G = 240,
            B = 120
        }
    }
};

byte[] bytes = packet.ToArray(includeHeader: true);
Send(bytes);
```

Если не нужен промежуточный массив, можно писать сразу в поток:

```csharp
using Multiplicity.Packets;

using Stream stream = GetNetworkStream();

var packet = new Ping();
packet.ToStream(stream, includeHeader: true);
```

Если нужно сформировать только payload без Terraria header:

```csharp
using Multiplicity.Packets;

var packet = new ProjectileNew
{
    Identity = 42,
    Type = 12,
    Owner = 1
};

byte[] payload = packet.ToPayloadArray();
SendPayloadOnly(payload);
```

## Пример 4. Работа со строковыми packet views без лишней аллокации

```csharp
using Multiplicity.Packets;
using Multiplicity.Packets.Views;

if (PacketViewParser.TryParse(buffer, out PacketView packetView, out _)
    && packetView.PacketType == PacketTypes.HostToken)
{
    Utf8StringPacketView view = new Utf8StringPacketView(packetView, PacketTypes.HostToken);

    // Если нужна строка:
    string token = view.GetString();

    // Если нужна только проверка/сравнение, можно работать с Utf8Bytes
    ReadOnlySpan<byte> utf8 = view.Utf8Bytes;
}
```

## Когда какой слой использовать

Используй `TerrariaPacket`, если:

- важна простота;
- пакет нужно мутировать;
- пакет нужно сериализовать обратно;
- горячий путь не критичен.

Используй `PacketView`, если:

- это `NetGetData` / receive hot-path;
- нужно быстро читать 1-2 поля и принимать решение;
- хочется минимум аллокаций;
- не нужен полноценный объект пакета.

## Полезные файлы в проекте

- [TerrariaPacket.cs](C:\Users\Xekep\Documents\GitHub\Multiplicity\Multiplicity.Packets\Core\TerrariaPacket.cs)
- [PacketViewParser.cs](C:\Users\Xekep\Documents\GitHub\Multiplicity\Multiplicity.Packets\Views\Core\PacketViewParser.cs)
- [PacketReader.cs](C:\Users\Xekep\Documents\GitHub\Multiplicity\Multiplicity.Packets\Views\Core\PacketReader.cs)
- [PacketViewExtensions.cs](C:\Users\Xekep\Documents\GitHub\Multiplicity\Multiplicity.Packets\Views\Core\PacketViewExtensions.cs)

## Итог

`Multiplicity.Packets` сейчас даёт:

- object-model для удобной логики и сериализации;
- zero-copy views для производительного server-side разбора;
- typed packet coverage для desktop-протокола Terraria `319`.
