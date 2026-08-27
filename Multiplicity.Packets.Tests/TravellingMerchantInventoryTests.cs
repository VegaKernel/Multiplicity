using System.IO;
using Multiplicity.Packets;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class TravellingMerchantInventoryTests
    {
        [Fact]
        public void DefaultPacket_SerializesFortyEmptySlots()
        {
            TravellingMerchantInventory packet = new TravellingMerchantInventory();

            byte[] payload = packet.ToPayloadArray();
            TravellingMerchantInventory parsed = Assert.IsType<TravellingMerchantInventory>(
                TerrariaPacket.DeserializePayload(PacketTypes.TravellingMerchantInventory, payload));

            Assert.Equal(80, payload.Length);
            Assert.Equal(40, parsed.Items.Length);
            Assert.All(parsed.Items, item => Assert.Equal((short)0, item));
        }

        [Fact]
        public void Serializer_RejectsNonProtocolSlotCount()
        {
            TravellingMerchantInventory packet = new TravellingMerchantInventory
            {
                Items = new short[39]
            };

            Assert.Throws<InvalidDataException>(() => packet.ToPayloadArray());
        }
    }
}
