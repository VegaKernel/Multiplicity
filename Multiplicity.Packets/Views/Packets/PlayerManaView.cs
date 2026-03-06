using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerManaView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerManaView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerMana, payload))
        {
        }

        public static PlayerManaView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerManaView(payload);
        }

        public PlayerManaView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerMana);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short mana = reader.ReadInt16();
            short maxMana = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            Mana = mana;
            MaxMana = maxMana;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public short Mana { get; }

        public short MaxMana { get; }
    }
}



