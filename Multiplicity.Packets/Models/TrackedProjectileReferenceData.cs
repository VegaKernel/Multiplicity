using System;
using System.IO;

namespace Multiplicity.Packets.Models
{
    public sealed class TrackedProjectileReferenceData
    {
        public const short PayloadLength = sizeof(int) + sizeof(short);

        public ProjectileKey Key { get; set; }

        public short ProjectileType { get; set; }

        public bool IsTrackingSomething => ProjectileType != 0;

        [Obsolete("Use Key. Terraria serializes a packed ProjectileKey, not a standalone Int16 owner index.")]
        public short OwnerIndex
        {
            get => IsTrackingSomething ? Key.Spawner : (short)-1;
            set
            {
                if (value == -1)
                {
                    Key = default;
                    ProjectileType = 0;
                    return;
                }

                if (value < byte.MinValue || value > byte.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Projectile spawner must fit in a byte.");
                }

                Key = new ProjectileKey((byte)value, Key.Index, Key.Generation);
            }
        }

        [Obsolete("Use Key. Terraria serializes a packed ProjectileKey containing index and generation.")]
        public short Identity
        {
            get => checked((short)Key.Index);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Projectile index cannot be negative.");
                }

                Key = new ProjectileKey(Key.Spawner, checked((ushort)value), Key.Generation);
            }
        }

        public TrackedProjectileReferenceData()
        {
        }

        public TrackedProjectileReferenceData(BinaryReader br)
        {
            Key = ProjectileKey.FromPacked(br.ReadInt32());
            ProjectileType = br.ReadInt16();
        }

        public short GetLength()
        {
            return PayloadLength;
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(Key.ToInt32());
            bw.Write(ProjectileType);
        }
    }
}
