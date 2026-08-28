using System.IO;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public sealed class ConnectRequestViewTests
    {
        [Fact]
        public void Parses_protocol_326_banner_directly_from_payload()
        {
            byte[] payload = [11, .. "Terraria326"u8.ToArray()];

            ConnectRequestView view = ConnectRequestView.FromPayload(payload);

            Assert.Equal("Terraria326", view.Version);
            Assert.Equal("Terraria326", view.Packet.Version);
        }

        [Fact]
        public void Rejects_trailing_bytes_after_length_prefixed_banner()
        {
            byte[] payload = [11, .. "Terraria326"u8.ToArray(), 0xFF];

            Assert.Throws<InvalidDataException>(() => Parse(payload));
        }

        [Fact]
        public void Rejects_truncated_length_prefixed_banner()
        {
            byte[] payload = [11, .. "Terraria32"u8.ToArray()];

            Assert.Throws<InvalidDataException>(() => Parse(payload));
        }

        private static void Parse(byte[] payload)
        {
            _ = ConnectRequestView.FromPayload(payload);
        }
    }
}
