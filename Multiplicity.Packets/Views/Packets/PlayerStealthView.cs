using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerStealthView
    {
        private readonly PacketView _packet;
        private readonly PlayerStealth _packetObject;

        public PlayerStealthView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlayerStealth, payload))
        {
        }

        public static PlayerStealthView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerStealthView(payload);
        }

        public PlayerStealthView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlayerStealth>(packet, PacketTypes.PlayerStealth);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlayerStealth Packet => _packetObject;

        public byte Player => _packetObject.Player;

        public float Stealth => _packetObject.Stealth;
    }
}


