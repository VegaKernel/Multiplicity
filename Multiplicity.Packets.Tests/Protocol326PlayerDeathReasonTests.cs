using System;
using Multiplicity.Packets;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326PlayerDeathReasonTests
    {
        [Fact]
        public void PlayerHurtV2_PreservesExplicitEmptyCustomReasonAndOther254()
        {
            PlayerHurtV2 packet = new PlayerHurtV2
            {
                PlayerId = 1,
                PlayerDeathReason = PlayerDeathReasonFlags.FromOther | PlayerDeathReasonFlags.FromCustomReason,
                FromOther = 254,
                FromCustomReason = string.Empty,
                Damage = 123,
                HitDirection = 2,
                Flags = 3,
                CooldownCounter = -4
            };

            byte[] payload = packet.ToPayloadArray();
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(9, packet.GetLength());
            Assert.Equal(packet.GetLength(), payload.Length);
            Assert.Equal(payload.Length + TerrariaPacket.PacketHeaderLength, bytes.Length);
            Assert.Equal(bytes.Length, BitConverter.ToInt16(bytes, 0));
            Assert.Equal((byte)(PlayerDeathReasonFlags.FromOther | PlayerDeathReasonFlags.FromCustomReason), payload[1]);
            Assert.Equal((byte)254, payload[2]);
            Assert.Equal((byte)0, payload[3]);

            PlayerHurtV2 parsed = Assert.IsType<PlayerHurtV2>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.True((parsed.PlayerDeathReason & PlayerDeathReasonFlags.FromOther) != 0);
            Assert.True((parsed.PlayerDeathReason & PlayerDeathReasonFlags.FromCustomReason) != 0);
            Assert.Equal((byte)254, parsed.FromOther);
            Assert.Equal(string.Empty, parsed.FromCustomReason);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void PlayerDeathV2_PreservesExplicitEmptyCustomReasonAndOther254()
        {
            PlayerDeathV2 packet = new PlayerDeathV2
            {
                PlayerId = 2,
                PlayerDeathReason = PlayerDeathReasonFlags.FromOther | PlayerDeathReasonFlags.FromCustomReason,
                FromOther = 254,
                FromCustomReason = string.Empty,
                Damage = 321,
                HitDirection = 1,
                Flags = 1
            };

            byte[] payload = packet.ToPayloadArray();
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(8, packet.GetLength());
            Assert.Equal(packet.GetLength(), payload.Length);
            Assert.Equal(payload.Length + TerrariaPacket.PacketHeaderLength, bytes.Length);
            Assert.Equal(bytes.Length, BitConverter.ToInt16(bytes, 0));
            Assert.Equal((byte)(PlayerDeathReasonFlags.FromOther | PlayerDeathReasonFlags.FromCustomReason), payload[1]);
            Assert.Equal((byte)254, payload[2]);
            Assert.Equal((byte)0, payload[3]);

            PlayerDeathV2 parsed = Assert.IsType<PlayerDeathV2>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.True((parsed.PlayerDeathReason & PlayerDeathReasonFlags.FromOther) != 0);
            Assert.True((parsed.PlayerDeathReason & PlayerDeathReasonFlags.FromCustomReason) != 0);
            Assert.Equal((byte)254, parsed.FromOther);
            Assert.Equal(string.Empty, parsed.FromCustomReason);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }
    }
}
