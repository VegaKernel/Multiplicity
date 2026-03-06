using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChestNameView
    {
        private readonly PacketView _packet;
        private readonly ChestName _packetObject;

        public ChestNameView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ChestName, payload))
        {
        }

        public static ChestNameView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ChestNameView(payload);
        }

        public ChestNameView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ChestName>(packet, PacketTypes.ChestName);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ChestName Packet => _packetObject;

        public short ChestId => _packetObject.ChestId;

        public short ChestX => _packetObject.ChestX;

        public short ChestY => _packetObject.ChestY;

        public string Name => _packetObject.Name;
    }
}


