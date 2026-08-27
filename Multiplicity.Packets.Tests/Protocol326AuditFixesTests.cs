using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326AuditFixesTests
    {
        [Fact]
        public void Zones_UsesSevenByteProtocol326Layout()
        {
            Zones packet = new Zones
            {
                PlayerId = 7,
                Zone1 = 1,
                Zone2 = 2,
                Zone3 = 3,
                Zone4 = 4,
                Zone5 = 5,
                TownNpcCount = 6
            };

            byte[] payload = packet.ToPayloadArray();
            Zones parsed = Assert.IsType<Zones>(TerrariaPacket.DeserializePayload(PacketTypes.Zones, payload));
            ZonesView view = ZonesView.FromPayload(payload);
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.Zones);

            Assert.Equal(7, payload.Length);
            Assert.Equal(new byte[] { 7, 1, 2, 3, 4, 5, 6 }, payload);
            Assert.Equal(packet.Zone5, parsed.Zone5);
            Assert.Equal(packet.TownNpcCount, parsed.TownNpcCount);
            Assert.Equal(packet.Zone5, view.Zone5);
            Assert.Equal(packet.TownNpcCount, view.TownNpcCount);
            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal(Zones.PayloadLength, metadata.FixedPayloadLength);
        }

        [Fact]
        public void TeleportationPotion_CarriesSubtypeByte()
        {
            TeleportationPotion packet = new TeleportationPotion { Subtype = 4 };

            byte[] payload = packet.ToPayloadArray();
            TeleportationPotion parsed = Assert.IsType<TeleportationPotion>(
                TerrariaPacket.DeserializePayload(PacketTypes.TeleportationPotion, payload));
            TeleportationPotionView view = TeleportationPotionView.FromPayload(payload);
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.TeleportationPotion);

            Assert.Equal(new byte[] { 4 }, payload);
            Assert.Equal(4, parsed.Subtype);
            Assert.Equal(4, view.Subtype);
            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal(TeleportationPotion.PayloadLength, metadata.FixedPayloadLength);
        }

        [Fact]
        public void SyncExtraValue_UsesInt32DeltaOnWire()
        {
            const int extraValue = 0x01020304;
            SyncExtraValue packet = new SyncExtraValue
            {
                NpcIndex = 42,
                ExtraValue = extraValue,
                X = 160.5f,
                Y = 320.25f
            };

            byte[] payload = packet.ToPayloadArray();
            SyncExtraValue parsed = Assert.IsType<SyncExtraValue>(
                TerrariaPacket.DeserializePayload(PacketTypes.SyncExtraValue, payload));
            SyncExtraValueView view = SyncExtraValueView.FromPayload(payload);

            Assert.Equal(SyncExtraValue.PayloadLength, payload.Length);
            Assert.Equal(new byte[] { 0x04, 0x03, 0x02, 0x01 }, payload[2..6]);
            Assert.Equal(extraValue, parsed.ExtraValue);
            Assert.Equal(extraValue, view.ExtraValue);
            Assert.Equal(packet.X, parsed.X);
            Assert.Equal(packet.Y, parsed.Y);
        }

        [Fact]
        public void NpcSpecial_IncludesProtocol326PartyGirlCakeEffect()
        {
            Assert.Equal(10, (byte)NpcSpecialType.PartyGirlCake);
        }
    }
}
