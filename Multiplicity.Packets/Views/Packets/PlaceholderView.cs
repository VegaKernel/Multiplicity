using System;
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
    }
}


