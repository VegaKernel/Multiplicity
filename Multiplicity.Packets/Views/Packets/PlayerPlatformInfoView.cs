using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerPlatformInfoView
    {
        private readonly PacketView _packet;

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
            packet.EnsureType(PacketTypes.PlayerPlatformInfo);
            _packet = packet;
            PacketReader reader = packet.CreatePayloadReader();
            PlayerSlot = reader.ReadByte();
            Platform = (PlayerPlatform)reader.ReadByte();
            PlatformUserIdUtf8 = reader.ReadLengthPrefixedBytes();
            if (Platform == PlayerPlatform.Nintendo)
                PlatformNameUtf8 = reader.ReadLengthPrefixedBytes();
            IsHost = reader.ReadBoolean();
            reader.EnsureEnd();
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public byte PlayerSlot { get; }
        public PlayerPlatform Platform { get; }
        public ReadOnlySpan<byte> PlatformUserIdUtf8 { get; }
        public ReadOnlySpan<byte> PlatformNameUtf8 { get; }
        public bool IsHost { get; }
        public PlayerPlatformInfo Packet => PacketViewMaterializer.Materialize<PlayerPlatformInfo>(_packet, PacketTypes.PlayerPlatformInfo);
    }
}
