using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SetCountsAsHostForGameplay : TerrariaPacket
        {
            private const short PayloadLength = 2;
    
            public byte PlayerId { get; set; }
    
            public bool CountsAsHostForGameplay { get; set; }
    
            public SetCountsAsHostForGameplay()
                : base((byte)PacketTypes.SetCountsAsHostForGameplay)
            {
            }
    
            public SetCountsAsHostForGameplay(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                CountsAsHostForGameplay = br.ReadBoolean();
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
                    bw.Write(CountsAsHostForGameplay);
                }
            }
        }
}

