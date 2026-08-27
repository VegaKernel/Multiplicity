using System;
using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Extensions.Mobile;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ServerInfoView
    {
        private readonly PacketView _packet;
        private readonly ServerInfo _packetObject;

        public ServerInfoView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(ServerInfo.PacketId, payload))
        {
        }

        public static ServerInfoView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ServerInfoView(payload);
        }

        public ServerInfoView(PacketView packet)
        {
            if (packet.Header.RawPacketId != ServerInfo.PacketId)
            {
                throw new InvalidDataException($"Expected packet id {ServerInfo.PacketId}, got {packet.Header.RawPacketId}.");
            }

            _packet = packet;
            _packetObject = (ServerInfo)new PacketDecoder(PacketExtensions.Mobile)
                .DecodePayload(ServerInfo.PacketId, packet.PayloadSpan.ToArray());
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ServerInfo Packet => _packetObject;
    }
}
