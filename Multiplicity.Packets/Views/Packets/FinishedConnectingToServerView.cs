using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct FinishedConnectingToServerView
    {
        private readonly PacketView _packet;
        private readonly FinishedConnectingToServer _packetObject;

        public FinishedConnectingToServerView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.FinishedConnectingToServer, payload))
        {
        }

        public static FinishedConnectingToServerView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new FinishedConnectingToServerView(payload);
        }

        public FinishedConnectingToServerView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<FinishedConnectingToServer>(packet, PacketTypes.FinishedConnectingToServer);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public FinishedConnectingToServer Packet => _packetObject;
    }
}


