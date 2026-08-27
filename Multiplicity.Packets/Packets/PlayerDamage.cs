using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 26 is unused in Terraria 1.4.5.x. Current player damage uses the V2 damage packets.
    /// This legacy type is retained only for source compatibility and must not be serialized.
    /// </summary>
    [PacketRegistrationIgnore]
    public class PlayerDamage : TerrariaPacket
    {
        public byte PlayerId { get; set; }
        public byte HitDirection { get; set; }
        public short Damage { get; set; }
        public string DeathText { get; set; } = string.Empty;
        public byte Flags { get; set; }

        public PlayerDamage()
            : base((byte)PacketTypes.PlayerDamage)
        {
            throw new NotSupportedException("Packet 26 is unused in Terraria protocol 326; use PlayerHurtV2/PlayerDeathV2.");
        }

        public PlayerDamage(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 26 is unused in Terraria protocol 326.");
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            throw new NotSupportedException("Packet 26 cannot be sent in Terraria protocol 326; use PlayerHurtV2/PlayerDeathV2.");
        }

        public override string ToString()
        {
            return "[Unused26: unsupported in protocol 326]";
        }
    }
}
