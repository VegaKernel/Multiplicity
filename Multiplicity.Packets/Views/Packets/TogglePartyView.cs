using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TogglePartyView
    {
        private readonly PacketView _packet;
        private readonly ToggleParty _packetObject;

        public TogglePartyView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ToggleParty, payload))
        {
        }

        public static TogglePartyView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TogglePartyView(payload);
        }

        public TogglePartyView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ToggleParty>(packet, PacketTypes.ToggleParty);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ToggleParty Packet => _packetObject;
    }
}


