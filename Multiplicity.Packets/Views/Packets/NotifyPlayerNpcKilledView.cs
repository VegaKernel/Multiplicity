using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NotifyPlayerNpcKilledView
    {
        private readonly PacketView _packet;
        private readonly NotifyPlayerNpcKilled _packetObject;

        public NotifyPlayerNpcKilledView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NotifyPlayerNpcKilled, payload))
        {
        }

        public static NotifyPlayerNpcKilledView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NotifyPlayerNpcKilledView(payload);
        }

        public NotifyPlayerNpcKilledView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NotifyPlayerNpcKilled>(packet, PacketTypes.NotifyPlayerNpcKilled);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NotifyPlayerNpcKilled Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;
    }
}


