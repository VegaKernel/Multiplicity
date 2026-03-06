using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SignReadView
    {
        private readonly PacketView _packet;
        private readonly SignRead _packetObject;

        public SignReadView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SignRead, payload))
        {
        }

        public static SignReadView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SignReadView(payload);
        }

        public SignReadView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SignRead>(packet, PacketTypes.SignRead);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SignRead Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;
    }
}


