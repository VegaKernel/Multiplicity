using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ZonesView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public ZonesView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.Zones, payload))
        {
        }

        public static ZonesView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ZonesView(payload);
        }

        public ZonesView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.Zones);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte zone1 = reader.ReadByte();
            byte zone2 = reader.ReadByte();
            byte zone3 = reader.ReadByte();
            byte zone4 = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            Zone1 = zone1;
            Zone2 = zone2;
            Zone3 = zone3;
            Zone4 = zone4;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public byte Zone1 { get; }

        public byte Zone2 { get; }

        public byte Zone3 { get; }

        public byte Zone4 { get; }
    }
}



