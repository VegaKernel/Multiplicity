using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CreateCombatTextExtendedView
    {
        private readonly PacketView _packet;
        private readonly CreateCombatTextExtended _packetObject;

        public CreateCombatTextExtendedView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CreateCombatTextExtended, payload))
        {
        }

        public static CreateCombatTextExtendedView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CreateCombatTextExtendedView(payload);
        }

        public CreateCombatTextExtendedView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CreateCombatTextExtended>(packet, PacketTypes.CreateCombatTextExtended);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CreateCombatTextExtended Packet => _packetObject;

        public float X => _packetObject.X;

        public float Y => _packetObject.Y;

        public ColorStruct Color => _packetObject.Color;

        public NetworkText CombatText => _packetObject.CombatText;
    }
}


