using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct DebugCommandsView
    {
        private readonly PacketView _packet;
        private readonly DebugCommands _packetObject;

        public DebugCommandsView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.DebugCommands, payload))
        {
        }

        public static DebugCommandsView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new DebugCommandsView(payload);
        }

        public DebugCommandsView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<DebugCommands>(packet, PacketTypes.DebugCommands);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public DebugCommands Packet => _packetObject;
    }
}


