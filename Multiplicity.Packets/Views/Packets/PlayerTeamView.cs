using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerTeamView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerTeamView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerTeam, payload))
        {
        }

        public static PlayerTeamView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerTeamView(payload);
        }

        public PlayerTeamView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerTeam);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte team = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            Team = team;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public byte Team { get; }
    }
}



