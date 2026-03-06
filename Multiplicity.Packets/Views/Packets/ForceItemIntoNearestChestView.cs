using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ForceItemIntoNearestChestView
    {
        private readonly PacketView _packet;
        private readonly ForceItemIntoNearestChest _packetObject;

        public ForceItemIntoNearestChestView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ForceItemIntoNearestChest, payload))
        {
        }

        public static ForceItemIntoNearestChestView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ForceItemIntoNearestChestView(payload);
        }

        public ForceItemIntoNearestChestView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ForceItemIntoNearestChest>(packet, PacketTypes.ForceItemIntoNearestChest);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ForceItemIntoNearestChest Packet => _packetObject;

        public byte InventorySlot => _packetObject.InventorySlot;
    }
}


