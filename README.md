# Multiplicity

`Multiplicity.Packets` - это низкоуровневая библиотека для чтения и формирования сетевых пакетов Terraria.

NuGet package ID:

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
- поддержка протокола `317-318`;
- zero-copy разбор через `ReadOnlySpan<byte>` для server hot-path;
- typed views для часто используемых и modern packet types.

## Ограничения

- `ServerInfo (162)` и `PlayerPlatformInfo (163)` пока остаются raw-placeholder пакетами;
- для этих двух пакетов нет подтверждённого wire-layout по актуальному исходнику/декомпилу Terraria.

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
- typed views вида `UpdatePlayerView`, `PlayerInfoView`, `ProjectileUpdateView` и т.д.

`PacketView` работает поверх исходного буфера.  
Это значит, что `ReadOnlySpan<byte>` внутри views живёт столько же, сколько живёт исходный packet slice.

## Покрытие typed views

Сейчас через `Multiplicity.Packets.Views` покрыты:

- базовые hot-path пакеты:
  - `PlayerInfo`
  - `PlayerInventorySlot`
  - `GetSection`
  - `UpdatePlayer`
  - `ProjectileUpdate`
  - `NPCUpdate`
  - `ChatMessage`
  - `ModifyTile`
  - `PlaceObject`
  - `LoadNetModule`
  - `WorldItemSync`
  - `PlayerHurtV2`
  - `PlayerDeathV2`
- player state пакеты:
  - `SpawnPlayer`
  - `PlayerActive`
  - `PlayerHP`
  - `PlayerMana`
  - `PlayerTeam`
  - `TogglePVP`
  - `PlayerZone`
  - `PlayerItemAnimation`
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
        Console.WriteLine($"Player #{playerInfo.PlayerID}: {playerInfo.Name}");
        break;

    case UpdatePlayer updatePlayer:
        Console.WriteLine($"Player #{updatePlayer.PlayerID}: X={updatePlayer.PositionX}, Y={updatePlayer.PositionY}");
        break;
}
```

Этот вариант проще для логики, где пакет потом нужно хранить, менять и сериализовать назад.

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
        case PacketTypes.UpdatePlayer:
        {
            UpdatePlayerView view = packetView.AsUpdatePlayerView();
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

## Пример 3. Сформировать пакет и отправить

```csharp
using Multiplicity.Packets;

var packet = new ChatMessage
{
    PlayerID = 255,
    Message = "Hello from Multiplicity",
    MessageColor = new ColorStruct
    {
        R = 255,
        G = 240,
        B = 120
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
- typed packet coverage для протокола Terraria `317-318`.
