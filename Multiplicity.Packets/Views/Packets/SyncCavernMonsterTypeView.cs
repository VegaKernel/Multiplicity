using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncCavernMonsterTypeView
        {
            private const int Rows = 2;
            private const int Columns = 3;
    
            private readonly ReadOnlySpan<byte> _sourceSpan;
            private readonly ReadOnlySpan<byte> _monsterTypesPayload;
    
            public SyncCavernMonsterTypeView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncCavernMonsterType);
                PacketReader reader = packet.CreatePayloadReader();
                ReadOnlySpan<byte> monsterTypesPayload = reader.ReadSpan(Rows * Columns * sizeof(ushort));
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                _monsterTypesPayload = monsterTypesPayload;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
    
            public ushort GetMonsterType(int row, int column)
            {
                if ((uint)row >= Rows)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }
    
                if ((uint)column >= Columns)
                {
                    throw new ArgumentOutOfRangeException(nameof(column));
                }
    
                int offset = ((row * Columns) + column) * sizeof(ushort);
                return BinaryPrimitives.ReadUInt16LittleEndian(_monsterTypesPayload.Slice(offset, sizeof(ushort)));
            }
        }
}

