using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcStrikeView
    {
        private readonly PacketView _packet;
        private readonly NpcStrike _packetObject;

        public NpcStrikeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcStrike, payload))
        {
        }

        public static NpcStrikeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcStrikeView(payload);
        }

        public NpcStrikeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcStrike>(packet, PacketTypes.NpcStrike);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcStrike Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;

        public short Damage => _packetObject.Damage;

        public float Knockback => _packetObject.Knockback;

        public byte Direction => _packetObject.Direction;

        public bool Crit => _packetObject.Crit;
    }
}


