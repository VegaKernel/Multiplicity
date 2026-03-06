using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncChestSizeView
    {
        private readonly PacketView _packet;
        private readonly SyncChestSize _packetObject;

        public SyncChestSizeView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SyncChestSize, payload))
        {
        }

        public static SyncChestSizeView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SyncChestSizeView(payload);
        }

        public SyncChestSizeView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SyncChestSize>(packet, PacketTypes.SyncChestSize);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SyncChestSize Packet => _packetObject;

        public short ChestId => _packetObject.ChestId;

        public short NewSize => _packetObject.NewSize;
    }
}


