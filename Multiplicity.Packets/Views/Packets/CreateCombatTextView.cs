using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CreateCombatTextView
    {
        private readonly PacketView _packet;
        private readonly CreateCombatText _packetObject;

        public CreateCombatTextView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CreateCombatText, payload))
        {
        }

        public static CreateCombatTextView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CreateCombatTextView(payload);
        }

        public CreateCombatTextView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CreateCombatText>(packet, PacketTypes.CreateCombatText);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CreateCombatText Packet => _packetObject;

        public float X => _packetObject.X;

        public float Y => _packetObject.Y;

        public ColorStruct Color => _packetObject.Color;

        public int HealAmount => _packetObject.HealAmount;
    }
}


