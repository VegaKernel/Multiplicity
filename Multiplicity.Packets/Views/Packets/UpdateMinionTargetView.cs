using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdateMinionTargetView
    {
        private readonly PacketView _packet;
        private readonly UpdateMinionTarget _packetObject;

        public UpdateMinionTargetView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.UpdateMinionTarget, payload))
        {
        }

        public static UpdateMinionTargetView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new UpdateMinionTargetView(payload);
        }

        public UpdateMinionTargetView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<UpdateMinionTarget>(packet, PacketTypes.UpdateMinionTarget);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public UpdateMinionTarget Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public float TargetX => _packetObject.TargetX;

        public float TargetY => _packetObject.TargetY;
    }
}


