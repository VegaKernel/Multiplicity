using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct EffectManaView
    {
        private readonly PacketView _packet;
        private readonly EffectMana _packetObject;

        public EffectManaView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.EffectMana, payload))
        {
        }

        public static EffectManaView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new EffectManaView(payload);
        }

        public EffectManaView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<EffectMana>(packet, PacketTypes.EffectMana);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public EffectMana Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public short ManaAmount => _packetObject.ManaAmount;
    }
}


