using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct MassWireOperationPayView
    {
        private readonly PacketView _packet;
        private readonly MassWireOperationPay _packetObject;

        public MassWireOperationPayView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.MassWireOperationPay, payload))
        {
        }

        public static MassWireOperationPayView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new MassWireOperationPayView(payload);
        }

        public MassWireOperationPayView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<MassWireOperationPay>(packet, PacketTypes.MassWireOperationPay);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public MassWireOperationPay Packet => _packetObject;

        public short ItemType => _packetObject.ItemType;

        public short Quantity => _packetObject.Quantity;

        public byte PlayerId => _packetObject.PlayerId;
    }
}


