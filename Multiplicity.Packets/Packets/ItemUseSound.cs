using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class ItemUseSound : TerrariaPacket
        {
            private const short PayloadLength = 1;
    
            public byte PlayerId { get; set; }
    
            public ItemUseSound()
                : base((byte)PacketTypes.ItemUseSound)
            {
            }
    
            public ItemUseSound(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
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
                }
            }
        }
}

