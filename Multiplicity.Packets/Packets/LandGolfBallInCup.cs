using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class LandGolfBallInCup : TerrariaPacket
        {
            private const short PayloadLength = 9;
    
            public byte PlayerId { get; set; }
    
            public ushort CupX { get; set; }
    
            public ushort CupY { get; set; }
    
            public ushort ShotX { get; set; }
    
            public ushort ShotY { get; set; }
    
            public LandGolfBallInCup()
                : base((byte)PacketTypes.LandGolfBallInCup)
            {
            }
    
            public LandGolfBallInCup(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                CupX = br.ReadUInt16();
                CupY = br.ReadUInt16();
                ShotX = br.ReadUInt16();
                ShotY = br.ReadUInt16();
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
                    bw.Write(PlayerId);
                    bw.Write(CupX);
                    bw.Write(CupY);
                    bw.Write(ShotX);
                    bw.Write(ShotY);
                }
            }
        }
}

