using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerTeamView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerTeamView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerTeam);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte team = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            Team = team;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public byte Team { get; }
    }
}
