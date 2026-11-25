using System.IO;
using System.Text;

namespace Multiplicity.Packets
{
    public class CustomServerAck : TerrariaPacket
    {
        public int Value { get; set; }

        public CustomServerAck() : base((byte)PacketTypes.CustomServerAck) { }

        public CustomServerAck(BinaryReader reader) : base(reader)
        {
            Value = reader.Read7BitEncodedInt();
        }

        public override short GetLength() => (short)PacketSerializationHelper.Get7BitEncodedIntLength(Value);

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write7BitEncodedInt(Value);
        }
    }
}
