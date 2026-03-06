using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct GetSectionView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public GetSectionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.GetSection);

            PacketReader reader = packet.CreatePayloadReader();
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            X = x;
            Y = y;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public int X { get; }

        public int Y { get; }
    }
}
