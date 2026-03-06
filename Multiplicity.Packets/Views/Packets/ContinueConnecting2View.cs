using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ContinueConnecting2View
    {
        private readonly PacketView _packet;
        private readonly ContinueConnecting2 _packetObject;

        public ContinueConnecting2View(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ContinueConnecting2, payload))
        {
        }

        public static ContinueConnecting2View FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ContinueConnecting2View(payload);
        }

        public ContinueConnecting2View(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ContinueConnecting2>(packet, PacketTypes.ContinueConnecting2);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ContinueConnecting2 Packet => _packetObject;
    }
}


