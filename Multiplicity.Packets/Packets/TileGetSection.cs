using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The TileGetSection (0x08) packet for Terraria protocol 325.
    /// </summary>
    public class TileGetSection : TerrariaPacket
    {
        public const short PayloadLength = 9;

        /// <summary>Tile X coordinate, or -1 for the spawn-area request.</summary>
        public int X { get; set; }

        /// <summary>Tile Y coordinate, or -1 for the spawn-area request.</summary>
        public int Y { get; set; }

        /// <summary>Client team included by Terraria 1.4.5.7.</summary>
        public byte Team { get; set; }

        public TileGetSection()
            : base((byte)PacketTypes.TileGetSection)
        {
        }

        public TileGetSection(BinaryReader br)
            : base(br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            Team = br.ReadByte();
        }

        public override string ToString() => $"[TileGetSection: X = {X} Y = {Y} Team = {Team}]";

        public override short GetLength() => PayloadLength;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter writer = new BinaryWriter(
                stream,
                new System.Text.UTF8Encoding(),
                leaveOpen: true);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Team);
        }
    }
}
