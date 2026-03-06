using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerActiveView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerActiveView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerActive, payload))
        {
        }

        public static PlayerActiveView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerActiveView(payload);
        }

        public PlayerActiveView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerActive);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            bool active = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            Active = active;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public bool Active { get; }
    }
}



