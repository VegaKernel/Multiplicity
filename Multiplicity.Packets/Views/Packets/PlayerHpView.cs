using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerHpView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerHpView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerHP);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short hp = reader.ReadInt16();
            short maxHp = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            Hp = hp;
            MaxHp = maxHp;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public short Hp { get; }

        public short MaxHp { get; }
    }
}
