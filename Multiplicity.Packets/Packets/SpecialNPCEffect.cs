using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SpecialNPCEffect (0x33) packet.
    /// </summary>
    public class SpecialNPCEffect : TerrariaPacket
    {
        private const short PayloadLength = 2;

        public byte SubjectId { get; set; }
        public SpecialNpcEffectType EffectType { get; set; }

        public SpecialNPCEffect()
            : base((byte)PacketTypes.SpecialNPCEffect)
        {
        }

        public SpecialNPCEffect(BinaryReader br)
            : base(br)
        {
            SubjectId = br.ReadByte();
            EffectType = (SpecialNpcEffectType)br.ReadByte();
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
            return $"[SpecialNPCEffect: SubjectId = {SubjectId}, EffectType = {EffectType}]";
        }
    }
}
