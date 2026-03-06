using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ItemOwnerView
    {
        private readonly PacketView _packet;
        private readonly ItemOwner _packetObject;

        public ItemOwnerView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ItemOwner, payload))
        {
        }

        public static ItemOwnerView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ItemOwnerView(payload);
        }

        public ItemOwnerView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ItemOwner>(packet, PacketTypes.ItemOwner);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ItemOwner Packet => _packetObject;

        public short ItemId => _packetObject.ItemId;

        public byte PlayerId => _packetObject.PlayerId;
    }
}


