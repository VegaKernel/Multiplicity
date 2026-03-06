using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TimeSetView
    {
        private readonly PacketView _packet;
        private readonly TimeSet _packetObject;

        public TimeSetView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TimeSet, payload))
        {
        }

        public static TimeSetView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TimeSetView(payload);
        }

        public TimeSetView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TimeSet>(packet, PacketTypes.TimeSet);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TimeSet Packet => _packetObject;

        public bool DayTime => _packetObject.DayTime;

        public int TimeValue => _packetObject.TimeValue;

        public short SunModY => _packetObject.SunModY;

        public short MoonModY => _packetObject.MoonModY;
    }
}


