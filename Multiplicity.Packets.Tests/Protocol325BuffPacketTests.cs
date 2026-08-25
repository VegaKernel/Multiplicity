using System;
using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public sealed class Protocol325BuffPacketTests
    {
        [Fact]
        public void NpcAddBuff_UsesSixBytePayloadWithUInt16BuffId()
        {
            var packet = new NpcAddBuff
            {
                NpcId = 17,
                Buff = 0x1234,
                Time = 900
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(new byte[] { 17, 0, 0x34, 0x12, 0x84, 0x03 }, payload);

            NpcAddBuff parsed = Assert.IsType<NpcAddBuff>(
                TerrariaPacket.DeserializePayload(PacketTypes.NpcAddBuff, payload));
            var view = new NpcAddBuffView(PacketView.FromPayload(PacketTypes.NpcAddBuff, payload));

            Assert.Equal((ushort)0x1234, parsed.Buff);
            Assert.Equal((ushort)0x1234, view.Buff);
            Assert.Equal((short)900, parsed.Time);
            Assert.Equal((short)900, view.Time);
        }

        [Fact]
        public void PlayerAddBuff_UsesSevenBytePayloadWithUInt16BuffId()
        {
            var packet = new PlayerAddBuff
            {
                PlayerId = 7,
                Buff = 0x1234,
                Time = 120000
            };

            byte[] payload = packet.ToPayloadArray();
            Assert.Equal(7, payload.Length);

            PlayerAddBuff parsed = Assert.IsType<PlayerAddBuff>(
                TerrariaPacket.DeserializePayload(PacketTypes.PlayerAddBuff, payload));
            var view = new PlayerAddBuffView(PacketView.FromPayload(PacketTypes.PlayerAddBuff, payload));

            Assert.Equal((byte)7, parsed.PlayerId);
            Assert.Equal((ushort)0x1234, parsed.Buff);
            Assert.Equal((ushort)0x1234, view.Buff);
            Assert.Equal(120000, parsed.Time);
            Assert.Equal(120000, view.Time);
        }
    }
}
