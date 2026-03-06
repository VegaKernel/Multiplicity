using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ClientSyncedInventoryView
    {
        private readonly PacketView _packet;
        private readonly ClientSyncedInventory _packetObject;

        public ClientSyncedInventoryView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ClientSyncedInventory, payload))
        {
        }

        public static ClientSyncedInventoryView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ClientSyncedInventoryView(payload);
        }

        public ClientSyncedInventoryView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ClientSyncedInventory>(packet, PacketTypes.ClientSyncedInventory);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ClientSyncedInventory Packet => _packetObject;
    }
}


