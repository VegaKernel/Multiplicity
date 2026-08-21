using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Protocol 325 acknowledgement for DamageNPC (packet 162).
    /// </summary>
    public sealed class DamageNPCAck : TerrariaPacket
    {
        public DamageNPCAck()
            : base((byte)PacketTypes.DamageNPCAck)
        {
        }

        public DamageNPCAck(BinaryReader br)
            : base(br)
        {
        }

        public override short GetLength() => 0;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }
        }
    }
}
