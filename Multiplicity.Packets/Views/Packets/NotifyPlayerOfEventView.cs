using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NotifyPlayerOfEventView
    {
        private readonly PacketView _packet;
        private readonly NotifyPlayerOfEvent _packetObject;

        public NotifyPlayerOfEventView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NotifyPlayerOfEvent, payload))
        {
        }

        public static NotifyPlayerOfEventView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NotifyPlayerOfEventView(payload);
        }

        public NotifyPlayerOfEventView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NotifyPlayerOfEvent>(packet, PacketTypes.NotifyPlayerOfEvent);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NotifyPlayerOfEvent Packet => _packetObject;

        public short EventId => _packetObject.EventId;
    }
}


