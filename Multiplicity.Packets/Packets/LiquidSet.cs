using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 48 is deprecated in Terraria 1.4.5.x. Current liquid synchronization uses NetLiquidModule.
    /// This legacy type is retained only for source compatibility and must not be serialized.
    /// </summary>
    public class LiquidSet : TerrariaPacket
    {
        public short X { get; set; }
        public short Y { get; set; }
        public byte Liquid { get; set; }
        public LiquidKind LiquidType { get; set; }

        public LiquidSet()
            : base((byte)PacketTypes.LiquidSet)
        {
            throw new NotSupportedException("Packet 48 is deprecated in Terraria protocol 326; use NetLiquidModule.");
        }

        public LiquidSet(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 48 is deprecated in Terraria protocol 326.");
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            throw new NotSupportedException("Packet 48 cannot be sent in Terraria protocol 326; use NetLiquidModule.");
        }

        public override string ToString()
        {
            return "[DeprecatedLiquidUpdate48: unsupported in protocol 326]";
        }
    }
}
