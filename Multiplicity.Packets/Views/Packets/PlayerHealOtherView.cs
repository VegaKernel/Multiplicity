using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerHealOtherView
    {
        private readonly PacketView _packet;
        private readonly PlayerHealOther _packetObject;

        public PlayerHealOtherView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerHealOther, payload))
        {
        }

        public static PlayerHealOtherView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerHealOtherView(payload);
        }

        public PlayerHealOtherView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerHealOther>(packet, PacketTypes.PlayerHealOther);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerHealOther Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public short HealAmount => _packetObject.HealAmount;
    }
}


