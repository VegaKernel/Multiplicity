using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326WorldInfoTests
    {
        [Fact]
        public void MinimalPayload_UsesElevenWorldFlagBytesAndDungeonTail()
        {
            WorldInfo packet = new WorldInfo
            {
                EventInfo11 = 0xA1,
                SunDialCooldown = 0xB2,
                MoonDialCooldown = 0xC3,
                DungeonX = 0x1234,
                DungeonY = unchecked((short)0xABCD)
            };

            byte[] payload = packet.ToPayloadArray();
            WorldInfo parsed = Assert.IsType<WorldInfo>(
                TerrariaPacket.DeserializePayload(PacketTypes.WorldInfo, payload));
            WorldInfoView view = WorldInfoView.FromPayload(payload);

            // Protocol 326 has 163 fixed bytes when the variable WorldName bytes and extra
            // spawn point entries are excluded. An empty BinaryWriter string contributes one
            // 7-bit length byte, so the minimal serialized payload is 164 bytes.
            Assert.Equal(164, payload.Length);
            Assert.Equal(packet.EventInfo11, parsed.EventInfo11);
            Assert.Equal(packet.SunDialCooldown, parsed.SunDialCooldown);
            Assert.Equal(packet.MoonDialCooldown, parsed.MoonDialCooldown);
            Assert.Equal(packet.DungeonX, parsed.DungeonX);
            Assert.Equal(packet.DungeonY, parsed.DungeonY);
            Assert.Equal(packet.EventInfo11, view.EventInfo11);
            Assert.Equal(packet.SunDialCooldown, view.SunDialCooldown);
            Assert.Equal(packet.MoonDialCooldown, view.MoonDialCooldown);
            Assert.Equal(packet.DungeonX, view.DungeonX);
            Assert.Equal(packet.DungeonY, view.DungeonY);
            Assert.Empty(parsed.TrailingDataMemory.ToArray());
        }

        [Fact]
        public void ExtraSpawnPoints_AddFourBytesEachBeforeDungeonCoordinates()
        {
            WorldInfo packet = new WorldInfo
            {
                ExtraSpawnPoints = new[]
                {
                    new WorldSpawnPoint(100, 200),
                    new WorldSpawnPoint(-300, 400)
                },
                DungeonX = 3413,
                DungeonY = 234
            };

            byte[] payload = packet.ToPayloadArray();
            WorldInfo parsed = Assert.IsType<WorldInfo>(
                TerrariaPacket.DeserializePayload(PacketTypes.WorldInfo, payload));

            Assert.Equal(172, payload.Length);
            Assert.Equal(2, parsed.ExtraSpawnPoints.Length);
            Assert.Equal((short)100, parsed.ExtraSpawnPoints[0].X);
            Assert.Equal((short)200, parsed.ExtraSpawnPoints[0].Y);
            Assert.Equal((short)-300, parsed.ExtraSpawnPoints[1].X);
            Assert.Equal((short)400, parsed.ExtraSpawnPoints[1].Y);
            Assert.Equal((short)3413, parsed.DungeonX);
            Assert.Equal((short)234, parsed.DungeonY);
        }
    }
}
