using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerHpView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerHpView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerHp, payload))
        {
        }

        public static PlayerHpView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerHpView(payload);
        }

        public PlayerHpView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerHp);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short hp = reader.ReadInt16();
            short maxHp = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            Hp = hp;
            MaxHp = maxHp;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public short Hp { get; }

        public short MaxHp { get; }
    }
}




