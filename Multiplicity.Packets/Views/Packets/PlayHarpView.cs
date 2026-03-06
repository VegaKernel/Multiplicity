using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayHarpView
    {
        private readonly PacketView _packet;
        private readonly PlayHarp _packetObject;

        public PlayHarpView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayHarp, payload))
        {
        }

        public static PlayHarpView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayHarpView(payload);
        }

        public PlayHarpView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayHarp>(packet, PacketTypes.PlayHarp);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayHarp Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public float Note => _packetObject.Note;
    }
}


