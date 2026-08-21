using System;
using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol325Tests
    {
        [Fact]
        public void ConnectRequest_DefaultsToTerraria325()
        {
            ConnectRequest packet = new ConnectRequest();

            Assert.Equal(325, ConnectRequest.Protocol325);
            Assert.Equal(ConnectRequest.Protocol325, ConnectRequest.CurrentProtocol);
            Assert.Equal("Terraria325", packet.Version);
            Assert.True(packet.IsProtocol(325));

            byte[] bytes = packet.ToArray(includeHeader: true);
            ConnectRequest parsed = Assert.IsType<ConnectRequest>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            PacketView packetView = PacketViewParser.ParseExact(bytes);
            ConnectRequestView view = packetView.AsConnectRequestView();

            Assert.Equal("Terraria325", parsed.Version);
            Assert.Equal("Terraria325", view.Version);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void KillPortal_UsesThreeByteWireLayout()
        {
            KillPortal packet = new KillPortal
            {
                PlayerId = 0x1234,
                PortalColor = 7
            };

            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(new byte[]
            {
                6, 0,
                (byte)PacketTypes.KillPortal,
                0x34, 0x12,
                7
            }, bytes);

            KillPortal parsed = Assert.IsType<KillPortal>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            PacketView packetView = PacketViewParser.ParseExact(bytes);
            KillPortalView view = packetView.AsKillPortalView();

            Assert.Equal((ushort)0x1234, parsed.PlayerId);
            Assert.Equal((byte)7, parsed.PortalColor);
            Assert.Equal((ushort)0x1234, view.PlayerId);
            Assert.Equal((byte)7, view.PortalColor);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void KillPortal_MetadataReportsThreeBytePayload()
        {
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.KillPortal);

            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal((short)3, metadata.FixedPayloadLength);
            Assert.True(TerrariaPacket.TryGetFixedPayloadLength(PacketTypes.KillPortal, out short payloadLength));
            Assert.Equal((short)3, payloadLength);
        }

        [Fact]
        public void Protocol325PacketIdRange_MatchesDesktopMessageIdsAndExtensions()
        {
            Assert.Equal(145, (int)PacketTypes.SyncItemsWithShimmerDeprecated);
            Assert.Equal(148, (int)PacketTypes.SyncItemCannotBeTakenByEnemiesDeprecated);
            Assert.Equal(161, (int)PacketTypes.HostToken);
            Assert.Equal(162, (int)PacketTypes.DamageNPCAck);
            Assert.Equal(163, (int)PacketTypes.ServerInfo);
            Assert.Equal(164, (int)PacketTypes.PlayerPlatformInfo);
        }

        [Fact]
        public void Protocol325_DeprecatedVanillaPacketIdsAreNotRegisteredForDeserialization()
        {
            Assert.False(TerrariaPacket.DeserializerMap.ContainsKey(PacketTypes.SyncItemsWithShimmerDeprecated));
            Assert.False(TerrariaPacket.DeserializerMap.ContainsKey(PacketTypes.SyncItemCannotBeTakenByEnemiesDeprecated));
        }

        [Fact]
        public void Protocol325_NetModuleRegistrationOrderMatchesTerraria1457()
        {
            Assert.Equal(0, (int)NetworkModuleTypes.NetLiquidModule);
            Assert.Equal(4, (int)NetworkModuleTypes.NetBestiaryModule);
            Assert.Equal(5, (int)NetworkModuleTypes.NetCreativeUnlocksModule);
            Assert.Equal(6, (int)NetworkModuleTypes.NetCreativePowersModule);
            Assert.Equal(12, (int)NetworkModuleTypes.NetCraftingRequestsModule);
            Assert.Equal(13, (int)NetworkModuleTypes.LeashedEntityModule);
            Assert.Equal(14, (int)NetworkModuleTypes.UnbreakableWallScanModule);
        }

        [Fact]
        public void NpcUpdate_ReleaseOwnerTailDoesNotDependOnKnownCatchableTable()
        {
            const short unknownCatchableType = 700;
            Assert.DoesNotContain(unknownCatchableType, NpcUpdate.NpcCatchable);

            NpcUpdate packet = new NpcUpdate
            {
                NpcSlot = 17,
                Generation = 3,
                NpcNetId = unknownCatchableType,
                Life = 80,
                ReleaseOwner = 9,
                HasReleaseOwner = true
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            NpcUpdate parsed = Assert.IsType<NpcUpdate>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            NpcUpdateView view = PacketViewParser.ParseExact(bytes).AsNpcUpdateView();

            Assert.True(parsed.HasReleaseOwner);
            Assert.Equal((byte)9, parsed.ReleaseOwner);
            Assert.True(view.HasReleaseOwner);
            Assert.Equal((byte)9, view.ReleaseOwner);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void ItemOwner_UsesProtocol325ReservationAndGrabDelayLayout()
        {
            ItemOwner packet = new ItemOwner
            {
                ItemId = 321,
                PlayerId = 4,
                TimeToKeepReservation = 300,
                GrabDelayPlayer = 7,
                GrabDelayTime = 900,
                PositionX = 12.5f,
                PositionY = 33.25f
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            ItemOwner parsed = Assert.IsType<ItemOwner>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            ItemOwnerView view = PacketViewParser.ParseExact(bytes).AsItemOwnerView();

            Assert.Equal(300, parsed.TimeToKeepReservation);
            Assert.Equal((byte)7, parsed.GrabDelayPlayer);
            Assert.Equal(900, parsed.GrabDelayTime);
            Assert.Equal(12.5f, view.PositionX);
            Assert.Equal(33.25f, view.PositionY);
            Assert.Equal(300, view.TimeToKeepReservation);
            Assert.Equal(900, view.GrabDelayTime);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void RemoveItemOwner_IncludesForceAssignFlag()
        {
            RemoveItemOwner packet = new RemoveItemOwner
            {
                ItemIndex = 42,
                ForceAssignToServer = true
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            Assert.Equal(new byte[] { 6, 0, (byte)PacketTypes.RemoveItemOwner, 42, 0, 1 }, bytes);

            RemoveItemOwner parsed = Assert.IsType<RemoveItemOwner>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            RemoveItemOwnerView view = PacketViewParser.ParseExact(bytes).AsRemoveItemOwnerView();

            Assert.True(parsed.ForceAssignToServer);
            Assert.True(view.ForceAssignToServer);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void DamageNPCAck_RoundTripsAsEmptyPayloadPacket()
        {
            DamageNPCAck packet = new DamageNPCAck();
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(new byte[] { 3, 0, (byte)PacketTypes.DamageNPCAck }, bytes);
            Assert.IsType<DamageNPCAck>(TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
        }

        [Fact]
        public void CatchNPC_UsesProtocol325TwoBytePayload()
        {
            CatchNPC packet = new CatchNPC
            {
                NpcId = 0x1234
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            Assert.Equal(new byte[] { 5, 0, (byte)PacketTypes.CatchNPC, 0x34, 0x12 }, bytes);

            CatchNPC parsed = Assert.IsType<CatchNPC>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            CatchNPCView view = PacketViewParser.ParseExact(bytes).AsCatchNPCView();

            Assert.Equal((short)0x1234, parsed.NpcId);
            Assert.Equal((short)0x1234, view.NpcId);
            Assert.Equal((short)2, CatchNPC.PayloadLength);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void ItemSync_ExplicitZeroValuedOptionalBlocksRemainPresent()
        {
            ItemDrop packet = new ItemDrop
            {
                ItemIndex = 12,
                Stack = 1,
                ItemNetId = 71,
                Flags = WorldItemSyncFlags.HasShimmerData | WorldItemSyncFlags.HasEnemyGrabDelay,
                Shimmered = false,
                ShimmerTime = 0f,
                EnemyGrabDelayTime = 0
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(30, payload.Length);

            ItemDrop parsed = Assert.IsType<ItemDrop>(
                TerrariaPacket.DeserializePayload(PacketTypes.ItemDrop, payload));

            Assert.True((parsed.Flags & WorldItemSyncFlags.HasShimmerData) != 0);
            Assert.True((parsed.Flags & WorldItemSyncFlags.HasEnemyGrabDelay) != 0);
            Assert.False(parsed.Shimmered);
            Assert.Equal(0f, parsed.ShimmerTime);
            Assert.Equal((byte)0, parsed.EnemyGrabDelayTime);
            Assert.Equal(payload, parsed.ToPayloadArray());
        }

        [Fact]
        public void PlayerControls_ExposeProtocol325DashAndSnappingStoneBits()
        {
            Assert.Equal(0x80, (byte)UpdatePlayerControlFlags.ControlDash);
            Assert.Equal(0x80, (byte)UpdatePlayerMiscFlags2.AccSnappingStoneLightUp);

            PlayerUpdate packet = new PlayerUpdate
            {
                PlayerId = 3,
                ControlFlags = UpdatePlayerControlFlags.ControlDash,
                MiscFlags2 = UpdatePlayerMiscFlags2.AccSnappingStoneLightUp,
                SelectedItem = 1
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            PlayerUpdate parsed = Assert.IsType<PlayerUpdate>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.True((parsed.ControlFlags & UpdatePlayerControlFlags.ControlDash) != 0);
            Assert.True((parsed.MiscFlags2 & UpdatePlayerMiscFlags2.AccSnappingStoneLightUp) != 0);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void UnusedMeleeStrike_IsRejectedInProtocol325()
        {
            byte[] bytes = { 3, 0, (byte)PacketTypes.NpcItemStrike };

            Assert.Throws<System.IO.InvalidDataException>(() => TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            Assert.False(TerrariaPacket.TryDeserialize((ReadOnlyMemory<byte>)bytes, out _));
            Assert.Throws<NotSupportedException>(() => new NpcItemStrike().ToArray(includeHeader: true));
        }

        [Fact]
        public void DebugCommands_UsesProtocol325StringIntFloatFloatLayout()
        {
            DebugCommands packet = new DebugCommands
            {
                Command = "/randomizeprojslots",
                Value = 123456,
                Value2 = 1.25f,
                Value3 = -3.5f
            };

            byte[] bytes = packet.ToArray(includeHeader: true);
            DebugCommands parsed = Assert.IsType<DebugCommands>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            DebugCommandsView view = PacketViewParser.ParseExact(bytes).AsDebugCommandsView();

            Assert.Equal(packet.Command, parsed.Command);
            Assert.Equal(packet.Value, parsed.Value);
            Assert.Equal(packet.Value2, parsed.Value2);
            Assert.Equal(packet.Value3, parsed.Value3);
            Assert.Equal(packet.Command, view.Command);
            Assert.Equal(packet.Value, view.Value);
            Assert.Equal(packet.Value2, view.Value2);
            Assert.Equal(packet.Value3, view.Value3);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }
    }
}
