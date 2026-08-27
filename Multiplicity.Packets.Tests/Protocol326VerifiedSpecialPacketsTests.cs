using Multiplicity.Packets;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326VerifiedSpecialPacketsTests
    {
        [Fact]
        public void NpcSpecial_PartyGirlCake_UsesTwoBytePayload()
        {
            NpcSpecial packet = new NpcSpecial
            {
                SubjectId = 37,
                EffectType = NpcSpecialType.PartyGirlCake
            };

            byte[] payload = packet.ToPayloadArray();
            NpcSpecial parsed = Assert.IsType<NpcSpecial>(
                TerrariaPacket.DeserializePayload(PacketTypes.NpcSpecial, payload));

            Assert.Equal(new byte[] { 37, 10 }, payload);
            Assert.Equal((byte)37, parsed.SubjectId);
            Assert.Equal(NpcSpecialType.PartyGirlCake, parsed.EffectType);
        }

        [Fact]
        public void ToggleParty_HasNoPayload()
        {
            ToggleParty packet = new ToggleParty();

            byte[] payload = packet.ToPayloadArray();
            ToggleParty parsed = Assert.IsType<ToggleParty>(
                TerrariaPacket.DeserializePayload(PacketTypes.ToggleParty, payload));

            Assert.Empty(payload);
            Assert.NotNull(parsed);
        }

        [Fact]
        public void FoodPlatterTryPlacing_UsesSharedNineByteItemPlacementLayout()
        {
            FoodPlatterTryPlacing packet = new FoodPlatterTryPlacing
            {
                TileX = -123,
                TileY = 456,
                ItemType = 789,
                Prefix = 17,
                Stack = 23
            };

            byte[] payload = packet.ToPayloadArray();
            FoodPlatterTryPlacing parsed = Assert.IsType<FoodPlatterTryPlacing>(
                TerrariaPacket.DeserializePayload(PacketTypes.FoodPlatterTryPlacing, payload));

            Assert.Equal(9, payload.Length);
            Assert.Equal(packet.TileX, parsed.TileX);
            Assert.Equal(packet.TileY, parsed.TileY);
            Assert.Equal(packet.ItemType, parsed.ItemType);
            Assert.Equal(packet.Prefix, parsed.Prefix);
            Assert.Equal(packet.Stack, parsed.Stack);
        }

        [Fact]
        public void DeadCellsDisplayJar_UsesSharedNineByteItemPlacementLayout()
        {
            TEDeadCellsDisplayJar packet = new TEDeadCellsDisplayJar
            {
                TileX = 321,
                TileY = -654,
                ItemType = 987,
                Prefix = 4,
                Stack = 11
            };

            byte[] payload = packet.ToPayloadArray();
            TEDeadCellsDisplayJar parsed = Assert.IsType<TEDeadCellsDisplayJar>(
                TerrariaPacket.DeserializePayload(PacketTypes.TEDeadCellsDisplayJar, payload));

            Assert.Equal(9, payload.Length);
            Assert.Equal(packet.TileX, parsed.TileX);
            Assert.Equal(packet.TileY, parsed.TileY);
            Assert.Equal(packet.ItemType, parsed.ItemType);
            Assert.Equal(packet.Prefix, parsed.Prefix);
            Assert.Equal(packet.Stack, parsed.Stack);
        }
    }
}
