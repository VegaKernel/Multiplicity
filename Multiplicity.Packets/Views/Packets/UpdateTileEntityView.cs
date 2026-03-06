using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdateTileEntityView
    {
        private readonly PacketView _packet;
        private readonly UpdateTileEntity _packetObject;

        public UpdateTileEntityView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.UpdateTileEntity, payload))
        {
        }

        public static UpdateTileEntityView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new UpdateTileEntityView(payload);
        }

        public UpdateTileEntityView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<UpdateTileEntity>(packet, PacketTypes.UpdateTileEntity);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public UpdateTileEntity Packet => _packetObject;

        public int Key => _packetObject.Key;

        public bool Exists => _packetObject.Exists;

        public ReadOnlyMemory<byte> TileEntityPayloadMemory => _packetObject.TileEntityPayloadMemory;
    }
}


