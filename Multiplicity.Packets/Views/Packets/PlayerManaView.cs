using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerManaView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerManaView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerMana);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short mana = reader.ReadInt16();
            short maxMana = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            Mana = mana;
            MaxMana = maxMana;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public short Mana { get; }

        public short MaxMana { get; }
    }
}
