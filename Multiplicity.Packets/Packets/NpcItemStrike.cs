using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 24 is UnusedMeleeStrike in Terraria 1.4.5.7 / protocol 325.
    /// Vanilla treats receiving it as an invariant violation.
    /// </summary>
    [PacketRegistrationIgnore]
    public sealed class NpcItemStrike : TerrariaPacket
    {
        public short NpcId { get; set; }
        public byte PlayerId { get; set; }

        public NpcItemStrike()
            : base((byte)PacketTypes.NpcItemStrike)
        {
            throw new NotSupportedException("Packet 24 (UnusedMeleeStrike) is not valid in Terraria protocol 325.");
        }

        public NpcItemStrike(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 24 (UnusedMeleeStrike) is not valid in Terraria protocol 325.");
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            throw new NotSupportedException("Packet 24 (UnusedMeleeStrike) cannot be sent in Terraria protocol 325.");
        }

        public override string ToString()
        {
            return "[UnusedMeleeStrike: unsupported in protocol 325]";
        }
    }
}
