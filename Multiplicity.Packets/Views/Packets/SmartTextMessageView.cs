using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SmartTextMessageView
    {
        private readonly PacketView _packet;
        private readonly SmartTextMessage _packetObject;

        public SmartTextMessageView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SmartTextMessage, payload))
        {
        }

        public static SmartTextMessageView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SmartTextMessageView(payload);
        }

        public SmartTextMessageView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SmartTextMessage>(packet, PacketTypes.SmartTextMessage);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SmartTextMessage Packet => _packetObject;

        public ColorStruct MessageColor => _packetObject.MessageColor;

        public NetworkText Message => _packetObject.Message;

        public short WidthLimit => _packetObject.WidthLimit;
    }
}


