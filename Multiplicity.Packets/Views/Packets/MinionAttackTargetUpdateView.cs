using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct MinionAttackTargetUpdateView
    {
        private readonly PacketView _packet;
        private readonly MinionAttackTargetUpdate _packetObject;

        public MinionAttackTargetUpdateView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.MinionAttackTargetUpdate, payload))
        {
        }

        public static MinionAttackTargetUpdateView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new MinionAttackTargetUpdateView(payload);
        }

        public MinionAttackTargetUpdateView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<MinionAttackTargetUpdate>(packet, PacketTypes.MinionAttackTargetUpdate);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public MinionAttackTargetUpdate Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public short MinionAttackTarget => _packetObject.MinionAttackTarget;
    }
}


