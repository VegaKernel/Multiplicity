using Multiplicity.Packets;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326ChestNameTests
    {
        [Fact]
        public void LookupRequest_UsesSixByteFormWithoutName()
        {
            ChestName packet = new ChestName
            {
                ChestId = -1,
                ChestX = 123,
                ChestY = 456,
                HasName = false
            };

            byte[] payload = packet.ToPayloadArray();
            ChestName parsed = Assert.IsType<ChestName>(
                TerrariaPacket.DeserializePayload(PacketTypes.ChestName, payload));

            Assert.Equal(6, packet.GetLength());
            Assert.Equal(6, payload.Length);
            Assert.Equal((short)-1, parsed.ChestId);
            Assert.Equal((short)123, parsed.ChestX);
            Assert.Equal((short)456, parsed.ChestY);
            Assert.False(parsed.HasName);
            Assert.Empty(parsed.Name);
        }

        [Fact]
        public void Response_AppendsLengthPrefixedName()
        {
            ChestName packet = new ChestName
            {
                ChestId = 42,
                ChestX = 123,
                ChestY = 456,
                HasName = true,
                Name = "Loot"
            };

            byte[] payload = packet.ToPayloadArray();
            ChestName parsed = Assert.IsType<ChestName>(
                TerrariaPacket.DeserializePayload(PacketTypes.ChestName, payload));

            Assert.Equal(packet.GetLength(), payload.Length);
            Assert.True(parsed.HasName);
            Assert.Equal("Loot", parsed.Name);
        }
    }
}
