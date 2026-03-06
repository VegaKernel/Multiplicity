using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncLoadoutView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SyncLoadoutView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncLoadout);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                byte loadoutIndex = reader.ReadByte();
                ushort hiddenAccessoryVisibilityMask = reader.ReadUInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                LoadoutIndex = loadoutIndex;
                HiddenAccessoryVisibilityMask = hiddenAccessoryVisibilityMask;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public byte LoadoutIndex { get; }
            public ushort HiddenAccessoryVisibilityMask { get; }
        }
}

