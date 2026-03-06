using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct HostTokenView
    {
        private readonly PacketView _packet;
        private readonly HostToken _packetObject;

        public HostTokenView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.HostToken, payload))
        {
        }

        public static HostTokenView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new HostTokenView(payload);
        }

        public HostTokenView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<HostToken>(packet, PacketTypes.HostToken);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public HostToken Packet => _packetObject;

        public string Token => _packetObject.Token;
    }
}


