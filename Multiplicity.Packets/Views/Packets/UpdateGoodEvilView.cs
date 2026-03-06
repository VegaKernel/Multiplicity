using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdateGoodEvilView
    {
        private readonly PacketView _packet;
        private readonly UpdateGoodEvil _packetObject;

        public UpdateGoodEvilView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.UpdateGoodEvil, payload))
        {
        }

        public static UpdateGoodEvilView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new UpdateGoodEvilView(payload);
        }

        public UpdateGoodEvilView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<UpdateGoodEvil>(packet, PacketTypes.UpdateGoodEvil);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public UpdateGoodEvil Packet => _packetObject;

        public byte Good => _packetObject.Good;

        public byte Evil => _packetObject.Evil;

        public byte Crimson => _packetObject.Crimson;
    }
}


