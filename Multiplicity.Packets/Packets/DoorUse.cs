using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The DoorUse (0x13) packet.
    /// </summary>
    public class DoorUse : TerrariaPacket
    {
        private const short PayloadLength = 6;

        public DoorUseAction Action { get; set; }
        public short TileX { get; set; }
        public short TileY { get; set; }
        public byte Direction { get; set; }

        public DoorUse()
            : base((byte)PacketTypes.DoorUse)
        {
        }

        public DoorUse(BinaryReader br)
            : base(br)
        {
            Action = (DoorUseAction)br.ReadByte();
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
            return $"[DoorUse: Action = {Action}, TileX = {TileX}, TileY = {TileY}, Direction = {Direction}]";
        }
    }
}
