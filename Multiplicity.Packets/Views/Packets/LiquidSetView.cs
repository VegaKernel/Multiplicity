using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct LiquidSetView
    {
        private readonly PacketView _packet;
        private readonly LiquidSet _packetObject;

        public LiquidSetView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.LiquidSet, payload))
        {
        }

        public static LiquidSetView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new LiquidSetView(payload);
        }

        public LiquidSetView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<LiquidSet>(packet, PacketTypes.LiquidSet);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public LiquidSet Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public byte Liquid => _packetObject.Liquid;

        public LiquidKind LiquidType => _packetObject.LiquidType;
    }
}


