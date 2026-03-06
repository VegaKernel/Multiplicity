using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcTalkView
    {
        private readonly PacketView _packet;
        private readonly NpcTalk _packetObject;

        public NpcTalkView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcTalk, payload))
        {
        }

        public static NpcTalkView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcTalkView(payload);
        }

        public NpcTalkView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcTalk>(packet, PacketTypes.NpcTalk);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcTalk Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public short NpcTalkTarget => _packetObject.NpcTalkTarget;
    }
}


