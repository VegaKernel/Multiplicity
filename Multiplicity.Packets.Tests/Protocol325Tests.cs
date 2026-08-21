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
        public void DamageNPCAck_RoundTripsAsEmptyPayloadPacket()
        {
            DamageNPCAck packet = new DamageNPCAck();
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(new byte[] { 3, 0, (byte)PacketTypes.DamageNPCAck }, bytes);
            Assert.IsType<DamageNPCAck>(TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
        }
    }
}
