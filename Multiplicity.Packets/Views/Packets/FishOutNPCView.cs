using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct FishOutNPCView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public FishOutNPCView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.FishOutNPC);
                PacketReader reader = packet.CreatePayloadReader();
                ushort tileX = reader.ReadUInt16();
                ushort tileY = reader.ReadUInt16();
                short npcNetId = reader.ReadInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                TileX = tileX;
                TileY = tileY;
                NpcNetId = npcNetId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public ushort TileX { get; }
            public ushort TileY { get; }
            public short NpcNetId { get; }
        }
}

