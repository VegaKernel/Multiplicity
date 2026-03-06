using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ContinueConnectingView
    {
        private readonly PacketView _packet;
        private readonly ContinueConnecting _packetObject;

        public ContinueConnectingView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ContinueConnecting, payload))
        {
        }

        public static ContinueConnectingView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ContinueConnectingView(payload);
        }

        public ContinueConnectingView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ContinueConnecting>(packet, PacketTypes.ContinueConnecting);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ContinueConnecting Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;
    }
}


