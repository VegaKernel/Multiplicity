using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerPlatformInfoView
    {
        private readonly PacketView _packet;
        private readonly PlayerPlatformInfo _packetObject;

        public PlayerPlatformInfoView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerPlatformInfo, payload))
        {
        }

        public static PlayerPlatformInfoView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerPlatformInfoView(payload);
        }

        public PlayerPlatformInfoView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerPlatformInfo>(packet, PacketTypes.PlayerPlatformInfo);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerPlatformInfo Packet => _packetObject;
    }
}


