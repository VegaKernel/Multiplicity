using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RequestNPCBuffRemovalView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public RequestNPCBuffRemovalView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.RequestNPCBuffRemoval);
                PacketReader reader = packet.CreatePayloadReader();
                short npcId = reader.ReadInt16();
                ushort buffId = reader.ReadUInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                NpcId = npcId;
                BuffId = buffId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public short NpcId { get; }
            public ushort BuffId { get; }
        }
}

