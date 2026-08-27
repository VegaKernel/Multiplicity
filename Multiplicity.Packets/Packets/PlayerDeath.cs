using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 44 is unused in Terraria 1.4.5.x. Current player death synchronization uses PlayerDeathV2.
    /// This legacy type is retained only for source compatibility and must not be serialized.
    /// </summary>
    public class PlayerDeath : TerrariaPacket
    {
        public byte PlayerId { get; set; }
        public byte HitDirection { get; set; }
        public short Damage { get; set; }
        public bool Pvp { get; set; }
        public string DeathText { get; set; } = string.Empty;

        public PlayerDeath()
            : base((byte)PacketTypes.PlayerDeath)
        {
            throw new NotSupportedException("Packet 44 is unused in Terraria protocol 326; use PlayerDeathV2.");
        }

        public PlayerDeath(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 44 is unused in Terraria protocol 326.");
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            throw new NotSupportedException("Packet 44 cannot be sent in Terraria protocol 326; use PlayerDeathV2.");
        }

        public override string ToString()
        {
            return "[Unused44: unsupported in protocol 326]";
        }
    }
}
