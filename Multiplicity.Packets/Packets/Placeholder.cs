using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Vanilla-unused Placeholder packet (0x43 / 67).
    /// Extension protocols may overlay this id through PacketExtensionRegistry.
    /// </summary>
    public class Placeholder : TerrariaPacket
    {
        public Placeholder()
            : base((byte)PacketTypes.Placeholder)
        {
        }

        public Placeholder(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return "[Placeholder]";
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }
        }
    }
}
