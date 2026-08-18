using System;
using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol319Tests
    {
        [Fact]
        public void ConnectRequest_DefaultsToTerraria319()
        {
            ConnectRequest packet = new ConnectRequest();

            Assert.Equal(319, ConnectRequest.Protocol319);
            Assert.Equal(ConnectRequest.Protocol319, ConnectRequest.CurrentProtocol);
            Assert.Equal("Terraria319", packet.Version);
            Assert.True(packet.IsProtocol(319));

            byte[] bytes = packet.ToArray(includeHeader: true);
            ConnectRequest parsed = Assert.IsType<ConnectRequest>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));
            PacketView packetView = PacketViewParser.ParseExact(bytes);
            ConnectRequestView view = packetView.AsConnectRequestView();

            Assert.Equal("Terraria319", parsed.Version);
            Assert.Equal("Terraria319", view.Version);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void KillPortal_UsesProtocol319WireLayout()
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
        public void DesktopPacketIdRange_RemainsUnchangedForProtocol319()
        {
            Assert.Equal(161, (int)PacketTypes.HostToken);
            Assert.Equal(162, (int)PacketTypes.ServerInfo);
            Assert.Equal(163, (int)PacketTypes.PlayerPlatformInfo);
        }
    }
}
