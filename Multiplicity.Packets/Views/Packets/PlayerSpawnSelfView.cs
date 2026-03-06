using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerSpawnSelfView
    {
        private readonly PacketView _packet;
        private readonly PlayerSpawnSelf _packetObject;

        public PlayerSpawnSelfView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerSpawnSelf, payload))
        {
        }

        public static PlayerSpawnSelfView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerSpawnSelfView(payload);
        }

        public PlayerSpawnSelfView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerSpawnSelf>(packet, PacketTypes.PlayerSpawnSelf);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerSpawnSelf Packet => _packetObject;
    }
}


