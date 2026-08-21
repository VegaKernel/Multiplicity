using System;

namespace Multiplicity.Packets.Models
{
    /// <summary>
    /// Packed projectile identity used by Terraria protocol 325.
    /// Layout: spawner[0..7], index[8..17], generation[18..31].
    /// </summary>
    public readonly struct ProjectileKey : IEquatable<ProjectileKey>
    {
        private readonly uint _bits;

        public ProjectileKey(byte spawner, ushort index, ushort generation)
        {
            if (index > 1000)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Projectile index must be in range 0..1000.");
            if (generation > 16383)
                throw new ArgumentOutOfRangeException(nameof(generation), generation, "Projectile generation must be in range 0..16383.");

            _bits = (uint)(spawner | ((index & 0x03FF) << 8) | ((generation & 0x3FFF) << 18));
        }

        private ProjectileKey(uint bits)
        {
            _bits = bits;
        }

        public byte Spawner => (byte)(_bits & 0xFF);
        public ushort Index => (ushort)((_bits >> 8) & 0x03FF);
        public ushort Generation => (ushort)((_bits >> 18) & 0x3FFF);
        public uint PackedValue => _bits;

        public static ProjectileKey FromPacked(uint value) => new ProjectileKey(value);
        public static ProjectileKey FromPacked(int value) => new ProjectileKey(unchecked((uint)value));

        public int ToInt32() => unchecked((int)_bits);

        public bool Equals(ProjectileKey other) => _bits == other._bits;
        public override bool Equals(object obj) => obj is ProjectileKey other && Equals(other);
        public override int GetHashCode() => _bits.GetHashCode();
        public override string ToString() => $"spawner:{Spawner}, index:{Index}, gen:{Generation}";

        public static bool operator ==(ProjectileKey left, ProjectileKey right) => left.Equals(right);
        public static bool operator !=(ProjectileKey left, ProjectileKey right) => !left.Equals(right);
    }
}
