using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerZoneView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerZoneView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerZone);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte zone1 = reader.ReadByte();
            byte zone2 = reader.ReadByte();
            byte zone3 = reader.ReadByte();
            byte zone4 = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            Zone1 = zone1;
            Zone2 = zone2;
            Zone3 = zone3;
            Zone4 = zone4;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public byte Zone1 { get; }

        public byte Zone2 { get; }

        public byte Zone3 { get; }

        public byte Zone4 { get; }
    }
}
