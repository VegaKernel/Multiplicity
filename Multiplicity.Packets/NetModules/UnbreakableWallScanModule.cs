using System.IO;

namespace Multiplicity.Packets
{
    public sealed class UnbreakableWallScanModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(byte) + sizeof(bool);

        public byte PlayerId { get; set; }

        public bool InsideUnbreakableWalls { get; set; }

        public UnbreakableWallScanModule()
            : base(NetworkModuleTypes.UnbreakableWallScanModule)
        {
        }

        public UnbreakableWallScanModule(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            InsideUnbreakableWalls = br.ReadBoolean();
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
            bw.Write(PlayerId);
            bw.Write(InsideUnbreakableWalls);
        }
    }
}