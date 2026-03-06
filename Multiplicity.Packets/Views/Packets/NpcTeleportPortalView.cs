using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcTeleportPortalView
    {
        private readonly PacketView _packet;
        private readonly NpcTeleportPortal _packetObject;

        public NpcTeleportPortalView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcTeleportPortal, payload))
        {
        }

        public static NpcTeleportPortalView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcTeleportPortalView(payload);
        }

        public NpcTeleportPortalView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcTeleportPortal>(packet, PacketTypes.NpcTeleportPortal);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcTeleportPortal Packet => _packetObject;

        public ushort NpcId => _packetObject.NpcId;

        public short PortalColorIndex => _packetObject.PortalColorIndex;

        public float NewPositionX => _packetObject.NewPositionX;

        public float NewPositionY => _packetObject.NewPositionY;

        public float VelocityX => _packetObject.VelocityX;

        public float VelocityY => _packetObject.VelocityY;
    }
}


