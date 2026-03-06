using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcKillCountView
    {
        private readonly PacketView _packet;
        private readonly NpcKillCount _packetObject;

        public NpcKillCountView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcKillCount, payload))
        {
        }

        public static NpcKillCountView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcKillCountView(payload);
        }

        public NpcKillCountView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcKillCount>(packet, PacketTypes.NpcKillCount);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcKillCount Packet => _packetObject;

        public short NpcType => _packetObject.NpcType;

        public int KillCount => _packetObject.KillCount;
    }
}


