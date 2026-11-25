# Changelog

## 4.0.0 - 2026-11-25

The NuGet package now includes a `net9.0` assembly alongside `net11.0`. Both target the same protocol;
the source generator still runs under the .NET 11 SDK when building the repository.

### Terraria 1.4.6.1 / protocol 329

Confirmed changes from Terraria 1.4.5.8 / protocol 326:

| Packet | Protocol 329 change |
| --- | --- |
| `ConnectRequest (1)` | Default banner changes from `Terraria326` to `Terraria329`. |
| `WorldInfo (7)` | Bit 5 of `EventInfo11` is `NoSpidersSeed`; payload size and dungeon-coordinate tail are unchanged. |
| `PlayerAnimation (41)` | Adds an `Int16 ItemAnimationMax` after `ItemAnimation`. |
| `TweakItem (88)` | Drops the `Width` and `Height` fields from the API and wire format. Other secondary bit positions stay unchanged. |
| `EmoteBubble (91)` | The anchor carries an NPC `UInt16`, a player `Byte`, a 32-bit projectile key, or no value. Removal (`255`) ends the payload at the anchor tag. Other tags have `UInt16` lifetime and `Byte` emote, without extra metadata. |
| `PlayerHurtV2 (117)` | Bit 2 of `Flags` is `Dodgeable`; payload size is unchanged. |
| `ServerInfo (163)` | New vanilla id: empty client request or a typed server response with an optional remote address and port. |
| `PlayerPlatformInfo (164)` | New vanilla id: player slot, platform, user ID, a Nintendo-only name and a host flag. |
| `CustomServerAck (165)` | New vanilla id with one 7-bit encoded `Int32`. |

`PacketDecoder.CommonExtensions` no longer overrides vanilla ids `163/164`. Use
`new PacketDecoder(PacketExtensions.Mobile)` for legacy opaque mobile packets.

The audit compared `MessageID`, `NetMessage`, `MessageBuffer`, and network module registration in the
Windows dedicated-server assemblies. The 15 `NetManager` module ids and packet framing are unchanged.
`NpcSpecial (51)` now handles the existing cake action (10) without a layout change.
The audited 1.4.6.1 `TerrariaServer.exe` SHA-256 is
`e6d7db36e8ababc465490743e03f46f3c76d2fcdb757239e60a2b139a92f03a1`.

## Previous protocol baselines

### Terraria 1.4.5.8 / protocol 326

- The connection banner changed from `Terraria325` to `Terraria326`.
- `WorldInfo (7)` kept the same eleven world/event flag bytes and appended `Int16 DungeonX` and `Int16 DungeonY` after the extra-spawn-point list.
- The `NetManager` module registration order stayed unchanged from protocol 325.
- Audited layouts for `OpenSignResponse (47)`, `LiquidUpdate (48)`, `UniqueTownNPCInfoSyncRequest (56)`, `QuestsCountSync (76)`, `QuickStackChests (85)`, `MoonlordHorror (103)` and `SpecialFX (112)`.
- `NpcUpdate (23)` handles catchable-NPC release ownership and selects 1-, 2-, or 4-byte life values automatically.
- Packet ids `24`, `25`, `26`, `44`, `83`, `145` and `148` are not registered as active vanilla packets. Legacy `LiquidUpdate (48)` remains registered alongside `NetLiquidModule`.

### Terraria 1.4.5.x

- NPC synchronization and damage packets use NPC slot and generation identity.
- Projectile create/destroy synchronization and projectile trackers use 32-bit `ProjectileKey` identity.
- Updated world-item synchronization and item ownership layouts, including the `RemoveItemOwner (39)` force-assignment flag.
- `NpcUpdateBuff (54)` uses variable-length `UInt16` buff ids and times with a zero terminator.
- Added `DamageNPCAck (162)`, updated world/seed flags, and restored `NetCreativeUnlocksModule` to the registration order.
