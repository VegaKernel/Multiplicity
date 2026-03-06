using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerDamageView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerDamageView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerDamage);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte hitDirection = reader.ReadByte();
            short damage = reader.ReadInt16();
            ReadOnlySpan<byte> deathTextUtf8 = reader.ReadLengthPrefixedBytes();
            byte flags = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            HitDirection = hitDirection;
            Damage = damage;
            DeathTextUtf8 = deathTextUtf8;
            Flags = flags;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public byte HitDirection { get; }

        public short Damage { get; }

        public ReadOnlySpan<byte> DeathTextUtf8 { get; }

        public byte Flags { get; }

        public string GetDeathText()
        {
            return Encoding.UTF8.GetString(DeathTextUtf8);
        }
    }
}
