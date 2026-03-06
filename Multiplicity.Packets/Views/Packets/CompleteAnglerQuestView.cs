using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CompleteAnglerQuestView
    {
        private readonly PacketView _packet;
        private readonly CompleteAnglerQuest _packetObject;

        public CompleteAnglerQuestView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CompleteAnglerQuest, payload))
        {
        }

        public static CompleteAnglerQuestView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CompleteAnglerQuestView(payload);
        }

        public CompleteAnglerQuestView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CompleteAnglerQuest>(packet, PacketTypes.CompleteAnglerQuest);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CompleteAnglerQuest Packet => _packetObject;
    }
}


