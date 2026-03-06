using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class ShimmerActions : TerrariaPacket
        {
            private const short BasePayloadLength = 9;
            private const short CoinLuckPayloadLength = 13;
    
            public ShimmerActionKind Action { get; set; }
    
            public float PositionX { get; set; }
    
            public float PositionY { get; set; }
    
            public int CoinLuckAmount { get; set; }
    
            public ShimmerActions()
                : base((byte)PacketTypes.ShimmerActions)
            {
            }
    
            public ShimmerActions(BinaryReader br)
                : base(br)
            {
                Action = (ShimmerActionKind)br.ReadByte();
                PositionX = br.ReadSingle();
                PositionY = br.ReadSingle();
    
                if (Action == ShimmerActionKind.AddCoinLuck)
                {
                    CoinLuckAmount = br.ReadInt32();
                }
            }
    
            public override short GetLength()
            {
                return Action == ShimmerActionKind.AddCoinLuck ? CoinLuckPayloadLength : BasePayloadLength;
            }
    
            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }
    
                using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                {
                    bw.Write((byte)Action);
                    bw.Write(PositionX);
                    bw.Write(PositionY);
    
                    if (Action == ShimmerActionKind.AddCoinLuck)
                    {
                        bw.Write(CoinLuckAmount);
                    }
                }
            }
        }
}

