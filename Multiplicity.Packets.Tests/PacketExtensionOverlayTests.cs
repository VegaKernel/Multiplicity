using System;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Extensions.Dimensions;
using Multiplicity.Packets.Extensions.Tsapi;
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
        public void TsapiServerInfo_IsDecodedOnlyByExtensionAwareDecoder()
        {
            ServerInfo packet = new ServerInfo
            {
                Payload = new byte[] { 1, 2, 3, 4 }
            };
            byte[] bytes = packet.ToArray(includeHeader: true);

            TerrariaPacket vanilla = TerrariaPacket.Deserialize(bytes);
            TerrariaPacket decoded = PacketDecoder.CommonExtensions.Decode(bytes);
            ServerInfo extension = Assert.IsType<ServerInfo>(decoded);

            Assert.IsType<ServerInfo>(vanilla); // Legacy compatibility path until the next breaking release.
            Assert.Equal(packet.Payload, extension.Payload);
        }

        [Fact]
        public void TsapiPlayerPlatformInfo_UsesExtensionId164()
        {
            PlayerPlatformInfo packet = new PlayerPlatformInfo
            {
                Payload = new byte[] { 9, 8, 7 }
            };

            TerrariaPacket decoded = PacketDecoder.CommonExtensions.Decode(packet.ToArray(includeHeader: true));
            PlayerPlatformInfo extension = Assert.IsType<PlayerPlatformInfo>(decoded);

            Assert.Equal(PlayerPlatformInfo.PacketId, extension.Id);
            Assert.Equal(packet.Payload, extension.Payload);
        }
    }
}
