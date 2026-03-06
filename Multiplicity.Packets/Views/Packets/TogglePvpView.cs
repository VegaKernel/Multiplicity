using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TogglePvpView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public TogglePvpView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TogglePVP);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            bool pvpEnabled = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            PvpEnabled = pvpEnabled;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public bool PvpEnabled { get; }
    }
}
