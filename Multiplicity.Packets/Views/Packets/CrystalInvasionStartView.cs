using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CrystalInvasionStartView
    {
        private readonly PacketView _packet;
        private readonly CrystalInvasionStart _packetObject;

        public CrystalInvasionStartView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CrystalInvasionStart, payload))
        {
        }

        public static CrystalInvasionStartView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CrystalInvasionStartView(payload);
        }

        public CrystalInvasionStartView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CrystalInvasionStart>(packet, PacketTypes.CrystalInvasionStart);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CrystalInvasionStart Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;
    }
}


