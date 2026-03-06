using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TeleportationPotionView
    {
        private readonly PacketView _packet;
        private readonly TeleportationPotion _packetObject;

        public TeleportationPotionView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TeleportationPotion, payload))
        {
        }

        public static TeleportationPotionView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TeleportationPotionView(payload);
        }

        public TeleportationPotionView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TeleportationPotion>(packet, PacketTypes.TeleportationPotion);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TeleportationPotion Packet => _packetObject;
    }
}


