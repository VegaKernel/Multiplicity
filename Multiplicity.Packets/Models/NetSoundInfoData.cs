using System.IO;

namespace Multiplicity.Packets.Models
{
    public sealed class NetSoundInfoData
    {
        private const byte HasStyleFlag = 1 << 0;
        private const byte HasVolumeFlag = 1 << 1;
        private const byte HasPitchOffsetFlag = 1 << 2;
        private const short BasePayloadLength = 11;

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public ushort SoundIndex { get; set; }

        public int? Style { get; set; }

        public float? Volume { get; set; }

        public float? PitchOffset { get; set; }

        public NetSoundInfoData()
        {
        }

        public NetSoundInfoData(BinaryReader br)
        {
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            SoundIndex = br.ReadUInt16();

            byte flags = br.ReadByte();
            if ((flags & HasStyleFlag) != 0)
            {
                Style = br.ReadInt32();
            }

            if ((flags & HasVolumeFlag) != 0)
            {
                Volume = br.ReadSingle();
            }

            if ((flags & HasPitchOffsetFlag) != 0)
            {
                PitchOffset = br.ReadSingle();
            }
        }

        public short GetLength()
        {
            short length = BasePayloadLength;
            if (Style.HasValue)
            {
                length += sizeof(int);
            }

            if (Volume.HasValue)
            {
                length += sizeof(float);
            }

            if (PitchOffset.HasValue)
            {
                length += sizeof(float);
            }

            return length;
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(PositionX);
            bw.Write(PositionY);
            bw.Write(SoundIndex);

            byte flags = 0;
            if (Style.HasValue)
            {
                flags |= HasStyleFlag;
            }

            if (Volume.HasValue)
            {
                flags |= HasVolumeFlag;
            }

            if (PitchOffset.HasValue)
            {
                flags |= HasPitchOffsetFlag;
            }

            bw.Write(flags);

            if (Style.HasValue)
            {
                bw.Write(Style.Value);
            }

            if (Volume.HasValue)
            {
                bw.Write(Volume.Value);
            }

            if (PitchOffset.HasValue)
            {
                bw.Write(PitchOffset.Value);
            }
        }
    }
}
