using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncPlayerChestIndexView
    {
        private readonly PacketView _packet;
        private readonly SyncPlayerChestIndex _packetObject;

        public SyncPlayerChestIndexView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SyncPlayerChestIndex, payload))
        {
        }

        public static SyncPlayerChestIndexView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SyncPlayerChestIndexView(payload);
        }

        public SyncPlayerChestIndexView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SyncPlayerChestIndex>(packet, PacketTypes.SyncPlayerChestIndex);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SyncPlayerChestIndex Packet => _packetObject;

        public byte Player => _packetObject.Player;

        public short Chest => _packetObject.Chest;
    }
}


