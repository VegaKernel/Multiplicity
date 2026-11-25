using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CustomServerAckView
    {
        private readonly PacketView _packet;

        public CustomServerAckView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CustomServerAck, payload))
        {
        }

        public CustomServerAckView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.CustomServerAck);
            _packet = packet;
            PacketReader reader = packet.CreatePayloadReader();
            Value = reader.Read7BitEncodedInt();
            reader.EnsureEnd();
        }

        public static CustomServerAckView FromPayload(ReadOnlySpan<byte> payload) => new CustomServerAckView(payload);

        public PacketHeader Header => _packet.Header;
        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;
        public int Value { get; }
    }
}
