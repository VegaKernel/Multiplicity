using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct WiredCannonShotView
    {
        private readonly PacketView _packet;
        private readonly WiredCannonShot _packetObject;

        public WiredCannonShotView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.WiredCannonShot, payload))
        {
        }

        public static WiredCannonShotView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new WiredCannonShotView(payload);
        }

        public WiredCannonShotView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<WiredCannonShot>(packet, PacketTypes.WiredCannonShot);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public WiredCannonShot Packet => _packetObject;

        public short Damage => _packetObject.Damage;

        public float Knockback => _packetObject.Knockback;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public short Angle => _packetObject.Angle;

        public short Ammo => _packetObject.Ammo;

        public byte PlayerId => _packetObject.PlayerId;
    }
}


