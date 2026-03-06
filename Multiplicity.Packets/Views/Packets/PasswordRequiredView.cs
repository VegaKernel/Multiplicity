using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PasswordRequiredView
    {
        private readonly PacketView _packet;
        private readonly PasswordRequired _packetObject;

        public PasswordRequiredView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PasswordRequired, payload))
        {
        }

        public static PasswordRequiredView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PasswordRequiredView(payload);
        }

        public PasswordRequiredView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PasswordRequired>(packet, PacketTypes.PasswordRequired);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PasswordRequired Packet => _packetObject;
    }
}


