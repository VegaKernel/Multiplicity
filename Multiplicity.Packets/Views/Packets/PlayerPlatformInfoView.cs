using System;
using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Extensions.Mobile;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerPlatformInfoView
    {
        private readonly PacketView _packet;
        private readonly PlayerPlatformInfo _packetObject;

        public PlayerPlatformInfoView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PlayerPlatformInfo.PacketId, payload))
        {
        }

        public static PlayerPlatformInfoView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerPlatformInfoView(payload);
        }

        public PlayerPlatformInfoView(PacketView packet)
        {
            if (packet.Header.RawPacketId != PlayerPlatformInfo.PacketId)
            {
                throw new InvalidDataException($"Expected packet id {PlayerPlatformInfo.PacketId}, got {packet.Header.RawPacketId}.");
            }

            _packet = packet;
            _packetObject = (PlayerPlatformInfo)new PacketDecoder(PacketExtensions.Mobile)
                .DecodePayload(PlayerPlatformInfo.PacketId, packet.PayloadSpan.ToArray());
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerPlatformInfo Packet => _packetObject;
    }
}
