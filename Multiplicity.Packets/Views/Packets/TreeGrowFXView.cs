using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TreeGrowFXView
    {
        private readonly PacketView _packet;
        private readonly TreeGrowFX _packetObject;

        public TreeGrowFXView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TreeGrowFX, payload))
        {
        }

        public static TreeGrowFXView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TreeGrowFXView(payload);
        }

        public TreeGrowFXView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TreeGrowFX>(packet, PacketTypes.TreeGrowFX);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TreeGrowFX Packet => _packetObject;

        public bool GrowEffect => _packetObject.GrowEffect;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public byte Height => _packetObject.Height;

        public short TreeGore => _packetObject.TreeGore;
    }
}


