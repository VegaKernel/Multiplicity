using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SignNewView
    {
        private readonly PacketView _packet;
        private readonly SignNew _packetObject;

        public SignNewView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SignNew, payload))
        {
        }

        public static SignNewView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SignNewView(payload);
        }

        public SignNewView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SignNew>(packet, PacketTypes.SignNew);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SignNew Packet => _packetObject;

        public short SignId => _packetObject.SignId;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public string Text => _packetObject.Text;

        public byte PlayerId => _packetObject.PlayerId;

        public byte Flags => _packetObject.Flags;

        public bool SuppressOpen => _packetObject.SuppressOpen;
    }
}


