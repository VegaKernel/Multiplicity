using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcSpecial (0x33) packet.
    /// </summary>
    public class NpcSpecial : TerrariaPacket
    {
        private const short PayloadLength = 2;

        public byte SubjectId { get; set; }
        public NpcSpecialType EffectType { get; set; }

        public NpcSpecial()
            : base((byte)PacketTypes.NpcSpecial)
        {
        }

        public NpcSpecial(BinaryReader br)
            : base(br)
        {
            SubjectId = br.ReadByte();
            EffectType = (NpcSpecialType)br.ReadByte();
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

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(SubjectId);
                bw.Write((byte)EffectType);
            }
        }

        public override string ToString()
        {
            return $"[NpcSpecial: SubjectId = {SubjectId}, EffectType = {EffectType}]";
        }
    }
}

