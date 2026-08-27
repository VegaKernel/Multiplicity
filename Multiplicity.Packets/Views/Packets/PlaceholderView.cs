using System;
using System.Net;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlaceholderView
    {
        private readonly PacketView _packet;
        private readonly Placeholder _packetObject;

        public PlaceholderView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.Placeholder, payload))
        {
        }

        public static PlaceholderView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlaceholderView(payload);
        }

        public PlaceholderView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<Placeholder>(packet, PacketTypes.Placeholder);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public Placeholder Packet => _packetObject;

        public bool IsDimensionsMessage => _packetObject.IsDimensionsMessage;

        public bool IsDimensionsRealIp => _packetObject.IsDimensionsRealIp;

        public short? DimensionsMessageType => _packetObject.DimensionsMessageType;

        public string DimensionsPayload => _packetObject.DimensionsPayload;

        public string ReportedRealIp => _packetObject.ReportedRealIp;

        public bool TryGetReportedRealIp(out IPAddress address) => _packetObject.TryGetReportedRealIp(out address);
    }
}


