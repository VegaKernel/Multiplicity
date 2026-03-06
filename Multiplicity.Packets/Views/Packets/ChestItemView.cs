using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChestItemView
    {
        private readonly PacketView _packet;
        private readonly ChestItem _packetObject;

        public ChestItemView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ChestItem, payload))
        {
        }

        public static ChestItemView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ChestItemView(payload);
        }

        public ChestItemView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ChestItem>(packet, PacketTypes.ChestItem);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ChestItem Packet => _packetObject;

        public short ChestId => _packetObject.ChestId;

        public byte ItemSlot => _packetObject.ItemSlot;

        public short Stack => _packetObject.Stack;

        public byte Prefix => _packetObject.Prefix;

        public short ItemNetId => _packetObject.ItemNetId;
    }
}


