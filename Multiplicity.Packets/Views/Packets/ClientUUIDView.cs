using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ClientUUIDView
    {
        private readonly PacketView _packet;
        private readonly ClientUUID _packetObject;

        public ClientUUIDView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ClientUUID, payload))
        {
        }

        public static ClientUUIDView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ClientUUIDView(payload);
        }

        public ClientUUIDView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ClientUUID>(packet, PacketTypes.ClientUUID);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ClientUUID Packet => _packetObject;

        public string Uuid => _packetObject.Uuid;
    }
}


