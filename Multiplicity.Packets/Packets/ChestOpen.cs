using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ChestOpen (0x21) packet.
    /// </summary>
    public class ChestOpen : TerrariaPacket
    {
        public const byte InvalidNameLength = byte.MaxValue;
        public const byte MaxChestNameLength = 20;

        private const short FixedPayloadLength = 7;

        public short ChestId { get; set; }

        public short ChestX { get; set; }

        public short ChestY { get; set; }

        public byte NameLength { get; set; }

        /// <summary>
        /// Gets or sets the chest name when the name marker is between 1 and 20.
        /// </summary>
        public string ChestName { get; set; }

        public ChestOpen()
            : base((byte)PacketTypes.ChestOpen)
        {
            ChestName = string.Empty;
        }

        public ChestOpen(BinaryReader br)
            : base(br)
        {
            ChestId = br.ReadInt16();
            ChestX = br.ReadInt16();
            ChestY = br.ReadInt16();
            NameLength = br.ReadByte();
            ChestName = string.Empty;

            if (NameLength > 0)
            {
                if (NameLength <= MaxChestNameLength)
                {
                    ChestName = br.ReadString();
                }
                else if (NameLength != InvalidNameLength)
                {
                    // Terraria preserves 255 as the explicit "no valid name" sentinel,
                    // but normalizes every other out-of-range marker to zero.
                    NameLength = 0;
                }
            }
        }

        public override string ToString()
        {
            return $"[ChestOpen: ChestId = {ChestId} ChestX = {ChestX} ChestY = {ChestY} NameLength = {NameLength} ChestName = {ChestName}]";
        }

        public override short GetLength()
        {
            string chestName = ChestName ?? string.Empty;
            if (chestName.Length == 0 || chestName.Length > MaxChestNameLength)
            {
                return FixedPayloadLength;
            }

            return checked((short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(chestName)));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);

            bw.Write(ChestId);
            bw.Write(ChestX);
            bw.Write(ChestY);

            string chestName = ChestName ?? string.Empty;
            if (chestName.Length == 0)
            {
                NameLength = 0;
            }
            else if (chestName.Length > MaxChestNameLength)
            {
                NameLength = InvalidNameLength;
            }
            else
            {
                NameLength = (byte)chestName.Length;
            }

            bw.Write(NameLength);
            if (NameLength > 0 && NameLength <= MaxChestNameLength)
            {
                bw.Write(chestName);
            }
        }
    }
}
