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

        public static PlayerSpawnView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerSpawnView(payload);
        }

        public PlayerSpawnView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerSpawn);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short spawnX = reader.ReadInt16();
            short spawnY = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            SpawnX = spawnX;
            SpawnY = spawnY;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public short SpawnX { get; }

        public short SpawnY { get; }
    }
}



