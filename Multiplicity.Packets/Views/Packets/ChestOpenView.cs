using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChestOpenView
    {
        private readonly PacketView _packet;
        private readonly ChestOpen _packetObject;

        public ChestOpenView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ChestOpen, payload))
        {
        }

        public static ChestOpenView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ChestOpenView(payload);
        }

        public ChestOpenView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ChestOpen>(packet, PacketTypes.ChestOpen);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ChestOpen Packet => _packetObject;

        public short ChestId => _packetObject.ChestId;

        public short ChestX => _packetObject.ChestX;

        public short ChestY => _packetObject.ChestY;

        public byte NameLength => _packetObject.NameLength;

        public string ChestName => _packetObject.ChestName;
    }
}


