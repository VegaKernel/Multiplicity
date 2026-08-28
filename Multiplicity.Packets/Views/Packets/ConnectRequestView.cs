using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ConnectRequestView
    {
        private readonly PacketView _packet;
        private readonly ConnectRequest _packetObject;

        public ConnectRequestView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ConnectRequest, payload))
        {
        }

        public static ConnectRequestView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ConnectRequestView(payload);
        }

        public ConnectRequestView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ConnectRequest);

            PacketReader reader = packet.CreatePayloadReader();
            string version = reader.ReadStringUtf8();
            reader.EnsureEnd();

            _packet = packet;
            _packetObject = new ConnectRequest { Version = version };
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ConnectRequest Packet => _packetObject;

        public string Version => _packetObject.Version;
    }
}
