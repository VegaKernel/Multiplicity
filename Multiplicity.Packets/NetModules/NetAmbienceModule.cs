using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetAmbienceModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(byte) + sizeof(int) + sizeof(byte);

        public byte PlayerId { get; set; }

        public int Seed { get; set; }

        public byte SkyEntityType { get; set; }

        public NetAmbienceModule()
            : base(NetworkModuleTypes.NetAmbienceModule)
        {
        }

        public NetAmbienceModule(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            Seed = br.ReadInt32();
            SkyEntityType = br.ReadByte();
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
            bw.Write(Seed);
            bw.Write(SkyEntityType);
        }
    }
}