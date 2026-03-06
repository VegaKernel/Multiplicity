using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncCavernMonsterType : TerrariaPacket
        {
            private const int VariantRows = 2;
            private const int VariantColumns = 3;
            private const short PayloadLength = VariantRows * VariantColumns * sizeof(ushort);
    
            public ushort[,] MonsterTypes { get; } = new ushort[VariantRows, VariantColumns];
    
            public SyncCavernMonsterType()
                : base((byte)PacketTypes.SyncCavernMonsterType)
            {
            }
    
            public SyncCavernMonsterType(BinaryReader br)
                : base(br)
            {
                for (int row = 0; row < VariantRows; row++)
                {
                    for (int column = 0; column < VariantColumns; column++)
                    {
                        MonsterTypes[row, column] = br.ReadUInt16();
                    }
                }
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
                    for (int row = 0; row < VariantRows; row++)
                    {
                        for (int column = 0; column < VariantColumns; column++)
                        {
                            bw.Write(MonsterTypes[row, column]);
                        }
                    }
                }
            }
        }
}

