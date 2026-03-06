using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CrystalInvasionRequestedToSkipWaitTimeView
    {
        private readonly PacketView _packet;
        private readonly CrystalInvasionRequestedToSkipWaitTime _packetObject;

        public CrystalInvasionRequestedToSkipWaitTimeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CrystalInvasionRequestedToSkipWaitTime, payload))
        {
        }

        public static CrystalInvasionRequestedToSkipWaitTimeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CrystalInvasionRequestedToSkipWaitTimeView(payload);
        }

        public CrystalInvasionRequestedToSkipWaitTimeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CrystalInvasionRequestedToSkipWaitTime>(packet, PacketTypes.CrystalInvasionRequestedToSkipWaitTime);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CrystalInvasionRequestedToSkipWaitTime Packet => _packetObject;
    }
}


