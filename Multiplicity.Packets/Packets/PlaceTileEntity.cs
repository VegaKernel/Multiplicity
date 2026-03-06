using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlaceTileEntity (0x57) packet.
    /// </summary>
    public class PlaceTileEntity : TerrariaPacket
    {
        private const short PayloadLength = 5;

        public short X { get; set; }
        public short Y { get; set; }
        public PlaceTileEntityType EntityType { get; set; }

        public PlaceTileEntity()
            : base((byte)PacketTypes.PlaceTileEntity)
        {
        }

        public PlaceTileEntity(BinaryReader br)
            : base(br)
        {
            X = br.ReadInt16();
            Y = br.ReadInt16();
            EntityType = (PlaceTileEntityType)br.ReadByte();
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
                bw.Write(X);
                bw.Write(Y);
                bw.Write((byte)EntityType);
            }
        }

        public override string ToString()
        {
            return $"[PlaceTileEntity: X = {X}, Y = {Y}, EntityType = {EntityType}]";
        }
    }
}
