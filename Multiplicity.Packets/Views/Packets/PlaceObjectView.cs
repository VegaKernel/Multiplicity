using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlaceObjectView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlaceObjectView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlaceObject);

            PacketReader reader = packet.CreatePayloadReader();
            short x = reader.ReadInt16();
            short y = reader.ReadInt16();
            short type = reader.ReadInt16();
            short style = reader.ReadInt16();
            byte alternate = reader.ReadByte();
            sbyte random = reader.ReadSByte();
            bool direction = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            X = x;
            Y = y;
            Type = type;
            Style = style;
            Alternate = alternate;
            Random = random;
            Direction = direction;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public short X { get; }

        public short Y { get; }

        public short Type { get; }

        public short Style { get; }

        public byte Alternate { get; }

        public sbyte Random { get; }

        public bool Direction { get; }
    }
}
