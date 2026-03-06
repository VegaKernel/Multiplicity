using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerDodgeView
    {
        private readonly PacketView _packet;
        private readonly PlayerDodge _packetObject;

        public PlayerDodgeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerDodge, payload))
        {
        }

        public static PlayerDodgeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerDodgeView(payload);
        }

        public PlayerDodgeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerDodge>(packet, PacketTypes.PlayerDodge);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerDodge Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public byte Flag => _packetObject.Flag;
    }
}


