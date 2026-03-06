using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SetMiscEventValues : TerrariaPacket
        {
            private const short PayloadLength = 5;
    
            public MiscEventValueType EventType { get; set; }
    
            public int Value { get; set; }
    
            public SetMiscEventValues()
                : base((byte)PacketTypes.SetMiscEventValues)
            {
            }
    
            public SetMiscEventValues(BinaryReader br)
                : base(br)
            {
                EventType = (MiscEventValueType)br.ReadByte();
                Value = br.ReadInt32();
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
                    bw.Write((byte)EventType);
                    bw.Write(Value);
                }
            }
        }
}

