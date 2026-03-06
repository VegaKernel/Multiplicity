using System.IO;

namespace Multiplicity.Packets
{
    public sealed class PlaceObject : TerrariaPacket
    {
        private const short PayloadLength = 11;

        public short X { get; set; }

        public short Y { get; set; }

        public short Type { get; set; }

        public short Style { get; set; }

        public byte Alternate { get; set; }

        public sbyte Random { get; set; }

        public bool Direction { get; set; }

        public PlaceObject()
            : base((byte)PacketTypes.PlaceObject)
        {
        }

        public PlaceObject(BinaryReader br)
            : base(br)
        {
            X = br.ReadInt16();
            Y = br.ReadInt16();
            Type = br.ReadInt16();
            Style = br.ReadInt16();
            Alternate = br.ReadByte();
            Random = br.ReadSByte();
            Direction = br.ReadBoolean();
        }

        public override string ToString()
        {
            return $"[PlaceObject: X = {X} Y = {Y} Type = {Type} Style = {Style} Alternate = {Alternate} Random = {Random} Direction = {Direction}]";
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
                bw.Write(Type);
                bw.Write(Style);
                bw.Write(Alternate);
                bw.Write(Random);
                bw.Write(Direction);
            }
        }
    }
}
