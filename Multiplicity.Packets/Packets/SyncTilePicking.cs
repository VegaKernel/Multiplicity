using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncTilePicking : TerrariaPacket
        {
            private const short PayloadLength = 6;
    
            public byte PlayerId { get; set; }
    
            public short TileX { get; set; }
    
            public short TileY { get; set; }
    
            public byte PickPower { get; set; }
    
            public SyncTilePicking()
                : base((byte)PacketTypes.SyncTilePicking)
            {
            }
    
            public SyncTilePicking(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                TileX = br.ReadInt16();
                TileY = br.ReadInt16();
                PickPower = br.ReadByte();
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
                    bw.Write(TileX);
                    bw.Write(TileY);
                    bw.Write(PickPower);
                }
            }
        }
}

