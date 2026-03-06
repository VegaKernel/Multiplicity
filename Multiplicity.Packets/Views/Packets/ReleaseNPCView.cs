using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ReleaseNPCView
    {
        private readonly PacketView _packet;
        private readonly ReleaseNPC _packetObject;

        public ReleaseNPCView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ReleaseNPC, payload))
        {
        }

        public static ReleaseNPCView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ReleaseNPCView(payload);
        }

        public ReleaseNPCView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ReleaseNPC>(packet, PacketTypes.ReleaseNPC);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ReleaseNPC Packet => _packetObject;

        public int X => _packetObject.X;

        public int Y => _packetObject.Y;

        public short NpcType => _packetObject.NpcType;

        public byte Style => _packetObject.Style;
    }
}


