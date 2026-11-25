using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Extensions.Dimensions;
using LegacyPlayerPlatformInfo = Multiplicity.Packets.Extensions.Mobile.PlayerPlatformInfo;
using LegacyServerInfo = Multiplicity.Packets.Extensions.Mobile.ServerInfo;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public sealed class PacketExtensionOverlayTests
    {
        [Fact]
        public void BarePacket67_FallsBackToVanillaPlaceholder()
        {
            Placeholder packet = new Placeholder();
            byte[] bytes = packet.ToArray(includeHeader: true);

            TerrariaPacket decoded = PacketDecoder.CommonExtensions.Decode(bytes);

            Assert.IsType<Placeholder>(decoded);
        }

        [Fact]
        public void DimensionsRealIp_OverridesPacket67WhenPayloadIsPresent()
        {
            DimensionsMessage packet = DimensionsMessage.CreateRealIp("203.0.113.42");
            byte[] bytes = packet.ToArray(includeHeader: true);

            TerrariaPacket vanilla = TerrariaPacket.Deserialize(bytes);
            TerrariaPacket decoded = PacketDecoder.CommonExtensions.Decode(bytes);
            DimensionsMessage dimensions = Assert.IsType<DimensionsMessage>(decoded);

            Assert.IsType<Placeholder>(vanilla);
            Assert.Equal(DimensionsMessage.RealIpMessageType, dimensions.MessageType);
            Assert.Equal("203.0.113.42", dimensions.ReportedRealIp);
            Assert.True(dimensions.TryGetReportedRealIp(out System.Net.IPAddress parsed));
            Assert.Equal("203.0.113.42", parsed.ToString());
            Assert.Equal(bytes, dimensions.ToArray(includeHeader: true));
        }

        [Fact]
        public void MobileServerInfo_RequiresExplicitLegacyOverlay()
        {
            LegacyServerInfo packet = new LegacyServerInfo(new byte[] { 1, 2, 3, 4 });
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.False(PacketDecoder.CommonExtensions.TryDecode(bytes, out _));
            TerrariaPacket decoded = new PacketDecoder(PacketExtensions.Mobile).Decode(bytes);
            LegacyServerInfo extension = Assert.IsType<LegacyServerInfo>(decoded);

            Assert.Equal(packet.Payload, extension.Payload);
        }

        [Fact]
        public void MobilePlayerPlatformInfo_UsesExtensionId164()
        {
            LegacyPlayerPlatformInfo packet = new LegacyPlayerPlatformInfo(new byte[] { 9, 8, 7 });
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.False(PacketDecoder.CommonExtensions.TryDecode(bytes, out _));
            TerrariaPacket decoded = new PacketDecoder(PacketExtensions.Mobile).Decode(bytes);
            LegacyPlayerPlatformInfo extension = Assert.IsType<LegacyPlayerPlatformInfo>(decoded);

            Assert.Equal(LegacyPlayerPlatformInfo.PacketId, extension.Id);
            Assert.Equal(packet.Payload, extension.Payload);
        }
    }
}
