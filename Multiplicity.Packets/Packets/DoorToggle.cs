using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The DoorToggle (0x13) packet.
    /// </summary>
    public class DoorToggle : TerrariaPacket
    {
        private const short PayloadLength = 6;

        public DoorToggleAction Action { get; set; }
        public short TileX { get; set; }
        public short TileY { get; set; }
        public byte Direction { get; set; }

        public DoorToggle()
            : base((byte)PacketTypes.DoorToggle)
        {
        }

        public DoorToggle(BinaryReader br)
            : base(br)
        {
            Action = (DoorToggleAction)br.ReadByte();
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            Direction = br.ReadByte();
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
                bw.Write(Direction);
            }
        }

        public override string ToString()
        {
            return $"[DoorToggle: Action = {Action}, TileX = {TileX}, TileY = {TileY}, Direction = {Direction}]";
        }
    }
}
