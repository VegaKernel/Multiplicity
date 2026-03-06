using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct HitSwitchView
    {
        private readonly PacketView _packet;
        private readonly HitSwitch _packetObject;

        public HitSwitchView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.HitSwitch, payload))
        {
        }

        public static HitSwitchView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new HitSwitchView(payload);
        }

        public HitSwitchView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<HitSwitch>(packet, PacketTypes.HitSwitch);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public HitSwitch Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;
    }
}


