using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileEntityItemSyncPacketView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public TileEntityItemSyncPacketView(PacketView packet, PacketTypes expectedPacketType)
            {
                packet.EnsureType(expectedPacketType);
                PacketReader reader = packet.CreatePayloadReader();
                short tileX = reader.ReadInt16();
                short tileY = reader.ReadInt16();
                short itemType = reader.ReadInt16();
                byte prefix = reader.ReadByte();
                short stack = reader.ReadInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PacketType = expectedPacketType;
                TileX = tileX;
                TileY = tileY;
                ItemType = itemType;
                Prefix = prefix;
                Stack = stack;
            }
    
            public PacketHeader Header { get; }
            public PacketTypes PacketType { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public short TileX { get; }
            public short TileY { get; }
            public short ItemType { get; }
            public byte Prefix { get; }
            public short Stack { get; }
        }
}

