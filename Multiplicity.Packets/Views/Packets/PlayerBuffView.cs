using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerBuffView
    {
        private readonly PacketView _packet;
        private readonly PlayerBuff _packetObject;

        public PlayerBuffView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerBuff, payload))
        {
        }

        public static PlayerBuffView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerBuffView(payload);
        }

        public PlayerBuffView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerBuff>(packet, PacketTypes.PlayerBuff);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerBuff Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public ReadOnlyMemory<byte> BuffTypeMemory => _packetObject.BuffTypeMemory;
    }
}


