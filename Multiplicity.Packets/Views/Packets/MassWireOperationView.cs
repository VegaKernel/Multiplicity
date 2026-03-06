using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct MassWireOperationView
    {
        private readonly PacketView _packet;
        private readonly MassWireOperation _packetObject;

        public MassWireOperationView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.MassWireOperation, payload))
        {
        }

        public static MassWireOperationView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new MassWireOperationView(payload);
        }

        public MassWireOperationView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<MassWireOperation>(packet, PacketTypes.MassWireOperation);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public MassWireOperation Packet => _packetObject;

        public short StartX => _packetObject.StartX;

        public short StartY => _packetObject.StartY;

        public short EndX => _packetObject.EndX;

        public short EndY => _packetObject.EndY;

        public MassWireOperationToolMode ToolMode => _packetObject.ToolMode;
    }
}


