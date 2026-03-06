using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetParticlesModule : TerrariaNetModule
    {
        private const short PayloadLength =
            sizeof(byte) +
            (sizeof(float) * 4) +
            sizeof(int) +
            sizeof(byte);

        public byte ParticleType { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float MovementX { get; set; }

        public float MovementY { get; set; }

        public int UniqueInfoPiece { get; set; }

        public byte InvokingPlayerIndex { get; set; }

        public NetParticlesModule()
            : base(NetworkModuleTypes.NetParticlesModule)
        {
        }

        public NetParticlesModule(BinaryReader br)
            : base(br)
        {
            ParticleType = br.ReadByte();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            MovementX = br.ReadSingle();
            MovementY = br.ReadSingle();
            UniqueInfoPiece = br.ReadInt32();
            InvokingPlayerIndex = br.ReadByte();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(ParticleType);
            bw.Write(PositionX);
            bw.Write(PositionY);
            bw.Write(MovementX);
            bw.Write(MovementY);
            bw.Write(UniqueInfoPiece);
            bw.Write(InvokingPlayerIndex);
        }
    }
}