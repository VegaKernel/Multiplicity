using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public sealed class Protocol325GolfPacketTests
    {
        [Fact]
        public void LandGolfBallInCup_UsesHitsAndProjectileTypeTail()
        {
            var packet = new LandGolfBallInCup
            {
                PlayerId = 4,
                CupX = 120,
                CupY = 80,
                Hits = 7,
                ProjectileType = 721
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(9, payload.Length);

            LandGolfBallInCup parsed = Assert.IsType<LandGolfBallInCup>(
                TerrariaPacket.DeserializePayload(PacketTypes.LandGolfBallInCup, payload));
            var view = new LandGolfBallInCupView(PacketView.FromPayload(PacketTypes.LandGolfBallInCup, payload));

            Assert.Equal((ushort)7, parsed.Hits);
            Assert.Equal((ushort)721, parsed.ProjectileType);
            Assert.Equal((ushort)7, view.Hits);
            Assert.Equal((ushort)721, view.ProjectileType);
        }
    }
}
