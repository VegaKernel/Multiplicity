using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncExtraValueView
    {
        private readonly PacketView _packet;
        private readonly SyncExtraValue _packetObject;

        public SyncExtraValueView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SyncExtraValue, payload))
        {
        }

        public static SyncExtraValueView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SyncExtraValueView(payload);
        }

        public SyncExtraValueView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SyncExtraValue>(packet, PacketTypes.SyncExtraValue);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SyncExtraValue Packet => _packetObject;

        public short NpcIndex => _packetObject.NpcIndex;

        public int ExtraValue => _packetObject.ExtraValue;

        public float X => _packetObject.X;

        public float Y => _packetObject.Y;
    }
}
