using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NumberOfAnglerQuestsCompletedView
    {
        private readonly PacketView _packet;
        private readonly NumberOfAnglerQuestsCompleted _packetObject;

        public NumberOfAnglerQuestsCompletedView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NumberOfAnglerQuestsCompleted, payload))
        {
        }

        public static NumberOfAnglerQuestsCompletedView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NumberOfAnglerQuestsCompletedView(payload);
        }

        public NumberOfAnglerQuestsCompletedView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NumberOfAnglerQuestsCompleted>(packet, PacketTypes.NumberOfAnglerQuestsCompleted);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NumberOfAnglerQuestsCompleted Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public int AnglerQuestsCompleted => _packetObject.AnglerQuestsCompleted;
    }
}


