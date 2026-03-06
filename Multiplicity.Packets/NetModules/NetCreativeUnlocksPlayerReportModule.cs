using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetCreativeUnlocksPlayerReportModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(byte) + sizeof(ushort) + sizeof(ushort);

        public byte UserId { get; set; }

        public ushort ItemId { get; set; }

        public ushort Amount { get; set; }

        public NetCreativeUnlocksPlayerReportModule()
            : base(NetworkModuleTypes.NetCreativeUnlocksPlayerReportModule)
        {
        }

        public NetCreativeUnlocksPlayerReportModule(BinaryReader br)
            : base(br)
        {
            UserId = br.ReadByte();
            ItemId = br.ReadUInt16();
            Amount = br.ReadUInt16();
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
            bw.Write(UserId);
            bw.Write(ItemId);
            bw.Write(Amount);
        }
    }
}