using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class RequestLucyPopup : TerrariaPacket
        {
            private const short PayloadLength = 18;
    
            public LucyPopupMessageSource MessageSource { get; set; }
    
            public byte Variation { get; set; }
    
            public float VelocityX { get; set; }
    
            public float VelocityY { get; set; }
    
            public int PositionX { get; set; }
    
            public int PositionY { get; set; }
    
            public RequestLucyPopup()
                : base((byte)PacketTypes.RequestLucyPopup)
            {
            }
    
            public RequestLucyPopup(BinaryReader br)
                : base(br)
            {
                MessageSource = (LucyPopupMessageSource)br.ReadByte();
                Variation = br.ReadByte();
                VelocityX = br.ReadSingle();
                VelocityY = br.ReadSingle();
                PositionX = br.ReadInt32();
                PositionY = br.ReadInt32();
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
                    bw.Write((byte)MessageSource);
                    bw.Write(Variation);
                    bw.Write(VelocityX);
                    bw.Write(VelocityY);
                    bw.Write(PositionX);
                    bw.Write(PositionY);
                }
            }
        }
}

