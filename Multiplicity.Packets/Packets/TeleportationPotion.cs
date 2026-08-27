using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The TeleportationPotion (0x49) packet.
    /// </summary>
    public class TeleportationPotion : TerrariaPacket
    {
        public const short PayloadLength = 1;

        /// <summary>
        /// Gets or sets the teleport subtype used by Terraria 1.4.5.x.
        /// </summary>
        public byte Subtype { get; set; }

        public TeleportationPotion()
            : base((byte)PacketTypes.TeleportationPotion)
        {
        }

        public TeleportationPotion(BinaryReader br)
            : base(br)
        {
            Subtype = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[TeleportationPotion: Subtype = {Subtype}]";
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

            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(Subtype);
            }
        }
    }
}
