using System;
using Multiplicity.Packets;
using Multiplicity.Packets.Models;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326NpcBuffTests
    {
        [Fact]
        public void NpcUpdateBuff_UsesVariableUShortPairsAndTerminator()
        {
            NpcUpdateBuff packet = new NpcUpdateBuff
            {
                NpcId = 0x1234
            };
            packet.Buffs.Add(new NpcBuffEntry(300, 60000));
            packet.Buffs.Add(new NpcBuffEntry(44, 120));

            byte[] payload = packet.ToPayloadArray();

            Assert.Equal(new byte[]
            {
                0x34, 0x12,
                0x2C, 0x01,
                0x60, 0xEA,
                0x2C, 0x00,
                0x78, 0x00,
                0x00, 0x00
            }, payload);
            Assert.Equal(12, packet.GetLength());

            NpcUpdateBuff parsed = Assert.IsType<NpcUpdateBuff>(
                TerrariaPacket.DeserializePayload(PacketTypes.NpcUpdateBuff, payload));
            NpcUpdateBuffView view = NpcUpdateBuffView.FromPayload(payload);

            Assert.Equal((short)0x1234, parsed.NpcId);
            Assert.Equal(2, parsed.Buffs.Count);
            Assert.Equal((ushort)300, parsed.Buffs[0].BuffType);
            Assert.Equal((ushort)60000, parsed.Buffs[0].Time);
            Assert.Equal((ushort)44, parsed.Buffs[1].BuffType);
            Assert.Equal((ushort)120, parsed.Buffs[1].Time);
            Assert.Equal(2, view.Buffs.Count);
            Assert.Equal((ushort)300, view.Buffs[0].BuffType);
            Assert.Equal(payload, parsed.ToPayloadArray());
        }

        [Fact]
        public void NpcUpdateBuff_PreservesZeroTimeForPresentBuff()
        {
            NpcUpdateBuff packet = new NpcUpdateBuff { NpcId = 7 };
            packet.Buffs.Add(new NpcBuffEntry(511, 0));

            byte[] payload = packet.ToPayloadArray();
            NpcUpdateBuff parsed = Assert.IsType<NpcUpdateBuff>(
                TerrariaPacket.DeserializePayload(PacketTypes.NpcUpdateBuff, payload));

            Assert.Equal(8, payload.Length);
            Assert.Equal((ushort)511, parsed.Buffs[0].BuffType);
            Assert.Equal((ushort)0, parsed.Buffs[0].Time);
            Assert.Equal(payload, parsed.ToPayloadArray());
        }

        [Fact]
        public void NpcUpdateBuff_MetadataIsVariableAndPayloadParserStopsAtTerminator()
        {
            NpcUpdateBuff packet = new NpcUpdateBuff { NpcId = 9 };
            packet.Buffs.Add(new NpcBuffEntry(300, 15));
            byte[] payload = packet.ToPayloadArray();
            byte[] extended = new byte[payload.Length + 3];
            Buffer.BlockCopy(payload, 0, extended, 0, payload.Length);
            extended[^3] = 0xAA;
            extended[^2] = 0xBB;
            extended[^1] = 0xCC;

            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.NpcUpdateBuff);
            bool parsed = PacketViewParser.TryParsePayload(
                PacketTypes.NpcUpdateBuff,
                (ReadOnlySpan<byte>)extended,
                out PacketView view,
                out int consumed);

            Assert.False(metadata.HasFixedPayloadLength);
            Assert.True(parsed);
            Assert.Equal(payload.Length, consumed);
            Assert.Equal(payload.Length, view.PayloadSpan.Length);
        }
    }
}
