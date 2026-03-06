using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TogglePvpView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TogglePvpView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.TogglePvp, payload))
        {
        }

        public static TogglePvpView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TogglePvpView(payload);
        }

        public TogglePvpView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TogglePvp);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            bool pvpEnabled = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            PvpEnabled = pvpEnabled;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public bool PvpEnabled { get; }
    }
}




