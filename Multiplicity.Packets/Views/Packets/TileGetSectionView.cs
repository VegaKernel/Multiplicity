using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileGetSectionView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TileGetSectionView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TileGetSection, payload))
        {
        }

        public static TileGetSectionView FromPayload(ReadOnlySpan<byte> payload) => new TileGetSectionView(payload);

        public TileGetSectionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TileGetSection);

            PacketReader reader = packet.CreatePayloadReader();
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            byte team = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            X = x;
            Y = y;
            Team = team;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public int X { get; }

        public int Y { get; }

        public byte Team { get; }
    }
}
