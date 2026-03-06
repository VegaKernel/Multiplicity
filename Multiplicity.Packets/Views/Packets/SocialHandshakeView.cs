using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SocialHandshakeView
    {
        private readonly PacketView _packet;
        private readonly SocialHandshake _packetObject;

        public SocialHandshakeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SocialHandshake, payload))
        {
        }

        public static SocialHandshakeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SocialHandshakeView(payload);
        }

        public SocialHandshakeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SocialHandshake>(packet, PacketTypes.SocialHandshake);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SocialHandshake Packet => _packetObject;
    }
}


