using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TeleportView
    {
        private readonly PacketView _packet;
        private readonly Teleport _packetObject;

        public TeleportView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.Teleport, payload))
        {
        }

        public static TeleportView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TeleportView(payload);
        }

        public TeleportView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<Teleport>(packet, PacketTypes.Teleport);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public Teleport Packet => _packetObject;

        public TeleportOperation Operation => _packetObject.Operation;

        public bool UseCurrentEntityPosition => _packetObject.UseCurrentEntityPosition;

        public short TargetId => _packetObject.TargetId;

        public float X => _packetObject.X;

        public float Y => _packetObject.Y;

        public byte TeleportStyle => _packetObject.TeleportStyle;

        public bool HasExtraInfo => _packetObject.HasExtraInfo;

        public int ExtraInfo => _packetObject.ExtraInfo;
    }
}


