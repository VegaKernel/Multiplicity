using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TravellingMerchantInventoryView
    {
        private readonly PacketView _packet;
        private readonly TravellingMerchantInventory _packetObject;

        public TravellingMerchantInventoryView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TravellingMerchantInventory, payload))
        {
        }

        public static TravellingMerchantInventoryView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TravellingMerchantInventoryView(payload);
        }

        public TravellingMerchantInventoryView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TravellingMerchantInventory>(packet, PacketTypes.TravellingMerchantInventory);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TravellingMerchantInventory Packet => _packetObject;

        public short[] Items => _packetObject.Items;
    }
}


