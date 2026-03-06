using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RequestQuestEffectView
    {
        private readonly PacketView _packet;
        private readonly RequestQuestEffect _packetObject;

        public RequestQuestEffectView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.RequestQuestEffect, payload))
        {
        }

        public static RequestQuestEffectView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new RequestQuestEffectView(payload);
        }

        public RequestQuestEffectView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<RequestQuestEffect>(packet, PacketTypes.RequestQuestEffect);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public RequestQuestEffect Packet => _packetObject;
    }
}


