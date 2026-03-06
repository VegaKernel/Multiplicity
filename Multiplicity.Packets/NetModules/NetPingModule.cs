using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetPingModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(float) * 2;

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public NetPingModule()
            : base(NetworkModuleTypes.NetPingModule)
        {
        }

        public NetPingModule(BinaryReader br)
            : base(br)
        {
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
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
            bw.Write(PositionX);
            bw.Write(PositionY);
        }
    }
}