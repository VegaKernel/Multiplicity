using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol325TileGetSectionTests
    {
        [Fact]
        public void TileGetSection_IncludesTeamByte()
        {
            TileGetSection packet = new TileGetSection
            {
                X = -1,
                Y = -1,
                Team = 4
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(TileGetSection.PayloadLength, payload.Length);

            TileGetSection parsed = Assert.IsType<TileGetSection>(
                TerrariaPacket.DeserializePayload(PacketTypes.TileGetSection, payload));
            TileGetSectionView view = TileGetSectionView.FromPayload(payload);

            Assert.Equal(packet.X, parsed.X);
            Assert.Equal(packet.Y, parsed.Y);
            Assert.Equal(packet.Team, parsed.Team);
            Assert.Equal(packet.X, view.X);
            Assert.Equal(packet.Y, view.Y);
            Assert.Equal(packet.Team, view.Team);
            Assert.Equal(payload, parsed.ToPayloadArray());
        }

        [Fact]
        public void TileGetSection_MetadataReportsNineBytePayload()
        {
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.TileGetSection);

            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal(TileGetSection.PayloadLength, metadata.FixedPayloadLength);
        }
    }
}
