using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcAddBuffView
    {
        private readonly PacketView _packet;
        private readonly NpcAddBuff _packetObject;

        public NpcAddBuffView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcAddBuff, payload))
        {
        }

        public static NpcAddBuffView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NpcAddBuffView(payload);
        }

        public NpcAddBuffView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NpcAddBuff>(packet, PacketTypes.NpcAddBuff);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NpcAddBuff Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;

        public ushort Buff => _packetObject.Buff;

        public short Time => _packetObject.Time;
    }
}
