using System;
using Multiplicity.Packets;
using Multiplicity.Packets.Models;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326PresenceFlagTests
    {
        [Fact]
        public void ProjectileNew_PreservesExplicitZeroValuedOptionalFields()
        {
            ProjectileNew packet = new ProjectileNew
            {
                Key = new ProjectileKey(spawner: 2, index: 17, generation: 3),
                Type = 42,
                Flags = ProjectileNewFlags.HasAI0 |
                        ProjectileNewFlags.HasDamage |
                        ProjectileNewFlags.HasExtraFlags,
                ExtraFlags = ProjectileNewExtraFlags.HasAI2,
                AI0 = 0f,
                Damage = 0,
                AI2 = 0f
            };

            byte[] payload = packet.ToPayloadArray();
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(34, payload.Length);
            Assert.Equal(packet.GetLength(), payload.Length);
            Assert.Equal((byte)(ProjectileNewFlags.HasAI0 |
                                ProjectileNewFlags.HasDamage |
                                ProjectileNewFlags.HasExtraFlags), payload[22]);
            Assert.Equal((byte)ProjectileNewExtraFlags.HasAI2, payload[23]);

            ProjectileNew parsed = Assert.IsType<ProjectileNew>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.True((parsed.Flags & ProjectileNewFlags.HasAI0) != 0);
            Assert.True((parsed.Flags & ProjectileNewFlags.HasDamage) != 0);
            Assert.True((parsed.Flags & ProjectileNewFlags.HasExtraFlags) != 0);
            Assert.True((parsed.ExtraFlags & ProjectileNewExtraFlags.HasAI2) != 0);
            Assert.Equal(0f, parsed.AI0);
            Assert.Equal((short)0, parsed.Damage);
            Assert.Equal(0f, parsed.AI2);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void NpcUpdate_PreservesExplicitDefaultValuedOptionalFieldsAndFullLifeFlag()
        {
            NpcUpdate packet = new NpcUpdate
            {
                NpcSlot = 5,
                Generation = 2,
                Target = 7,
                NpcNetId = 1,
                Flags = NpcUpdateFlags.HasAI0 | NpcUpdateFlags.LifeIsFull,
                ExtraFlags = NpcUpdateExtraFlags.HasPlayerCountScaling |
                             NpcUpdateExtraFlags.HasDifficultyOverride,
                StatsScaledForPlayerCount = 1,
                Difficulty = 1f,
                Life = 0
            };
            packet.AI[0] = 0f;

            byte[] payload = packet.ToPayloadArray();
            byte[] bytes = packet.ToArray(includeHeader: true);

            Assert.Equal(33, payload.Length);
            Assert.Equal(packet.GetLength(), payload.Length);
            Assert.Equal((byte)(NpcUpdateFlags.HasAI0 | NpcUpdateFlags.LifeIsFull), payload[20]);
            Assert.Equal((byte)(NpcUpdateExtraFlags.HasPlayerCountScaling |
                                NpcUpdateExtraFlags.HasDifficultyOverride), payload[21]);

            NpcUpdate parsed = Assert.IsType<NpcUpdate>(
                TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes));

            Assert.True((parsed.Flags & NpcUpdateFlags.HasAI0) != 0);
            Assert.True((parsed.Flags & NpcUpdateFlags.LifeIsFull) != 0);
            Assert.True((parsed.ExtraFlags & NpcUpdateExtraFlags.HasPlayerCountScaling) != 0);
            Assert.True((parsed.ExtraFlags & NpcUpdateExtraFlags.HasDifficultyOverride) != 0);
            Assert.Equal(0f, parsed.AI[0]);
            Assert.Equal((byte)1, parsed.StatsScaledForPlayerCount);
            Assert.Equal(1f, parsed.Difficulty);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }
    }
}
