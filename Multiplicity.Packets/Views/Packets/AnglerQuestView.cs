using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct AnglerQuestView
    {
        private readonly PacketView _packet;
        private readonly AnglerQuest _packetObject;

        public AnglerQuestView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.AnglerQuest, payload))
        {
        }

        public static AnglerQuestView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new AnglerQuestView(payload);
        }

        public AnglerQuestView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<AnglerQuest>(packet, PacketTypes.AnglerQuest);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public AnglerQuest Packet => _packetObject;

        public byte Quest => _packetObject.Quest;

        public bool Completed => _packetObject.Completed;
    }
}


