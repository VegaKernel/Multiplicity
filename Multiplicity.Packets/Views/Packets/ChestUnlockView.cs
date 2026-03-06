using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChestUnlockView
    {
        private readonly PacketView _packet;
        private readonly ChestUnlock _packetObject;

        public ChestUnlockView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ChestUnlock, payload))
        {
        }

        public static ChestUnlockView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ChestUnlockView(payload);
        }

        public ChestUnlockView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ChestUnlock>(packet, PacketTypes.ChestUnlock);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ChestUnlock Packet => _packetObject;

        public ChestUnlockAction Action => _packetObject.Action;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;
    }
}


