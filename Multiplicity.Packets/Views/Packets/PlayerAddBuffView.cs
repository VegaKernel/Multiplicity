using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerAddBuffView
    {
        private readonly PacketView _packet;
        private readonly PlayerAddBuff _packetObject;

        public PlayerAddBuffView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerAddBuff, payload))
        {
        }

        public static PlayerAddBuffView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerAddBuffView(payload);
        }

        public PlayerAddBuffView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerAddBuff>(packet, PacketTypes.PlayerAddBuff);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerAddBuff Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public ushort Buff => _packetObject.Buff;

        public int Time => _packetObject.Time;
    }
}
