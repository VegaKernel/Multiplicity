using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PoofOfSmokeView
    {
        private readonly PacketView _packet;
        private readonly PoofOfSmoke _packetObject;

        public PoofOfSmokeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PoofOfSmoke, payload))
        {
        }

        public static PoofOfSmokeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PoofOfSmokeView(payload);
        }

        public PoofOfSmokeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PoofOfSmoke>(packet, PacketTypes.PoofOfSmoke);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PoofOfSmoke Packet => _packetObject;

        public int PackedVector => _packetObject.PackedVector;
    }
}


