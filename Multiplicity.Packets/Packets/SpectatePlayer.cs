using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SpectatePlayer : TerrariaPacket
        {
            private const short PayloadLength = 3;
    
            public byte PlayerId { get; set; }
    
            public short TargetPlayerId { get; set; }
    
            public SpectatePlayer()
                : base((byte)PacketTypes.SpectatePlayer)
            {
            }
    
            public SpectatePlayer(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                TargetPlayerId = br.ReadInt16();
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
                    bw.Write(TargetPlayerId);
                }
            }
        }
}

