using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct StatusView
    {
        private readonly PacketView _packet;
        private readonly Status _packetObject;

        public StatusView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.Status, payload))
        {
        }

        public static StatusView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new StatusView(payload);
        }

        public StatusView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<Status>(packet, PacketTypes.Status);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public Status Packet => _packetObject;

        public int StatusMax => _packetObject.StatusMax;

        public NetworkText StatusText => _packetObject.StatusText;

        public StatusSpecialFlags SpecialFlags => _packetObject.SpecialFlags;
    }
}


