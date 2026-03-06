using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct GemLockToggleView
    {
        private readonly PacketView _packet;
        private readonly GemLockToggle _packetObject;

        public GemLockToggleView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.GemLockToggle, payload))
        {
        }

        public static GemLockToggleView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new GemLockToggleView(payload);
        }

        public GemLockToggleView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<GemLockToggle>(packet, PacketTypes.GemLockToggle);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public GemLockToggle Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public bool On => _packetObject.On;
    }
}


