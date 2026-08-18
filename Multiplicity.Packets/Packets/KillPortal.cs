using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The KillPortal (0x5F) packet.
    /// Carries the owner player id and the portal color/index to remove.
    /// </summary>
    public class KillPortal : TerrariaPacket
    {
        private const short PayloadLength = 3;

        /// <summary>
        /// Gets or sets the player id that owns the portal projectile.
        /// Terraria serializes this field as an unsigned 16-bit value.
        /// </summary>
        public ushort PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the portal color/index stored in projectile ai[1].
        /// </summary>
        public byte PortalColor { get; set; }

        /// <summary>
        /// Historical compatibility alias. Packet 95 does not contain a projectile index.
        /// </summary>
        [Obsolete("Packet 95 contains the portal owner player id, not a projectile index. Use PlayerId instead.")]
        public ushort ProjectileIndex
        {
            get => PlayerId;
            set => PlayerId = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KillPortal"/> class.
        /// </summary>
        public KillPortal()
            : base((byte)PacketTypes.KillPortal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KillPortal"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public KillPortal(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadUInt16();
            PortalColor = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[KillPortal: PlayerId = {PlayerId}, PortalColor = {PortalColor}]";
        }

        #region implemented abstract members of TerrariaPacket

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
                br.Write(PlayerId);
                br.Write(PortalColor);
            }
        }

        #endregion
    }
}
