using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcSpecialView
    {
        private readonly PacketView _packet;
        private readonly NpcSpecial _packetObject;

        public NpcSpecialView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcSpecial, payload))
        {
        }

        public static NpcSpecialView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcSpecialView(payload);
        }

        public NpcSpecialView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcSpecial>(packet, PacketTypes.NpcSpecial);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcSpecial Packet => _packetObject;

        public byte SubjectId => _packetObject.SubjectId;

        public NpcSpecialType EffectType => _packetObject.EffectType;
    }
}


