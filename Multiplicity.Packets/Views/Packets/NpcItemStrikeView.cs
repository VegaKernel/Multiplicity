using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcItemStrikeView
    {
        private readonly PacketView _packet;
        private readonly NpcItemStrike _packetObject;

        public NpcItemStrikeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcItemStrike, payload))
        {
        }

        public static NpcItemStrikeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcItemStrikeView(payload);
        }

        public NpcItemStrikeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcItemStrike>(packet, PacketTypes.NpcItemStrike);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcItemStrike Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;

        public byte PlayerId => _packetObject.PlayerId;
    }
}


