using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct CreateTemporaryAnimationView
    {
        private readonly PacketView _packet;
        private readonly CreateTemporaryAnimation _packetObject;

        public CreateTemporaryAnimationView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.CreateTemporaryAnimation, payload))
        {
        }

        public static CreateTemporaryAnimationView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new CreateTemporaryAnimationView(payload);
        }

        public CreateTemporaryAnimationView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<CreateTemporaryAnimation>(packet, PacketTypes.CreateTemporaryAnimation);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public CreateTemporaryAnimation Packet => _packetObject;

        public short AnimationType => _packetObject.AnimationType;

        public ushort TileType => _packetObject.TileType;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;
    }
}


