using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct EffectHealView
    {
        private readonly PacketView _packet;
        private readonly EffectHeal _packetObject;

        public EffectHealView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.EffectHeal, payload))
        {
        }

        public static EffectHealView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new EffectHealView(payload);
        }

        public EffectHealView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<EffectHeal>(packet, PacketTypes.EffectHeal);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public EffectHeal Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public short HealAmount => _packetObject.HealAmount;
    }
}


