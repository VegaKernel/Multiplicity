using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct DisconnectView
    {
        private readonly PacketView _packet;
        private readonly Disconnect _packetObject;

        public DisconnectView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.Disconnect, payload))
        {
        }

        public static DisconnectView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new DisconnectView(payload);
        }

        public DisconnectView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<Disconnect>(packet, PacketTypes.Disconnect);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public Disconnect Packet => _packetObject;

        public NetworkText Reason => _packetObject.Reason;
    }
}


