using System;
using System.Collections.Generic;
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

        public IReadOnlyList<NpcBuffEntry> Buffs => _packetObject.Buffs;

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId => _packetObject.BuffId;

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time => _packetObject.Time;

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId2 => _packetObject.BuffId2;

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time2 => _packetObject.Time2;

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId3 => _packetObject.BuffId3;

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time3 => _packetObject.Time3;

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId4 => _packetObject.BuffId4;

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time4 => _packetObject.Time4;

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId5 => _packetObject.BuffId5;

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time5 => _packetObject.Time5;
    }
}
