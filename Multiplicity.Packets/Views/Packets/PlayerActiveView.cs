using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerActiveView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerActiveView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerActive);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            bool active = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            Active = active;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public bool Active { get; }
    }
}
