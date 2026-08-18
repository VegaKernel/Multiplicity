using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct KillPortalView
    {
        private readonly PacketView _packet;
        private readonly KillPortal _packetObject;

        public KillPortalView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.KillPortal, payload))
        {
        }

        public static KillPortalView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new KillPortalView(payload);
        }

        public KillPortalView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<KillPortal>(packet, PacketTypes.KillPortal);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public KillPortal Packet => _packetObject;

        public ushort PlayerId => _packetObject.PlayerId;

        public byte PortalColor => _packetObject.PortalColor;

        [Obsolete("Packet 95 contains the portal owner player id, not a projectile index. Use PlayerId instead.")]
        public ushort ProjectileIndex => PlayerId;
    }
}
