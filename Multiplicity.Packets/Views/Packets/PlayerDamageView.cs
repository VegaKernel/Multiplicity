using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerDamageView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerDamageView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerDamage, payload))
        {
        }

        public static PlayerDamageView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerDamageView(payload);
        }

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
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            HitDirection = hitDirection;
            Damage = damage;
            DeathTextUtf8 = deathTextUtf8;
            Flags = flags;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

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



