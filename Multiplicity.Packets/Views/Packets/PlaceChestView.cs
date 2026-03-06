using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlaceChestView
    {
        private readonly PacketView _packet;
        private readonly PlaceChest _packetObject;

        public PlaceChestView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlaceChest, payload))
        {
        }

        public static PlaceChestView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlaceChestView(payload);
        }

        public PlaceChestView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlaceChest>(packet, PacketTypes.PlaceChest);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlaceChest Packet => _packetObject;

        public PlaceChestAction Action => _packetObject.Action;

        public short TileX => _packetObject.TileX;

        public short TileY => _packetObject.TileY;

        public short Style => _packetObject.Style;

        public short ChestIdToDestroy => _packetObject.ChestIdToDestroy;
    }
}


