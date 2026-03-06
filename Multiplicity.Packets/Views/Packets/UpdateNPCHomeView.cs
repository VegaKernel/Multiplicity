using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdateNPCHomeView
    {
        private readonly PacketView _packet;
        private readonly UpdateNPCHome _packetObject;

        public UpdateNPCHomeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.UpdateNPCHome, payload))
        {
        }

        public static UpdateNPCHomeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new UpdateNPCHomeView(payload);
        }

        public UpdateNPCHomeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<UpdateNPCHome>(packet, PacketTypes.UpdateNPCHome);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public UpdateNPCHome Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;

        public short HomeTileX => _packetObject.HomeTileX;

        public short HomeTileY => _packetObject.HomeTileY;

        public byte Homeless => _packetObject.Homeless;
    }
}


