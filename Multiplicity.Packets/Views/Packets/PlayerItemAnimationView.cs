using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerItemAnimationView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerItemAnimationView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerItemAnimation);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            float itemRotation = reader.ReadSingle();
            short itemAnimation = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            ItemRotation = itemRotation;
            ItemAnimation = itemAnimation;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public float ItemRotation { get; }

        public short ItemAnimation { get; }
    }
}
