using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerTeleportPortalView
    {
        private readonly PacketView _packet;
        private readonly PlayerTeleportPortal _packetObject;

        public PlayerTeleportPortalView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerTeleportPortal, payload))
        {
        }

        public static PlayerTeleportPortalView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerTeleportPortalView(payload);
        }

        public PlayerTeleportPortalView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerTeleportPortal>(packet, PacketTypes.PlayerTeleportPortal);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerTeleportPortal Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public short PortalColorIndex => _packetObject.PortalColorIndex;

        public float NewPositionX => _packetObject.NewPositionX;

        public float NewPositionY => _packetObject.NewPositionY;

        public float VelocityX => _packetObject.VelocityX;

        public float VelocityY => _packetObject.VelocityY;
    }
}


