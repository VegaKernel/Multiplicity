using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PasswordSendView
    {
        private readonly PacketView _packet;
        private readonly PasswordSend _packetObject;

        public PasswordSendView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PasswordSend, payload))
        {
        }

        public static PasswordSendView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PasswordSendView(payload);
        }

        public PasswordSendView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PasswordSend>(packet, PacketTypes.PasswordSend);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PasswordSend Packet => _packetObject;

        public string Password => _packetObject.Password;
    }
}


