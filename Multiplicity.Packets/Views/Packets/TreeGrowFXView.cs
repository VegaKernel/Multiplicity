using System;

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
        public byte EffectType => _packetObject.EffectType;
        public int X => _packetObject.X;
        public int Y => _packetObject.Y;
        public byte Style => _packetObject.Style;
        public short Extra => _packetObject.Extra;
        public bool ExtraFlag => _packetObject.ExtraFlag;
    }
}
