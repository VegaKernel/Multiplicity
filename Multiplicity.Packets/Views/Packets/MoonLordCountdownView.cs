using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct MoonLordCountdownView
    {
        private readonly PacketView _packet;
        private readonly MoonLordCountdown _packetObject;

        public MoonLordCountdownView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.MoonLordCountdown, payload))
        {
        }

        public static MoonLordCountdownView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new MoonLordCountdownView(payload);
        }

        public MoonLordCountdownView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<MoonLordCountdown>(packet, PacketTypes.MoonLordCountdown);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public MoonLordCountdown Packet => _packetObject;

        public int CountdownValue => _packetObject.CountdownValue;
    }
}


