using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ReportInvasionProgressView
    {
        private readonly PacketView _packet;
        private readonly ReportInvasionProgress _packetObject;

        public ReportInvasionProgressView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ReportInvasionProgress, payload))
        {
        }

        public static ReportInvasionProgressView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ReportInvasionProgressView(payload);
        }

        public ReportInvasionProgressView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ReportInvasionProgress>(packet, PacketTypes.ReportInvasionProgress);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ReportInvasionProgress Packet => _packetObject;

        public int Progress => _packetObject.Progress;

        public int MaxProgress => _packetObject.MaxProgress;

        public sbyte Icon => _packetObject.Icon;

        public sbyte Wave => _packetObject.Wave;
    }
}


