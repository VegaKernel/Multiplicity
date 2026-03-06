using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcShopItemView
    {
        private readonly PacketView _packet;
        private readonly NpcShopItem _packetObject;

        public NpcShopItemView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcShopItem, payload))
        {
        }

        public static NpcShopItemView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcShopItemView(payload);
        }

        public NpcShopItemView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcShopItem>(packet, PacketTypes.NpcShopItem);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcShopItem Packet => _packetObject;

        public byte Slot => _packetObject.Slot;

        public short ItemType => _packetObject.ItemType;

        public short Stack => _packetObject.Stack;

        public byte Prefix => _packetObject.Prefix;

        public int Value => _packetObject.Value;

        public NpcShopItemFlags Flags => _packetObject.Flags;
    }
}


