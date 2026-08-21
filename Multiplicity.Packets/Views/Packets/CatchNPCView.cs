using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CatchNPCView
    {
        private readonly PacketView _packet;
        private readonly CatchNPC _packetObject;

        public CatchNPCView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CatchNPC, payload))
        {
        }

        public static CatchNPCView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CatchNPCView(payload);
        }

        public CatchNPCView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CatchNPC>(packet, PacketTypes.CatchNPC);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CatchNPC Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;
    }
}
