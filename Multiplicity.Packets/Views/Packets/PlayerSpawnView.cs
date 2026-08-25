using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerSpawnView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerSpawnView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerSpawn, payload))
        {
        }

        public static PlayerSpawnView FromPayload(ReadOnlySpan<byte> payload) => new PlayerSpawnView(payload);

        public PlayerSpawnView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerSpawn);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short spawnX = reader.ReadInt16();
            short spawnY = reader.ReadInt16();
            int respawnTimer = reader.ReadInt32();
            short numberOfDeathsPVE = reader.ReadInt16();
            short numberOfDeathsPVP = reader.ReadInt16();
            byte team = reader.ReadByte();
            byte spawnContext = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            SpawnX = spawnX;
            SpawnY = spawnY;
            RespawnTimer = respawnTimer;
            NumberOfDeathsPVE = numberOfDeathsPVE;
            NumberOfDeathsPVP = numberOfDeathsPVP;
            Team = team;
            SpawnContext = spawnContext;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public short SpawnX { get; }

        public short SpawnY { get; }

        public int RespawnTimer { get; }

        public short NumberOfDeathsPVE { get; }

        public short NumberOfDeathsPVP { get; }

        public byte Team { get; }

        public byte SpawnContext { get; }
    }
}
