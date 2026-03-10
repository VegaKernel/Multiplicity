using System;
using System.IO;
using Multiplicity.Packets;
using Multiplicity.Packets.Models;
using Multiplicity.Packets.Views;
using Xunit;
using TileModel = Multiplicity.Packets.Models.Tile;

namespace Multiplicity.Packets.Tests
{
    public class PacketParsingTests
    {
        [Fact]
        public void DeserializeReadOnlyMemory_PlayerInfo_RoundTripsKeyFields()
        {
            PlayerInfo original = new PlayerInfo
            {
                PlayerId = 7,
                SkinVariant = 3,
                VoiceVariant = 2,
                VoicePitchOffset = 0.15f,
                Hair = 11,
                Name = "Tester",
                HairDye = 5,
                HideVisibleAccessory = 0x1234,
                HideMisc = 0x56,
                HairColor = CreateColor(1, 2, 3),
                SkinColor = CreateColor(4, 5, 6),
                EyeColor = CreateColor(7, 8, 9),
                ShirtColor = CreateColor(10, 11, 12),
                UnderShirtColor = CreateColor(13, 14, 15),
                PantsColor = CreateColor(16, 17, 18),
                ShoeColor = CreateColor(19, 20, 21),
                DifficultyFlags = PlayerDifficultyFlags.Creative | PlayerDifficultyFlags.ExtraAccessory,
                TorchAndCartFlags = PlayerTorchAndCartFlags.EnabledSuperCart,
                ConsumableUnlockFlags = PlayerConsumableUnlockFlags.UsedAegisCrystal | PlayerConsumableUnlockFlags.AteArtisanBread
            };

            byte[] bytes = original.ToArray(includeHeader: true);

            TerrariaPacket parsedPacket = TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes);
            PlayerInfo parsed = Assert.IsType<PlayerInfo>(parsedPacket);

            Assert.Equal(original.PlayerId, parsed.PlayerId);
            Assert.Equal(original.Name, parsed.Name);
            Assert.Equal(original.HideVisibleAccessory, parsed.HideVisibleAccessory);
            Assert.Equal(original.DifficultyFlags, parsed.DifficultyFlags);
            Assert.Equal(original.ConsumableUnlockFlags, parsed.ConsumableUnlockFlags);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void PacketViewParser_TryParse_ConsumesFirstPacket_AndParsesPlayerUpdateView()
        {
            PlayerUpdate original = new PlayerUpdate
            {
                PlayerId = 9,
                ControlFlags = UpdatePlayerControlFlags.ControlJump | UpdatePlayerControlFlags.DirectionRight,
                MovementFlags = UpdatePlayerMovementFlags.HasVelocity | UpdatePlayerMovementFlags.HasMount,
                MiscFlags1 = UpdatePlayerMiscFlags1.HasPotionOfReturnPositions,
                MiscFlags2 = UpdatePlayerMiscFlags2.HasCameraTarget,
                SelectedItem = 4,
                PositionX = 100.5f,
                PositionY = 200.25f,
                VelocityX = 1.5f,
                VelocityY = -2.5f,
                MountType = 77,
                PotionOfReturnOriginalPositionX = 10f,
                PotionOfReturnOriginalPositionY = 20f,
                PotionOfReturnHomePositionX = 30f,
                PotionOfReturnHomePositionY = 40f,
                CameraTargetX = 55f,
                CameraTargetY = 65f
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            byte[] combinedBuffer = new byte[packetBytes.Length + 3];
            Buffer.BlockCopy(packetBytes, 0, combinedBuffer, 0, packetBytes.Length);

            bool result = PacketViewParser.TryParse(combinedBuffer.AsSpan(), out PacketView packetView, out int consumed);

            Assert.True(result);
            Assert.Equal(packetBytes.Length, consumed);

            PlayerUpdateView view = packetView.AsPlayerUpdateView();
            Assert.Equal(original.PlayerId, view.PlayerId);
            Assert.Equal(original.PositionX, view.PositionX);
            Assert.Equal(original.PositionY, view.PositionY);
            Assert.Equal(original.VelocityX, view.VelocityX);
            Assert.Equal(original.VelocityY, view.VelocityY);
            Assert.Equal(original.MountType, view.MountType);
            Assert.True(view.HasVelocity);
            Assert.True(view.HasMount);
            Assert.True(view.HasPotionOfReturnPositions);
            Assert.True(view.HasCameraTarget);
        }

        [Fact]
        public void PacketViewParser_ParseExact_HostToken_UsesStringView()
        {
            HostToken original = new HostToken
            {
                Token = "token-123"
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            PacketView packetView = PacketViewParser.ParseExact(bytes);
            StringPacketView view = packetView.AsStringPacketView();

            Assert.Equal(PacketTypes.HostToken, view.PacketType);
            Assert.Equal("token-123", view.GetValue());
        }

        [Fact]
        public void ModernTypedView_RequestSection_ParsesExpectedFields()
        {
            RequestSection original = new RequestSection
            {
                SectionX = 12,
                SectionY = 34
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            RequestSectionView view = packetView.AsRequestSectionView();

            Assert.Equal((ushort)12, view.SectionX);
            Assert.Equal((ushort)34, view.SectionY);
        }

        [Fact]
        public void WeaponsRackTryPlacingView_ParsesWeaponsRackTryPlacing()
        {
            WeaponsRackTryPlacing original = new WeaponsRackTryPlacing
            {
                TileX = 100,
                TileY = 101,
                ItemType = 2000,
                Prefix = 3,
                Stack = 2
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            WeaponsRackTryPlacingView view = packetView.AsWeaponsRackTryPlacingView();

            Assert.Equal(PacketTypes.WeaponsRackTryPlacing, view.Header.PacketType);
            Assert.Equal((short)100, view.TileX);
            Assert.Equal((short)101, view.TileY);
            Assert.Equal((short)2000, view.ItemType);
            Assert.Equal((byte)3, view.Prefix);
            Assert.Equal((short)2, view.Stack);
        }

        [Fact]
        public void PingView_ParsesPing()
        {
            Ping original = new Ping();

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            PingView view = packetView.AsPingView();

            Assert.Equal(PacketTypes.Ping, view.Header.PacketType);
        }

        [Fact]
        public void PacketViewParser_TryParse_ReturnsFalse_ForIncompleteBuffer()
        {
            ChatMessage original = new ChatMessage
            {
                PlayerId = 1,
                MessageColor = CreateColor(255, 100, 50),
                Message = "hello"
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            ReadOnlySpan<byte> incomplete = bytes.AsSpan(0, bytes.Length - 1);

            bool result = PacketViewParser.TryParse(incomplete, out PacketView _, out int consumed);

            Assert.False(result);
            Assert.Equal(0, consumed);
        }

        [Fact]
        public void DeserializePayload_PlayerInfo_ParsesWithoutPacketHeader()
        {
            PlayerInfo original = new PlayerInfo
            {
                PlayerId = 2,
                Name = "PayloadOnly",
                Hair = 7,
                DifficultyFlags = PlayerDifficultyFlags.Creative
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            ReadOnlyMemory<byte> payload = packetBytes.AsMemory(TerrariaPacket.PacketHeaderLength);

            TerrariaPacket parsedPacket = TerrariaPacket.DeserializePayload(PacketTypes.PlayerInfo, payload);
            PlayerInfo parsed = Assert.IsType<PlayerInfo>(parsedPacket);

            Assert.Equal(original.PlayerId, parsed.PlayerId);
            Assert.Equal(original.Name, parsed.Name);
            Assert.Equal(original.Hair, parsed.Hair);
            Assert.Equal(original.DifficultyFlags, parsed.DifficultyFlags);
        }

        [Fact]
        public void PacketViewParser_ParsePayload_UpdatePlayer_ParsesTypedViewWithoutPacketHeader()
        {
            PlayerUpdate original = new PlayerUpdate
            {
                PlayerId = 4,
                ControlFlags = UpdatePlayerControlFlags.ControlUseItem,
                MovementFlags = UpdatePlayerMovementFlags.HasVelocity,
                SelectedItem = 8,
                PositionX = 321.5f,
                PositionY = 654.25f,
                VelocityX = -1.25f,
                VelocityY = 2.75f
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            PacketView packetView = PacketViewParser.ParsePayload(
                PacketTypes.PlayerUpdate,
                packetBytes,
                TerrariaPacket.PacketHeaderLength,
                packetBytes.Length - TerrariaPacket.PacketHeaderLength);

            PlayerUpdateView view = packetView.AsPlayerUpdateView();

            Assert.False(packetView.HasPacketSpan);
            Assert.Equal(PacketTypes.PlayerUpdate, view.Header.PacketType);
            Assert.Equal(original.PlayerId, view.PlayerId);
            Assert.Equal(original.PositionX, view.PositionX);
            Assert.Equal(original.PositionY, view.PositionY);
            Assert.Equal(original.VelocityX, view.VelocityX);
            Assert.Equal(original.VelocityY, view.VelocityY);
            Assert.Equal(packetBytes.Length - TerrariaPacket.PacketHeaderLength, view.SourceSpan.Length);

            bool threw = false;
            try
            {
                packetView.GetPacketSpan();
            }
            catch (InvalidDataException)
            {
                threw = true;
            }

            Assert.True(threw);
        }

        [Fact]
        public void PaintTileView_ParsesColorAndCoat()
        {
            PaintTile original = new PaintTile
            {
                X = 123,
                Y = 456,
                Color = 7,
                Coat = 2
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            PaintTileView view = packetView.AsPaintTileView();
            PaintTile parsed = Assert.IsType<PaintTile>(TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)original.ToArray(includeHeader: true)));

            Assert.Equal((short)123, view.X);
            Assert.Equal((short)456, view.Y);
            Assert.Equal((byte)7, view.Color);
            Assert.Equal((byte)2, view.Coat);
            Assert.Equal((byte)2, parsed.Coat);
            Assert.Equal(original.ToArray(includeHeader: true), parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void PaintWallView_ParsesColorAndCoat()
        {
            PaintWall original = new PaintWall
            {
                X = 321,
                Y = 654,
                Color = 9,
                Coat = 1
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            PaintWallView view = packetView.AsPaintWallView();
            PaintWall parsed = Assert.IsType<PaintWall>(TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)original.ToArray(includeHeader: true)));

            Assert.Equal((short)321, view.X);
            Assert.Equal((short)654, view.Y);
            Assert.Equal((byte)9, view.Color);
            Assert.Equal((byte)1, view.Coat);
            Assert.Equal((byte)1, parsed.Coat);
            Assert.Equal(original.ToArray(includeHeader: true), parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void PacketViewParser_TryParsePayload_ReturnsFalse_ForTooLargePayload()
        {
            byte[] payload = new byte[ushort.MaxValue];

            bool result = PacketViewParser.TryParsePayload(PacketTypes.Ping, payload, 0, payload.Length, out PacketView _);

            Assert.False(result);
        }

        [Fact]
        public void ProjectileNew_PayloadSerialization_RoundTripsWithoutHeader()
        {
            ProjectileNew original = new ProjectileNew
            {
                Identity = 42,
                PositionX = 14.5f,
                PositionY = 28.25f,
                VelocityX = 1.75f,
                VelocityY = -0.5f,
                Owner = 3,
                Type = 97,
                AI0 = 2.25f,
                Damage = 55
            };

            byte[] payload = original.ToPayloadArray();
            Span<byte> copiedPayload = stackalloc byte[payload.Length];

            original.CopyPayloadTo(copiedPayload);

            Assert.Equal(payload, copiedPayload.ToArray());

            ProjectileNew parsed = Assert.IsType<ProjectileNew>(
                TerrariaPacket.DeserializePayload(PacketTypes.ProjectileNew, payload));
            ProjectileNewView view = ProjectileNewView.FromPayload(payload);

            Assert.Equal(original.Identity, parsed.Identity);
            Assert.Equal(original.Identity, view.Identity);
            Assert.Equal(original.Type, view.Type);
            Assert.Equal(original.Damage, view.Damage);
        }

        [Fact]
        public void PayloadMetadata_ItemDrop_ReportsFixedPayloadLength()
        {
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.ItemDrop);

            Assert.Equal(PacketTypes.ItemDrop, metadata.PacketType);
            Assert.Equal(typeof(ItemDrop), metadata.PacketClrType);
            Assert.Equal((short)24, metadata.DefaultPayloadLength);
            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal((short)24, metadata.FixedPayloadLength);
            Assert.True(TerrariaPacket.TryGetFixedPayloadLength(PacketTypes.ItemDrop, out short payloadLength));
            Assert.Equal((short)24, payloadLength);
        }

        [Fact]
        public void PayloadMetadata_HostToken_ReportsDefaultLengthButNoFixedLength()
        {
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.HostToken);

            Assert.Equal(PacketTypes.HostToken, metadata.PacketType);
            Assert.Equal(typeof(HostToken), metadata.PacketClrType);
            Assert.Equal(new HostToken().GetLength(), metadata.DefaultPayloadLength);
            Assert.False(metadata.HasFixedPayloadLength);
            Assert.False(TerrariaPacket.TryGetFixedPayloadLength(PacketTypes.HostToken, out _));
        }

        [Fact]
        public void PacketViewParser_TryParsePayload_WithConsumed_TrimsFixedPayloadFromTrailingBytes()
        {
            ItemDrop original = new ItemDrop
            {
                ItemIndex = 12,
                PositionX = 14.5f,
                PositionY = 28.25f,
                VelocityX = 1.5f,
                VelocityY = -2.5f,
                Stack = 3,
                Prefix = 4,
                Flags = WorldItemSyncFlags.KeepTime,
                ItemNetId = 71
            };

            byte[] payload = original.ToPayloadArray();
            byte[] extendedPayload = new byte[payload.Length + 1];
            Buffer.BlockCopy(payload, 0, extendedPayload, 0, payload.Length);
            extendedPayload[^1] = 0x7F;

            bool result = PacketViewParser.TryParsePayload(
                PacketTypes.ItemDrop,
                extendedPayload,
                0,
                out PacketView packetView,
                out int consumed);

            Assert.True(result);
            Assert.Equal(payload.Length, consumed);
            Assert.Equal(payload.Length, packetView.PayloadSpan.Length);

            ItemDropView view = packetView.AsItemDropView();
            Assert.Equal(original.ItemIndex, view.ItemIndex);
            Assert.Equal(original.Stack, view.Stack);
            Assert.Equal(original.ItemNetId, view.ItemNetId);
        }

        [Fact]
        public void PacketViewParser_TryParsePayload_WithConsumed_UsesVariablePacketParser()
        {
            HostToken original = new HostToken
            {
                Token = "payload-token"
            };

            byte[] payload = original.ToPayloadArray();
            byte[] extendedPayload = new byte[payload.Length + 3];
            Buffer.BlockCopy(payload, 0, extendedPayload, 0, payload.Length);
            extendedPayload[^3] = 1;
            extendedPayload[^2] = 2;
            extendedPayload[^1] = 3;

            bool result = PacketViewParser.TryParsePayload(
                PacketTypes.HostToken,
                extendedPayload,
                0,
                out PacketView packetView,
                out int consumed);

            Assert.True(result);
            Assert.Equal(payload.Length, consumed);
            Assert.Equal("payload-token", packetView.AsStringPacketView().GetValue());
        }

        [Fact]
        public void PacketViewParser_TryParsePayloadBounded_AllowsTrailingBytes_ForFixedPayload()
        {
            ItemDrop original = new ItemDrop
            {
                ItemIndex = 21,
                PositionX = 3.5f,
                PositionY = 8.25f,
                VelocityX = 1.25f,
                VelocityY = -4.5f,
                Stack = 6,
                Prefix = 2,
                Flags = WorldItemSyncFlags.KeepTime,
                ItemNetId = 73
            };

            byte[] payload = original.ToPayloadArray();
            byte[] extendedPayload = new byte[payload.Length + 2];
            Buffer.BlockCopy(payload, 0, extendedPayload, 0, payload.Length);
            extendedPayload[^2] = 0x44;
            extendedPayload[^1] = 0x55;

            bool exactResult = PacketViewParser.TryParsePayload(PacketTypes.ItemDrop, extendedPayload, 0, extendedPayload.Length, out PacketView _);
            bool boundedResult = PacketViewParser.TryParsePayloadBounded(
                PacketTypes.ItemDrop,
                extendedPayload,
                0,
                extendedPayload.Length,
                out PacketView packetView,
                out int consumed);

            Assert.False(exactResult);
            Assert.True(boundedResult);
            Assert.Equal(payload.Length, consumed);
            Assert.Equal(original.ItemIndex, packetView.AsItemDropView().ItemIndex);
        }

        [Fact]
        public void PacketViewParser_TryParsePayloadBounded_AllowsTrailingBytes_ForVariablePayload()
        {
            HostToken original = new HostToken
            {
                Token = "bounded-payload"
            };

            byte[] payload = original.ToPayloadArray();
            byte[] extendedPayload = new byte[payload.Length + 2];
            Buffer.BlockCopy(payload, 0, extendedPayload, 0, payload.Length);
            extendedPayload[^2] = 0x21;
            extendedPayload[^1] = 0x22;

            bool exactResult = PacketViewParser.TryParsePayload(PacketTypes.HostToken, extendedPayload, 0, extendedPayload.Length, out PacketView _);
            bool boundedResult = PacketViewParser.TryParsePayloadBounded(
                PacketTypes.HostToken,
                extendedPayload,
                0,
                extendedPayload.Length,
                out PacketView packetView,
                out int consumed);

            Assert.False(exactResult);
            Assert.True(boundedResult);
            Assert.Equal(payload.Length, consumed);
            Assert.Equal(original.Token, packetView.AsStringPacketView().GetValue());
        }

        [Fact]
        public void PacketViewParser_TryParsePayloadBounded_ReturnsFalse_WhenConsumedExceedsAvailableLength()
        {
            ItemDrop original = new ItemDrop
            {
                ItemIndex = 5,
                PositionX = 1f,
                PositionY = 2f,
                VelocityX = 3f,
                VelocityY = 4f,
                Stack = 1,
                Prefix = 0,
                Flags = WorldItemSyncFlags.None,
                ItemNetId = 9
            };

            byte[] payload = original.ToPayloadArray();

            bool result = PacketViewParser.TryParsePayloadBounded(
                PacketTypes.ItemDrop,
                payload,
                0,
                payload.Length - 1,
                out PacketView _,
                out int consumed);

            Assert.False(result);
            Assert.Equal(0, consumed);
        }

        [Fact]
        public void StringPacketView_FromPayload_ParsesWithoutPacketHeader()
        {
            HostToken original = new HostToken
            {
                Token = "payload-token"
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            ReadOnlySpan<byte> payload = packetBytes.AsSpan(TerrariaPacket.PacketHeaderLength);

            StringPacketView view = StringPacketView.FromPayload(PacketTypes.HostToken, payload);

            Assert.Equal(PacketTypes.HostToken, view.PacketType);
            Assert.Equal("payload-token", view.GetValue());
            Assert.Equal(payload.Length, view.SourceSpan.Length);
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_NetPingModule_RoundTrips()
        {
            NetPingModule original = new NetPingModule
            {
                PositionX = 12.5f,
                PositionY = 64.25f
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            NetPingModule parsed = Assert.IsType<NetPingModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.Equal(original.PositionX, parsed.PositionX);
            Assert.Equal(original.PositionY, parsed.PositionY);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void LoadNetModule_Deserializes_TeleportPylonModule()
        {
            LoadNetModule packet = new LoadNetModule
            {
                LoadedModule = new NetTeleportPylonModule
                {
                    MessageType = NetTeleportPylonMessageType.PlayerRequestsTeleport,
                    TileX = 400,
                    TileY = 401,
                    PylonType = 7
                }
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            LoadNetModule parsed = Assert.IsType<LoadNetModule>(TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            NetTeleportPylonModule module = Assert.IsType<NetTeleportPylonModule>(parsed.LoadedModule);

            Assert.Equal(NetTeleportPylonMessageType.PlayerRequestsTeleport, module.MessageType);
            Assert.Equal((short)400, module.TileX);
            Assert.Equal((short)401, module.TileY);
            Assert.Equal((byte)7, module.PylonType);
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_CraftingRequest_RoundTripsRequestPayload()
        {
            NetCraftingRequestsModule original = new NetCraftingRequestsModule();
            original.Items.Add(new CraftingRequestItem { ItemIdOrRecipeGroup = 12, Stack = 3 });
            original.Items.Add(new CraftingRequestItem { ItemIdOrRecipeGroup = 6001, Stack = 99 });
            original.ChestIndices.Add(4);
            original.ChestIndices.Add(17);

            byte[] bytes = original.ToArray(includeHeader: true);
            NetCraftingRequestsModule parsed = Assert.IsType<NetCraftingRequestsModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.Equal(NetCraftingRequestsPayloadKind.Request, parsed.PayloadKind);
            Assert.Equal(2, parsed.Items.Count);
            Assert.Equal(6001, parsed.Items[1].ItemIdOrRecipeGroup);
            Assert.Equal(99, parsed.Items[1].Stack);
            Assert.Equal(new[] { 4, 17 }, parsed.ChestIndices);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_TagEffectFullState_PreservesSparseArrays()
        {
            TagEffectModule original = new TagEffectModule
            {
                PlayerId = 5,
                MessageType = TagEffectMessageType.FullState,
                EffectType = 14,
                IncludeProcTimeEntries = true
            };
            original.TimeEntries.Add(new TagEffectNpcTimeEntry { NpcIndex = 2, TimeLeft = 120 });
            original.TimeEntries.Add(new TagEffectNpcTimeEntry { NpcIndex = 9, TimeLeft = 45 });
            original.ProcTimeEntries.Add(new TagEffectNpcTimeEntry { NpcIndex = 7, TimeLeft = 18 });

            byte[] bytes = original.ToArray(includeHeader: true);
            TagEffectModule parsed = Assert.IsType<TagEffectModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.Equal((byte)5, parsed.PlayerId);
            Assert.Equal(TagEffectMessageType.FullState, parsed.MessageType);
            Assert.Equal((short)14, parsed.EffectType);
            Assert.True(parsed.IncludeProcTimeEntries);
            Assert.Equal(2, parsed.TimeEntries.Count);
            Assert.Equal((byte)9, parsed.TimeEntries[1].NpcIndex);
            Assert.Equal(45, parsed.TimeEntries[1].TimeLeft);
            Assert.Single(parsed.ProcTimeEntries);
            Assert.Equal((byte)7, parsed.ProcTimeEntries[0].NpcIndex);
            Assert.Equal(18, parsed.ProcTimeEntries[0].TimeLeft);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_LeashedEntityFullSync_PreservesEnvelopeAndPayload()
        {
            LeashedEntityModule original = new LeashedEntityModule
            {
                MessageType = LeashedEntityMessageType.FullSync,
                Slot = 321,
                EntityType = 200,
                AnchorX = 1200,
                AnchorY = 1300,
                EntityPayload = new byte[] { 1, 2, 3, 4, 5 }
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            LeashedEntityModule parsed = Assert.IsType<LeashedEntityModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.Equal(LeashedEntityMessageType.FullSync, parsed.MessageType);
            Assert.Equal(321, parsed.Slot);
            Assert.Equal(200, parsed.EntityType);
            Assert.Equal((short)1200, parsed.AnchorX);
            Assert.Equal((short)1300, parsed.AnchorY);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, parsed.EntityPayload);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void LoadNetModuleView_AsNetPingModuleView_ParsesModulePayload()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new NetPingModule
                {
                    PositionX = 44.5f,
                    PositionY = 88.25f
                }
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            LoadNetModuleView moduleView = packetView.AsLoadNetModuleView();
            NetPingModuleView pingView = moduleView.AsNetPingModuleView();

            Assert.Equal(NetworkModuleTypes.NetPingModule, moduleView.ModuleType);
            Assert.Equal(44.5f, pingView.PositionX);
            Assert.Equal(88.25f, pingView.PositionY);
        }

        [Fact]
        public void LoadNetModuleView_AsNetTextModuleView_ParsesClientMessage()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new NetTextModule
                {
                    PayloadKind = NetTextModulePayloadKind.ClientChatMessage,
                    CommandName = "say",
                    ChatMessage = "hello"
                }
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            NetTextModuleView textView = packetView.AsLoadNetModuleView().AsNetTextModuleView();

            Assert.Equal(NetTextModulePayloadKind.ClientChatMessage, textView.PayloadKind);
            Assert.Equal("say", textView.CommandName);
            Assert.Equal("hello", textView.ChatMessage);
        }

        [Fact]
        public void LoadNetModuleView_AsNetCraftingRequestsModuleView_ParsesRequestCollections()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new NetCraftingRequestsModule()
            };

            NetCraftingRequestsModule craftingModule = (NetCraftingRequestsModule)original.LoadedModule;
            craftingModule.Items.Add(new CraftingRequestItem { ItemIdOrRecipeGroup = 123, Stack = 2 });
            craftingModule.Items.Add(new CraftingRequestItem { ItemIdOrRecipeGroup = 456, Stack = 8 });
            craftingModule.ChestIndices.Add(7);
            craftingModule.ChestIndices.Add(9);

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            NetCraftingRequestsModuleView craftingView = packetView.AsLoadNetModuleView().AsNetCraftingRequestsModuleView();

            Assert.Equal(NetCraftingRequestsPayloadKind.Request, craftingView.PayloadKind);
            Assert.Equal(2, craftingView.Items.Count);
            Assert.Equal(456, craftingView.Items[1].ItemIdOrRecipeGroup);
            Assert.Equal(8, craftingView.Items[1].Stack);
            Assert.Equal(new[] { 7, 9 }, craftingView.ChestIndices);
        }

        [Fact]
        public void LoadNetModuleView_AsLeashedEntityModuleView_ParsesEnvelopeFields()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new LeashedEntityModule
                {
                    MessageType = LeashedEntityMessageType.FullSync,
                    Slot = 15,
                    EntityType = 4,
                    AnchorX = 320,
                    AnchorY = 640,
                    EntityPayload = new byte[] { 9, 8, 7 }
                }
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            LeashedEntityModuleView leashedView = packetView.AsLoadNetModuleView().AsLeashedEntityModuleView();

            Assert.Equal(LeashedEntityMessageType.FullSync, leashedView.MessageType);
            Assert.Equal(15, leashedView.Slot);
            Assert.Equal(4, leashedView.EntityType);
            Assert.Equal((short)320, leashedView.AnchorX);
            Assert.Equal((short)640, leashedView.AnchorY);
            Assert.Equal(new byte[] { 9, 8, 7 }, leashedView.EntityPayloadSpan.ToArray());
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_NetBannersFullState_RoundTripsTypedState()
        {
            NetBannersModule original = new NetBannersModule
            {
                MessageType = NetBannersMessageType.FullState,
                FullStateData = new BannerSystemState
                {
                    KillCounts = new[] { 4, 8, 15, 16, 23, 42 },
                    ClaimableBanners = new ushort[] { 2, 5, 9 }
                }
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            NetBannersModule parsed = Assert.IsType<NetBannersModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.Equal(NetBannersMessageType.FullState, parsed.MessageType);
            Assert.Equal(new[] { 4, 8, 15, 16, 23, 42 }, parsed.FullStateData.KillCounts);
            Assert.Equal(new ushort[] { 2, 5, 9 }, parsed.FullStateData.ClaimableBanners);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void LoadNetModuleView_AsNetBannersModuleView_ParsesFullState()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new NetBannersModule
                {
                    MessageType = NetBannersMessageType.FullState,
                    FullStateData = new BannerSystemState
                    {
                        KillCounts = new[] { 1, 3, 5 },
                        ClaimableBanners = new ushort[] { 7, 11 }
                    }
                }
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            BannerSystemState state = packetView.AsLoadNetModuleView().AsNetBannersModuleView().GetFullState();

            Assert.Equal(new[] { 1, 3, 5 }, state.KillCounts);
            Assert.Equal(new ushort[] { 7, 11 }, state.ClaimableBanners);
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_NetCreativePowers_PerPlayerToggle_RoundTripsTypedPayload()
        {
            NetCreativePowersModule original = new NetCreativePowersModule
            {
                PowerType = CreativePowerId.GodmodePower,
                PowerData = new CreativePowerPerPlayerTogglePayload
                {
                    MessageType = CreativePowerPerPlayerToggleMessageType.SyncEveryone,
                    PlayerStateBytes = new byte[] { 0b_0000_0101, 0b_1000_0000 }
                }
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            NetCreativePowersModule parsed = Assert.IsType<NetCreativePowersModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));
            CreativePowerPerPlayerTogglePayload payload = Assert.IsType<CreativePowerPerPlayerTogglePayload>(parsed.PowerData);

            Assert.Equal(CreativePowerId.GodmodePower, parsed.PowerType);
            Assert.Equal(CreativePowerPerPlayerToggleMessageType.SyncEveryone, payload.MessageType);
            Assert.True(payload.GetEnabledState(0));
            Assert.False(payload.GetEnabledState(1));
            Assert.True(payload.GetEnabledState(2));
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void LoadNetModuleView_AsNetCreativePowersModuleView_ParsesSharedSliderPayload()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new NetCreativePowersModule
                {
                    PowerType = CreativePowerId.ModifyTimeRate,
                    PowerData = new CreativePowerSharedSliderPayload
                    {
                        Value = 0.75f
                    }
                }
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            NetCreativePowersModuleView view = packetView.AsLoadNetModuleView().AsNetCreativePowersModuleView();
            CreativePowerSharedSliderPayload payload = Assert.IsType<CreativePowerSharedSliderPayload>(view.GetPayload());

            Assert.Equal(CreativePowerId.ModifyTimeRate, view.PowerType);
            Assert.Equal(CreativePowerPayloadKind.SharedSlider, view.PayloadKind);
            Assert.Equal(0.75f, payload.Value);
        }

        [Fact]
        public void TerrariaNetModule_Deserialize_LeashedEntityFullSync_RoundTripsTypedKitePayload()
        {
            LeashedEntityModule original = new LeashedEntityModule
            {
                MessageType = LeashedEntityMessageType.FullSync,
                Slot = 12,
                EntityKind = LeashedEntityType.Kite,
                AnchorX = 1440,
                AnchorY = 1660,
                EntityData = new LeashedKitePayload
                {
                    ProjectileType = 88,
                    Position = new System.Numerics.Vector2(10.5f, 20.25f),
                    Velocity = new System.Numerics.Vector2(1.5f, -0.75f),
                    RotationByte = 64,
                    WindTarget = 0.8f,
                    CloudAlpha = 0.3f,
                    TimeCounter = 5.5f
                }
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            LeashedEntityModule parsed = Assert.IsType<LeashedEntityModule>(TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)bytes));
            LeashedKitePayload payload = Assert.IsType<LeashedKitePayload>(parsed.EntityData);

            Assert.Equal(LeashedEntityType.Kite, parsed.EntityKind);
            Assert.Equal(88, payload.ProjectileType);
            Assert.Equal(10.5f, payload.Position.X);
            Assert.Equal(20.25f, payload.Position.Y);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void LoadNetModuleView_AsLeashedEntityModuleView_ParsesNormalButterflyPayload()
        {
            LoadNetModule original = new LoadNetModule
            {
                LoadedModule = new LeashedEntityModule
                {
                    MessageType = LeashedEntityMessageType.FullSync,
                    Slot = 27,
                    EntityKind = LeashedEntityType.NormalButterfly,
                    AnchorX = 320,
                    AnchorY = 512,
                    EntityData = new NormalButterflyLeashedCritterPayload
                    {
                        NpcType = 222,
                        Size = new System.Numerics.Vector2(16f, 18f),
                        PositionOffset = new System.Numerics.Vector2(2.25f, -1.5f),
                        FacingRight = true,
                        RandomState = 123456789,
                        WaitTime = 44,
                        State = 3,
                        TargetOffsetX = -2,
                        TargetOffsetY = 5,
                        Variant = 6
                    }
                }
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            LeashedEntityModuleView view = packetView.AsLoadNetModuleView().AsLeashedEntityModuleView();
            NormalButterflyLeashedCritterPayload payload = Assert.IsType<NormalButterflyLeashedCritterPayload>(view.GetPayload());

            Assert.Equal(LeashedEntityType.NormalButterfly, view.EntityKind);
            Assert.Equal(222, payload.NpcType);
            Assert.True(payload.FacingRight);
            Assert.Equal((byte)6, payload.Variant);
        }

        [Fact]
        public void TilePayloadReader_Read_PacketReader_ParsesFrameImportantTile()
        {
            using MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                writer.Write((byte)29);
                writer.Write((byte)159);
                writer.Write((byte)5);
                writer.Write((byte)7);
                writer.Write((byte)8);
                writer.Write((ushort)21);
                writer.Write((short)10);
                writer.Write((short)20);
                writer.Write((ushort)55);
                writer.Write((byte)6);
                writer.Write((byte)2);
            }

            PacketReader reader = new PacketReader(stream.ToArray());
            TileModel tile = TilePayloadReader.Read(ref reader, static tileType => tileType == 21);

            Assert.True(tile.IsActive);
            Assert.True(tile.HasWall);
            Assert.True(tile.HasLiquid);
            Assert.True(tile.WireRed);
            Assert.True(tile.WireGreen);
            Assert.True(tile.WireBlue);
            Assert.True(tile.WireYellow);
            Assert.True(tile.HasTileColor);
            Assert.True(tile.HasWallColor);
            Assert.Equal((ushort)21, tile.Type);
            Assert.Equal((short)10, tile.FrameX);
            Assert.Equal((short)20, tile.FrameY);
            Assert.Equal((ushort)55, tile.Wall);
            Assert.Equal((byte)7, tile.TileColor);
            Assert.Equal((byte)8, tile.WallColor);
            Assert.Equal((byte)1, tile.Slope);
            Assert.Equal((byte)6, tile.LiquidAmount);
            Assert.Equal((byte)2, tile.LiquidType);
            reader.EnsureEnd();
        }

        [Fact]
        public void TileSendSquareView_CreateTilePayloadReader_AllowsReadingNestedTilePayload()
        {
            byte[] tilePayload;
            using (MemoryStream stream = new MemoryStream())
            {
                using BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
                writer.Write((byte)1);
                writer.Write((byte)96);
                writer.Write((byte)0);
                writer.Write((ushort)53);
                tilePayload = stream.ToArray();
            }

            TileSendSquare packet = new TileSendSquare
            {
                TileX = 100,
                TileY = 200,
                Width = 1,
                Height = 1,
                TileChangeType = 0,
                TilePayload = tilePayload
            };

            TileSendSquareView view = PacketViewParser.ParseExact(packet.ToArray(includeHeader: true)).AsTileSendSquareView();
            PacketReader reader = view.CreateTilePayloadReader();
            TileModel tile = TilePayloadReader.Read(ref reader, static _ => false);

            Assert.Equal((short)100, view.TileX);
            Assert.Equal((short)200, view.TileY);
            Assert.Equal((byte)1, view.Width);
            Assert.Equal((byte)1, view.Height);
            Assert.Equal((ushort)53, tile.Type);
            Assert.Equal((short)-1, tile.FrameX);
            Assert.Equal((short)-1, tile.FrameY);
            Assert.Equal((byte)6, tile.Slope);
            reader.EnsureEnd();
        }

        [Fact]
        public void TileSendSquareView_ReadTiles_ParsesRectangle()
        {
            byte[] tilePayload;
            using (MemoryStream stream = new MemoryStream())
            {
                using BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);

                writer.Write((byte)1);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((ushort)1);

                writer.Write((byte)5);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((ushort)2);
                writer.Write((ushort)9);

                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);

                writer.Write((byte)1);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((ushort)7);

                tilePayload = stream.ToArray();
            }

            TileSendSquare packet = new TileSendSquare
            {
                TileX = 50,
                TileY = 75,
                Width = 2,
                Height = 2,
                TileChangeType = 0,
                TilePayload = tilePayload
            };

            TileSendSquareView view = PacketViewParser.ParseExact(packet.ToArray(includeHeader: true)).AsTileSendSquareView();
            TileModel[,] tiles = view.ReadTiles(static _ => false);

            Assert.Equal((ushort)1, tiles[0, 0].Type);
            Assert.Equal((ushort)2, tiles[0, 1].Type);
            Assert.Equal((ushort)9, tiles[0, 1].Wall);
            Assert.False(tiles[1, 0].IsActive);
            Assert.Equal((ushort)7, tiles[1, 1].Type);
        }

        private static ColorStruct CreateColor(byte r, byte g, byte b)
        {
            return new ColorStruct
            {
                R = r,
                G = g,
                B = b
            };
        }
    }
}



