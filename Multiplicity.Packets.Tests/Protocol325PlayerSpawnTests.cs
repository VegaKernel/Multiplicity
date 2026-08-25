using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol325PlayerSpawnTests
    {
        [Fact]
        public void PlayerSpawn_UsesCompleteProtocol325Payload()
        {
            PlayerSpawn packet = new PlayerSpawn
            {
                PlayerId = 7,
                SpawnX = 123,
                SpawnY = 456,
                RespawnTimer = 180,
                NumberOfDeathsPVE = 12,
                NumberOfDeathsPVP = 3,
                Team = 4,
                SpawnContext = 2
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(PlayerSpawn.PayloadLength, payload.Length);

            PlayerSpawn parsed = Assert.IsType<PlayerSpawn>(
                TerrariaPacket.DeserializePayload(PacketTypes.PlayerSpawn, payload));
            PlayerSpawnView view = PlayerSpawnView.FromPayload(payload);

            Assert.Equal(packet.PlayerId, parsed.PlayerId);
            Assert.Equal(packet.SpawnX, parsed.SpawnX);
            Assert.Equal(packet.SpawnY, parsed.SpawnY);
            Assert.Equal(packet.RespawnTimer, parsed.RespawnTimer);
            Assert.Equal(packet.NumberOfDeathsPVE, parsed.NumberOfDeathsPVE);
            Assert.Equal(packet.NumberOfDeathsPVP, parsed.NumberOfDeathsPVP);
            Assert.Equal(packet.Team, parsed.Team);
            Assert.Equal(packet.SpawnContext, parsed.SpawnContext);

            Assert.Equal(packet.PlayerId, view.PlayerId);
            Assert.Equal(packet.RespawnTimer, view.RespawnTimer);
            Assert.Equal(packet.NumberOfDeathsPVE, view.NumberOfDeathsPVE);
            Assert.Equal(packet.NumberOfDeathsPVP, view.NumberOfDeathsPVP);
            Assert.Equal(packet.Team, view.Team);
            Assert.Equal(packet.SpawnContext, view.SpawnContext);
            Assert.Equal(payload, parsed.ToPayloadArray());
        }

        [Fact]
        public void PlayerSpawn_MetadataReportsFifteenBytePayload()
        {
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.PlayerSpawn);

            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal(PlayerSpawn.PayloadLength, metadata.FixedPayloadLength);
        }
    }
}
