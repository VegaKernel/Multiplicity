using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SetMiscEventValuesView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SetMiscEventValuesView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SetMiscEventValues);
                PacketReader reader = packet.CreatePayloadReader();
                MiscEventValueType eventType = (MiscEventValueType)reader.ReadByte();
                int value = reader.ReadInt32();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                EventType = eventType;
                Value = value;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public MiscEventValueType EventType { get; }
            public int Value { get; }
        }
}

