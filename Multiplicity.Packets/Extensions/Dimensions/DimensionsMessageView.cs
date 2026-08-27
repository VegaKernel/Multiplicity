using System;
using System.Net;

namespace Multiplicity.Packets.Extensions.Dimensions
{
    public readonly ref struct DimensionsMessageView
    {
        private readonly PacketView _packet;
        private readonly DimensionsMessage _packetObject;

        public DimensionsMessageView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.Placeholder, payload))
        {
        }

        public DimensionsMessageView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.Placeholder);
            _packet = packet;
            _packetObject = (DimensionsMessage)new PacketDecoder(PacketExtensions.Dimensions)
                .DecodePayload(DimensionsMessage.PacketId, packet.PayloadSpan.ToArray());
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public DimensionsMessage Packet => _packetObject;

        public short MessageType => _packetObject.MessageType;

        public string Payload => _packetObject.Payload;

        public bool IsRealIp => _packetObject.IsRealIp;

        public string ReportedRealIp => _packetObject.ReportedRealIp;

        public bool TryGetReportedRealIp(out IPAddress address) => _packetObject.TryGetReportedRealIp(out address);
    }
}
