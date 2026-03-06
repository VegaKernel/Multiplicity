using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerAnimationView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerAnimationView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerAnimation, payload))
        {
        }

        public static PlayerAnimationView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerAnimationView(payload);
        }

        public PlayerAnimationView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerAnimation);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            float itemRotation = reader.ReadSingle();
            short itemAnimation = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            ItemRotation = itemRotation;
            ItemAnimation = itemAnimation;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public float ItemRotation { get; }

        public short ItemAnimation { get; }
    }
}



