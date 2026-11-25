using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TweakItemView
    {
        private readonly PacketView _packet;
        private readonly TweakItem _packetObject;

        public TweakItemView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TweakItem, payload))
        {
        }

        public static TweakItemView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TweakItemView(payload);
        }

        public TweakItemView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TweakItem>(packet, PacketTypes.TweakItem);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TweakItem Packet => _packetObject;

        public short ItemIndex => _packetObject.ItemIndex;

        public TweakItemFlags1 Flags1 => _packetObject.Flags1;

        public uint PackedColorValue => _packetObject.PackedColorValue;

        public ushort Damage => _packetObject.Damage;

        public float Knockback => _packetObject.Knockback;

        public ushort UseAnimation => _packetObject.UseAnimation;

        public ushort UseTime => _packetObject.UseTime;

        public short Shoot => _packetObject.Shoot;

        public float ShootSpeed => _packetObject.ShootSpeed;

        public TweakItemFlags2 Flags2 => _packetObject.Flags2;

        public float Scale => _packetObject.Scale;

        public short Ammo => _packetObject.Ammo;

        public short UseAmmo => _packetObject.UseAmmo;

        public bool NotAmmo => _packetObject.NotAmmo;
    }
}


