using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 83 is unused in Terraria 1.4.5.x.
    /// This legacy type is retained only for source compatibility and must not be serialized.
    /// </summary>
    [PacketRegistrationIgnore]
    public class NpcKillCount : TerrariaPacket
    {
        public short NpcType { get; set; }
        public int KillCount { get; set; }

        public NpcKillCount()
            : base((byte)PacketTypes.NpcKillCount)
        {
            throw new NotSupportedException("Packet 83 is unused in Terraria protocol 326.");
        }

        public NpcKillCount(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 83 is unused in Terraria protocol 326.");
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            throw new NotSupportedException("Packet 83 cannot be sent in Terraria protocol 326.");
        }

        public override string ToString()
        {
            return "[Unused83: unsupported in protocol 326]";
        }
    }
}
