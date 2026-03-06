using System.IO;

namespace Multiplicity.Packets.Models
{
    public sealed class TrackedProjectileReferenceData
    {
        private const short EmptyPayloadLength = 2;
        private const short TrackedPayloadLength = 6;

        public short OwnerIndex { get; set; } = -1;

        public short Identity { get; set; }

        public short ProjectileType { get; set; }

        public bool IsTrackingSomething
        {
            get
            {
                return OwnerIndex != -1;
            }
        }

        public TrackedProjectileReferenceData()
        {
        }

        public TrackedProjectileReferenceData(BinaryReader br)
        {
            OwnerIndex = br.ReadInt16();
            if (OwnerIndex == -1)
            {
                return;
            }

            Identity = br.ReadInt16();
            ProjectileType = br.ReadInt16();
        }

        public short GetLength()
        {
            return IsTrackingSomething ? TrackedPayloadLength : EmptyPayloadLength;
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(OwnerIndex);
            if (!IsTrackingSomething)
            {
                return;
            }

            bw.Write(Identity);
            bw.Write(ProjectileType);
        }
    }
}
