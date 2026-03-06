using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileEntityHatRackItemSyncView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public TileEntityHatRackItemSyncView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.TileEntityHatRackItemSync);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                int tileEntityId = reader.ReadInt32();
                byte encodedSlot = reader.ReadByte();
                ushort itemType = reader.ReadUInt16();
                ushort stack = reader.ReadUInt16();
                byte prefix = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                TileEntityId = tileEntityId;
                EncodedSlot = encodedSlot;
                ItemType = itemType;
                Stack = stack;
                Prefix = prefix;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public int TileEntityId { get; }
            public byte EncodedSlot { get; }
            public ushort ItemType { get; }
            public ushort Stack { get; }
            public byte Prefix { get; }
        }
}

