using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PingView
    {
        private readonly PacketView _packet;
        private readonly Ping _packetObject;

        public PingView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.Ping, payload))
        {
        }

        public static PingView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PingView(payload);
        }

        public PingView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<Ping>(packet, PacketTypes.Ping);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public Ping Packet => _packetObject;
    }
}


