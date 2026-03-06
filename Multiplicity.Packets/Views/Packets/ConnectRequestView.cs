using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ConnectRequestView
    {
        private readonly PacketView _packet;
        private readonly ConnectRequest _packetObject;

        public ConnectRequestView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ConnectRequest, payload))
        {
        }

        public static ConnectRequestView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ConnectRequestView(payload);
        }

        public ConnectRequestView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ConnectRequest>(packet, PacketTypes.ConnectRequest);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ConnectRequest Packet => _packetObject;

        public string Version => _packetObject.Version;
    }
}


