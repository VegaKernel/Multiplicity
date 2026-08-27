using System;
using System.IO;
using Multiplicity.Packets;
using Multiplicity.Packets.Models;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326Terraria1458WireAuditTests
    {
        [Fact]
        public void SignNew_UsesTerraria1458FlagsTail()
        {
            SignNew packet = new SignNew
            {
                SignId = 12,
                X = 345,
                Y = 678,
                Text = "hello",
                PlayerId = 9,
                Flags = 1
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((short)12);
                bw.Write((short)345);
                bw.Write((short)678);
                bw.Write("hello");
                bw.Write((byte)9);
                bw.Write((byte)1);
            });

            Assert.Equal(expected, packet.ToPayloadArray());
            SignNew parsed = Assert.IsType<SignNew>(TerrariaPacket.DeserializePayload(PacketTypes.SignNew, expected));
            Assert.Equal((byte)1, parsed.Flags);
            Assert.True(parsed.SuppressOpen);
        }

        [Fact]
        public void LiquidSet_RemainsActiveWithLegacySixByteWireShape()
        {
            LiquidSet packet = new LiquidSet
            {
                X = 123,
                Y = -456,
                Liquid = 200,
                LiquidType = (LiquidKind)2
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((short)123);
                bw.Write((short)-456);
                bw.Write((byte)200);
                bw.Write((byte)2);
            });

            Assert.Equal(6, packet.GetLength());
            Assert.Equal(expected, packet.ToPayloadArray());
            Assert.True(TerrariaPacket.DeserializerMap.ContainsKey(PacketTypes.LiquidSet));
            Assert.IsType<LiquidSet>(TerrariaPacket.DeserializePayload(PacketTypes.LiquidSet, expected));
        }

        [Fact]
        public void UpdateNpcName_SupportsRequestAndServerResponseShapes()
        {
            UpdateNPCName request = new UpdateNPCName { NpcId = 17 };
            Assert.Equal(new byte[] { 17, 0 }, request.ToPayloadArray());

            UpdateNPCName response = new UpdateNPCName
            {
                NpcId = 17,
                HasNameData = true,
                Name = "Molly",
                TownNpcVariationIndex = 0x12345678
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((short)17);
                bw.Write("Molly");
                bw.Write(0x12345678);
            });

            Assert.Equal(expected, response.ToPayloadArray());
            UpdateNPCName parsed = Assert.IsType<UpdateNPCName>(
                TerrariaPacket.DeserializePayload(PacketTypes.UpdateNPCName, expected));
            Assert.True(parsed.HasNameData);
            Assert.Equal("Molly", parsed.Name);
            Assert.Equal(0x12345678, parsed.TownNpcVariationIndex);
        }

        [Fact]
        public void QuestsCountSync_CarriesAnglerAndGolferCounters()
        {
            NumberOfAnglerQuestsCompleted packet = new NumberOfAnglerQuestsCompleted
            {
                PlayerId = 4,
                AnglerQuestsCompleted = 1234,
                GolferScoreAccumulated = 5678
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((byte)4);
                bw.Write(1234);
                bw.Write(5678);
            });

            Assert.Equal(9, packet.GetLength());
            Assert.Equal(expected, packet.ToPayloadArray());
            NumberOfAnglerQuestsCompleted parsed = Assert.IsType<NumberOfAnglerQuestsCompleted>(
                TerrariaPacket.DeserializePayload(PacketTypes.NumberOfAnglerQuestsCompleted, expected));
            Assert.Equal(5678, parsed.GolferScoreAccumulated);
        }

        [Fact]
        public void QuickStackChests_ParsesClientInventoryRequest()
        {
            ForceItemIntoNearestChest packet = new ForceItemIntoNearestChest
            {
                PayloadKind = QuickStackChestsPayloadKind.InventoryRequest,
                InventorySlots = new short[] { 3, 71, 120 },
                SmartStack = true
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write(3);
                bw.Write((short)3);
                bw.Write((short)71);
                bw.Write((short)120);
                bw.Write(true);
            });

            Assert.Equal(expected, packet.ToPayloadArray());
            ForceItemIntoNearestChest parsed = Assert.IsType<ForceItemIntoNearestChest>(
                TerrariaPacket.DeserializePayload(PacketTypes.ForceItemIntoNearestChest, expected));
            Assert.Equal(QuickStackChestsPayloadKind.InventoryRequest, parsed.PayloadKind);
            Assert.Equal(new short[] { 3, 71, 120 }, parsed.InventorySlots);
            Assert.True(parsed.SmartStack);
        }

        [Fact]
        public void QuickStackChests_ParsesServerBlockedChestList()
        {
            ForceItemIntoNearestChest packet = new ForceItemIntoNearestChest
            {
                PayloadKind = QuickStackChestsPayloadKind.BlockedChestList,
                BlockedChestIds = new ushort[] { 2, 400, 7999 }
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write(3);
                bw.Write((ushort)2);
                bw.Write((ushort)400);
                bw.Write((ushort)7999);
            });

            Assert.Equal(expected, packet.ToPayloadArray());
            ForceItemIntoNearestChest parsed = Assert.IsType<ForceItemIntoNearestChest>(
                TerrariaPacket.DeserializePayload(PacketTypes.ForceItemIntoNearestChest, expected));
            Assert.Equal(QuickStackChestsPayloadKind.BlockedChestList, parsed.PayloadKind);
            Assert.Equal(new ushort[] { 2, 400, 7999 }, parsed.BlockedChestIds);
        }

        [Fact]
        public void MoonLordCountdown_CarriesMaximumAndCurrentCounters()
        {
            MoonLordCountdown packet = new MoonLordCountdown
            {
                MaxCountdownValue = 3600,
                CountdownValue = 1234
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write(3600);
                bw.Write(1234);
            });

            Assert.Equal(8, packet.GetLength());
            Assert.Equal(expected, packet.ToPayloadArray());
            MoonLordCountdown parsed = Assert.IsType<MoonLordCountdown>(
                TerrariaPacket.DeserializePayload(PacketTypes.MoonLordCountdown, expected));
            Assert.Equal(3600, parsed.MaxCountdownValue);
            Assert.Equal(1234, parsed.CountdownValue);
        }

        [Fact]
        public void SpecialFx_UsesTerraria1458ThirteenByteShape()
        {
            TreeGrowFX packet = new TreeGrowFX
            {
                EffectType = 1,
                X = 123456,
                Y = -654321,
                Style = 7,
                Extra = 2222,
                ExtraFlag = true
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((byte)1);
                bw.Write(123456);
                bw.Write(-654321);
                bw.Write((byte)7);
                bw.Write((short)2222);
                bw.Write((byte)1);
            });

            Assert.Equal(13, packet.GetLength());
            Assert.Equal(expected, packet.ToPayloadArray());
            TreeGrowFX parsed = Assert.IsType<TreeGrowFX>(
                TerrariaPacket.DeserializePayload(PacketTypes.TreeGrowFX, expected));
            Assert.Equal(123456, parsed.X);
            Assert.Equal(-654321, parsed.Y);
            Assert.True(parsed.ExtraFlag);
        }

        [Fact]
        public void PlayerInfo_UsesUInt16AccessoryVisibilityAndThreeByteRgb()
        {
            PlayerInfo packet = new PlayerInfo
            {
                PlayerId = 1,
                SkinVariant = 2,
                VoiceVariant = 3,
                Hair = 4,
                Name = "A",
                HairDye = 5,
                HideVisibleAccessory = 0x1234,
                HideMisc = 6,
                HairColor = Color(10, 11, 12),
                SkinColor = Color(20, 21, 22),
                EyeColor = Color(30, 31, 32),
                ShirtColor = Color(40, 41, 42),
                UnderShirtColor = Color(50, 51, 52),
                PantsColor = Color(60, 61, 62),
                ShoeColor = Color(70, 71, 72)
            };

            byte[] payload = packet.ToPayloadArray();

            Assert.Equal((byte)0x34, payload[11]);
            Assert.Equal((byte)0x12, payload[12]);
            Assert.Equal(new byte[] { 10, 11, 12 }, payload[14..17]);
            Assert.Equal(new byte[] { 70, 71, 72 }, payload[32..35]);
            Assert.Equal(38, payload.Length);
        }

        [Fact]
        public void PlayerDeathReason_WireFieldsMatchTerrariaOrderAndWidths()
        {
            PlayerDeathV2 packet = new PlayerDeathV2
            {
                PlayerId = 7,
                PlayerDeathReason = PlayerDeathReasonFlags.FromPlayerIndex |
                                    PlayerDeathReasonFlags.FromNpcIndex |
                                    PlayerDeathReasonFlags.FromProjectileIndex |
                                    PlayerDeathReasonFlags.FromOther |
                                    PlayerDeathReasonFlags.FromProjectileType |
                                    PlayerDeathReasonFlags.FromItemType |
                                    PlayerDeathReasonFlags.FromItemPrefix |
                                    PlayerDeathReasonFlags.FromCustomReason,
                FromPlayerIndex = 1,
                FromNpcIndex = 2,
                FromProjectileIndex = 3,
                FromOther = 4,
                FromProjectileType = 5,
                FromItemType = 6,
                FromItemPrefix = 7,
                FromCustomReason = "x",
                Damage = 8,
                HitDirection = 9,
                Flags = 10
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((byte)7);
                bw.Write((byte)0xFF);
                bw.Write((short)1);
                bw.Write((short)2);
                bw.Write((short)3);
                bw.Write((byte)4);
                bw.Write((short)5);
                bw.Write((short)6);
                bw.Write((byte)7);
                bw.Write("x");
                bw.Write((short)8);
                bw.Write((byte)9);
                bw.Write((byte)10);
            });

            Assert.Equal(expected, packet.ToPayloadArray());
        }

        [Fact]
        public void NetSoundInfo_UsesTwoSinglesAndPresenceFlagsWithoutXnaTypes()
        {
            PlayLegacySound packet = new PlayLegacySound
            {
                SoundInfo = new NetSoundInfoData
                {
                    PositionX = 1.25f,
                    PositionY = -2.5f,
                    SoundIndex = 123,
                    Style = 456,
                    Volume = 0.75f,
                    PitchOffset = -0.25f
                }
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write(1.25f);
                bw.Write(-2.5f);
                bw.Write((ushort)123);
                bw.Write((byte)0x07);
                bw.Write(456);
                bw.Write(0.75f);
                bw.Write(-0.25f);
            });

            Assert.Equal(expected, packet.ToPayloadArray());
        }

        [Fact]
        public void DisplayDollAndHatRack_UseDependencyFreeFiveByteItemData()
        {
            ItemSyncData item = new ItemSyncData { ItemType = 1000, Stack = 23, Prefix = 4 };
            TileEntityDisplayDollItemSync doll = new TileEntityDisplayDollItemSync
            {
                PlayerId = 2,
                TileEntityId = 123456,
                ItemIndex = 6,
                Command = DisplayDollSyncCommand.Equipment,
                ItemData = item
            };
            TileEntityHatRackItemSync rack = new TileEntityHatRackItemSync
            {
                PlayerId = 3,
                TileEntityId = 654321,
                EncodedSlot = 2,
                ItemData = new ItemSyncData { ItemType = 1000, Stack = 23, Prefix = 4 }
            };

            Assert.Equal(5, ItemSyncData.PayloadLength);
            Assert.Equal(12, doll.GetLength());
            Assert.Equal(11, rack.GetLength());
            Assert.Equal(new byte[] { 0xE8, 0x03, 0x17, 0x00, 0x04 }, doll.ToPayloadArray()[7..12]);
            Assert.Equal(new byte[] { 0xE8, 0x03, 0x17, 0x00, 0x04 }, rack.ToPayloadArray()[6..11]);
        }

        [Fact]
        public void WorldItemSync_UsesTwoVectorPairsAndOptionalShimmerAndGrabDelay()
        {
            ItemDrop packet = new ItemDrop
            {
                ItemIndex = 400,
                PositionX = 10.5f,
                PositionY = 20.25f,
                VelocityX = 1.5f,
                VelocityY = -0.75f,
                Stack = 99,
                Prefix = 8,
                Flags = WorldItemSyncFlags.HasShimmerData | WorldItemSyncFlags.HasEnemyGrabDelay,
                ItemNetId = 1234,
                Shimmered = true,
                ShimmerTime = 0.5f,
                EnemyGrabDelayTime = 77
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((short)400);
                bw.Write(10.5f);
                bw.Write(20.25f);
                bw.Write(1.5f);
                bw.Write(-0.75f);
                bw.Write((short)99);
                bw.Write((byte)8);
                bw.Write((byte)(WorldItemSyncFlags.HasShimmerData | WorldItemSyncFlags.HasEnemyGrabDelay));
                bw.Write((short)1234);
                bw.Write(true);
                bw.Write(0.5f);
                bw.Write((byte)77);
            });

            Assert.Equal(expected, packet.ToPayloadArray());
        }

        [Fact]
        public void ShimmerActions_UsesBytePlusVector2AndOptionalInt32()
        {
            ShimmerActions packet = new ShimmerActions
            {
                Action = ShimmerActionKind.AddCoinLuck,
                PositionX = 12.5f,
                PositionY = -9.25f,
                CoinLuckAmount = 123456
            };

            byte[] expected = BuildPayload(bw =>
            {
                bw.Write((byte)1);
                bw.Write(12.5f);
                bw.Write(-9.25f);
                bw.Write(123456);
            });

            Assert.Equal(13, packet.GetLength());
            Assert.Equal(expected, packet.ToPayloadArray());
        }

        [Fact]
        public void WorldInfo_UsesThirteenTreeTopVariationsAndKeepsMemoryCacheCoherent()
        {
            byte[] first = new byte[13];
            for (byte i = 0; i < first.Length; i++) first[i] = (byte)(i + 1);
            byte[] second = new byte[13];
            for (byte i = 0; i < second.Length; i++) second[i] = (byte)(100 + i);

            WorldInfo packet = new WorldInfo
            {
                TreeTopVariations = first,
                ExtraSpawnPoints = new[] { new WorldSpawnPoint(111, 222) },
                DungeonX = 333,
                DungeonY = 444
            };

            Assert.Equal(first, packet.TreeTopVariations);
            packet.TreeTopVariationsMemory = second;
            Assert.Equal(second, packet.TreeTopVariations);

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(second, payload[102..115]);
            WorldInfo parsed = Assert.IsType<WorldInfo>(TerrariaPacket.DeserializePayload(PacketTypes.WorldInfo, payload));
            Assert.Single(parsed.ExtraSpawnPoints);
            Assert.Equal((short)333, parsed.DungeonX);
            Assert.Equal((short)444, parsed.DungeonY);
        }

        [Fact]
        public void NpcUpdate_UsesConditionalAiLifeAndCurrentCatchableReleaseOwnerTable()
        {
            Assert.Contains((short)688, NpcUpdate.NpcCatchable);

            NpcUpdate packet = new NpcUpdate
            {
                NpcSlot = 4,
                Generation = 5,
                PositionX = 1f,
                PositionY = 2f,
                VelocityX = 3f,
                VelocityY = 4f,
                Target = 6,
                AI = new[] { 0f, 7.5f, 0f, 0f },
                NpcNetId = 688,
                Life = 40000,
                ReleaseOwner = 9
            };

            byte[] payload = packet.ToPayloadArray();
            NpcUpdate parsed = Assert.IsType<NpcUpdate>(TerrariaPacket.DeserializePayload(PacketTypes.NpcUpdate, payload));

            Assert.True((parsed.Flags & NpcUpdateFlags.HasAI1) != 0);
            Assert.False((parsed.Flags & NpcUpdateFlags.HasAI0) != 0);
            Assert.Equal((byte)4, parsed.LifeBytes);
            Assert.Equal(40000, parsed.Life);
            Assert.True(parsed.HasReleaseOwner);
            Assert.Equal((byte)9, parsed.ReleaseOwner);
        }

        [Fact]
        public void ProjectileNew_UsesPackedInt32KeyAndConditionalFields()
        {
            ProjectileNew packet = new ProjectileNew
            {
                Key = new ProjectileKey(3, 42, 7),
                PositionX = 1f,
                PositionY = 2f,
                VelocityX = 3f,
                VelocityY = 4f,
                Type = 5,
                Flags = ProjectileNewFlags.HasAI0 | ProjectileNewFlags.HasBannerIdToRespondTo |
                        ProjectileNewFlags.HasDamage | ProjectileNewFlags.HasExtraFlags,
                ExtraFlags = ProjectileNewExtraFlags.HasAI2,
                AI0 = 6f,
                BannerIdToRespondTo = 700,
                Damage = 800,
                AI2 = 9f
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(packet.Key.ToInt32(), BitConverter.ToInt32(payload, 0));
            Assert.Equal((byte)packet.Flags, payload[22]);
            Assert.Equal((byte)packet.ExtraFlags, payload[23]);
            Assert.Equal(payload, Assert.IsType<ProjectileNew>(
                TerrariaPacket.DeserializePayload(PacketTypes.ProjectileNew, payload)).ToPayloadArray());
        }

        [Fact]
        public void TileSendSquare_PreservesRawTilePayloadAndInvalidatesArrayCache()
        {
            TileSendSquare packet = new TileSendSquare
            {
                TileX = 10,
                TileY = 20,
                Width = 1,
                Height = 1,
                TileChangeType = 2,
                TilePayload = new byte[] { 1, 2, 3 }
            };

            Assert.Equal(new byte[] { 1, 2, 3 }, packet.TilePayload);
            packet.TilePayloadMemory = new byte[] { 4, 5, 6, 7 };
            Assert.Equal(new byte[] { 4, 5, 6, 7 }, packet.TilePayload);

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(new byte[] { 4, 5, 6, 7 }, payload[7..]);
            Assert.Equal(11, payload.Length);
        }

        private static byte[] BuildPayload(Action<BinaryWriter> write)
        {
            using MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                write(writer);
            }
            return stream.ToArray();
        }

        private static ColorStruct Color(byte r, byte g, byte b)
        {
            return new ColorStruct { R = r, G = g, B = b };
        }
    }
}
