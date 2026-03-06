using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CrystalInvasionWipeAllView
    {
        private readonly PacketView _packet;
        private readonly CrystalInvasionWipeAll _packetObject;

        public CrystalInvasionWipeAllView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CrystalInvasionWipeAll, payload))
        {
        }

        public static CrystalInvasionWipeAllView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CrystalInvasionWipeAllView(payload);
        }

        public CrystalInvasionWipeAllView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CrystalInvasionWipeAll>(packet, PacketTypes.CrystalInvasionWipeAll);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CrystalInvasionWipeAll Packet => _packetObject;
    }
}


