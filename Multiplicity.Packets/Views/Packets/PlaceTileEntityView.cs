using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlaceTileEntityView
    {
        private readonly PacketView _packet;
        private readonly PlaceTileEntity _packetObject;

        public PlaceTileEntityView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlaceTileEntity, payload))
        {
        }

        public static PlaceTileEntityView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlaceTileEntityView(payload);
        }

        public PlaceTileEntityView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlaceTileEntity>(packet, PacketTypes.PlaceTileEntity);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlaceTileEntity Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public PlaceTileEntityType EntityType => _packetObject.EntityType;
    }
}


