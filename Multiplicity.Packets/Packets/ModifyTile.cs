using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ModifyTile (0x11) packet.
    /// </summary>
    public class ModifyTile : TerrariaPacket
    {
        private const short PayloadLength = 8;

        public ModifyTileAction Action { get; set; }
        public short TileX { get; set; }
        public short TileY { get; set; }
        public short Value { get; set; }
        public byte Style { get; set; }

        public ModifyTile()
            : base((byte)PacketTypes.ModifyTile)
        {
        }

        public ModifyTile(BinaryReader br)
            : base(br)
        {
            Action = (ModifyTileAction)br.ReadByte();
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            Value = br.ReadInt16();
            Style = br.ReadByte();
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

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write((byte)Action);
                bw.Write(TileX);
                bw.Write(TileY);
                bw.Write(Value);
                bw.Write(Style);
            }
        }

        public override string ToString()
        {
            return $"[ModifyTile: Action = {Action}, TileX = {TileX}, TileY = {TileY}, Value = {Value}, Style = {Style}]";
        }
    }
}
