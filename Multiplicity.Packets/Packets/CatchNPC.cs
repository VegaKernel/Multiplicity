using System.IO;

namespace Multiplicity.Packets
{
    public sealed class CatchNPC : TerrariaPacket
    {
        public const short PayloadLength = 2;

        public short NPCId { get; set; }

        public CatchNPC()
            : base((byte)PacketTypes.CatchNPC)
        {
        }

        public CatchNPC(BinaryReader br)
            : base(br)
        {
            NPCId = br.ReadInt16();
            CheckLength();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            base.ToStream(stream, includeHeader);

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(NPCId);
            }
        }
    }
}
