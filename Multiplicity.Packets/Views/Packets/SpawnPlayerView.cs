using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SpawnPlayerView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public SpawnPlayerView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SpawnPlayer);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short spawnX = reader.ReadInt16();
            short spawnY = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            SpawnX = spawnX;
            SpawnY = spawnY;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public short SpawnX { get; }

        public short SpawnY { get; }
    }
}
