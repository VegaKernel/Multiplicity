using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcUpdateBuffView
    {
        private readonly PacketView _packet;
        private readonly NpcUpdateBuff _packetObject;

        public NpcUpdateBuffView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcUpdateBuff, payload))
        {
        }

        public static NpcUpdateBuffView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcUpdateBuffView(payload);
        }

        public NpcUpdateBuffView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcUpdateBuff>(packet, PacketTypes.NpcUpdateBuff);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcUpdateBuff Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;

        public byte BuffId => _packetObject.BuffId;

        public short Time => _packetObject.Time;

        public byte BuffId2 => _packetObject.BuffId2;

        public short Time2 => _packetObject.Time2;

        public byte BuffId3 => _packetObject.BuffId3;

        public short Time3 => _packetObject.Time3;

        public byte BuffId4 => _packetObject.BuffId4;

        public short Time4 => _packetObject.Time4;

        public byte BuffId5 => _packetObject.BuffId5;

        public short Time5 => _packetObject.Time5;
    }
}


