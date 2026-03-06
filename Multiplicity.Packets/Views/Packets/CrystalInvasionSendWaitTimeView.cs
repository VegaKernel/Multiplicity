using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CrystalInvasionSendWaitTimeView
    {
        private readonly PacketView _packet;
        private readonly CrystalInvasionSendWaitTime _packetObject;

        public CrystalInvasionSendWaitTimeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CrystalInvasionSendWaitTime, payload))
        {
        }

        public static CrystalInvasionSendWaitTimeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CrystalInvasionSendWaitTimeView(payload);
        }

        public CrystalInvasionSendWaitTimeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CrystalInvasionSendWaitTime>(packet, PacketTypes.CrystalInvasionSendWaitTime);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CrystalInvasionSendWaitTime Packet => _packetObject;

        public int TimeUntilNextWave => _packetObject.TimeUntilNextWave;
    }
}


