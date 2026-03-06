using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ServerInfoView
    {
        private readonly PacketView _packet;
        private readonly ServerInfo _packetObject;

        public ServerInfoView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ServerInfo, payload))
        {
        }

        public static ServerInfoView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ServerInfoView(payload);
        }

        public ServerInfoView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ServerInfo>(packet, PacketTypes.ServerInfo);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ServerInfo Packet => _packetObject;
    }
}


