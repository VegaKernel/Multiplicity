using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerDeathView
    {
        private readonly PacketView _packet;
        private readonly PlayerDeath _packetObject;

        public PlayerDeathView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerDeath, payload))
        {
        }

        public static PlayerDeathView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerDeathView(payload);
        }

        public PlayerDeathView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerDeath>(packet, PacketTypes.PlayerDeath);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerDeath Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public byte HitDirection => _packetObject.HitDirection;

        public short Damage => _packetObject.Damage;

        public bool Pvp => _packetObject.Pvp;

        public string DeathText => _packetObject.DeathText;
    }
}


