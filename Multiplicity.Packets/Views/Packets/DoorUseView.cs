using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct DoorUseView
    {
        private readonly PacketView _packet;
        private readonly DoorUse _packetObject;

        public DoorUseView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.DoorUse, payload))
        {
        }

        public static DoorUseView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new DoorUseView(payload);
        }

        public DoorUseView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<DoorUse>(packet, PacketTypes.DoorUse);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public DoorUse Packet => _packetObject;

        public DoorUseAction Action => _packetObject.Action;

        public short TileX => _packetObject.TileX;

        public short TileY => _packetObject.TileY;

        public byte Direction => _packetObject.Direction;
    }
}


