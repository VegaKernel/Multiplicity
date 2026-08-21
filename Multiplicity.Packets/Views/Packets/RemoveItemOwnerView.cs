using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RemoveItemOwnerView
    {
        private readonly PacketView _packet;
        private readonly RemoveItemOwner _packetObject;

        public RemoveItemOwnerView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.RemoveItemOwner, payload))
        {
        }

        public static RemoveItemOwnerView FromPayload(ReadOnlySpan<byte> payload) => new RemoveItemOwnerView(payload);

        public RemoveItemOwnerView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<RemoveItemOwner>(packet, PacketTypes.RemoveItemOwner);
        }

        public PacketHeader Header => _packet.Header;
        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;
        public RemoveItemOwner Packet => _packetObject;
        public short ItemIndex => _packetObject.ItemIndex;
        public bool ForceAssignToServer => _packetObject.ForceAssignToServer;
    }
}
