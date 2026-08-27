using System;
using System.Collections.Generic;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcUpdateBuff (0x36) packet.
    /// Modern Terraria serializes active NPC buffs as a variable-length sequence of
    /// ushort buff type + ushort time pairs followed by a zero ushort terminator.
    /// </summary>
    public class NpcUpdateBuff : TerrariaPacket
    {
        private const short BasePayloadLength = sizeof(short) + sizeof(ushort);
        private const short BuffPayloadLength = sizeof(ushort) + sizeof(ushort);

        public short NpcId { get; set; }

        public List<NpcBuffEntry> Buffs { get; } = new List<NpcBuffEntry>();

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId
        {
            get => GetLegacyBuffId(0);
            set => GetOrCreateLegacyEntry(0).BuffType = value;
        }

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time
        {
            get => GetLegacyBuffTime(0);
            set => GetOrCreateLegacyEntry(0).Time = unchecked((ushort)value);
        }

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId2
        {
            get => GetLegacyBuffId(1);
            set => GetOrCreateLegacyEntry(1).BuffType = value;
        }

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time2
        {
            get => GetLegacyBuffTime(1);
            set => GetOrCreateLegacyEntry(1).Time = unchecked((ushort)value);
        }

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId3
        {
            get => GetLegacyBuffId(2);
            set => GetOrCreateLegacyEntry(2).BuffType = value;
        }

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time3
        {
            get => GetLegacyBuffTime(2);
            set => GetOrCreateLegacyEntry(2).Time = unchecked((ushort)value);
        }

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId4
        {
            get => GetLegacyBuffId(3);
            set => GetOrCreateLegacyEntry(3).BuffType = value;
        }

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time4
        {
            get => GetLegacyBuffTime(3);
            set => GetOrCreateLegacyEntry(3).Time = unchecked((ushort)value);
        }

        [Obsolete("Use Buffs. The wire buff type is ushort and the packet is variable-length.")]
        public byte BuffId5
        {
            get => GetLegacyBuffId(4);
            set => GetOrCreateLegacyEntry(4).BuffType = value;
        }

        [Obsolete("Use Buffs. The wire buff time is ushort and the packet is variable-length.")]
        public short Time5
        {
            get => GetLegacyBuffTime(4);
            set => GetOrCreateLegacyEntry(4).Time = unchecked((ushort)value);
        }

        public NpcUpdateBuff()
            : base((byte)PacketTypes.NpcUpdateBuff)
        {
        }

        public NpcUpdateBuff(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadInt16();

            while (true)
            {
                ushort buffType = br.ReadUInt16();
                if (buffType == 0)
                {
                    break;
                }

                ushort time = br.ReadUInt16();
                Buffs.Add(new NpcBuffEntry(buffType, time));
            }
        }

        public override string ToString()
        {
            return $"[NpcUpdateBuff: NpcId = {NpcId} BuffCount = {CountSerializableBuffs()}]";
        }

        public override short GetLength()
        {
            return checked((short)(BasePayloadLength + (CountSerializableBuffs() * BuffPayloadLength)));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(NpcId);

            for (int i = 0; i < Buffs.Count; i++)
            {
                NpcBuffEntry entry = Buffs[i];
                if (entry == null || entry.BuffType == 0)
                {
                    continue;
                }

                bw.Write(entry.BuffType);
                bw.Write(entry.Time);
            }

            bw.Write((ushort)0);
        }

        private int CountSerializableBuffs()
        {
            int count = 0;
            for (int i = 0; i < Buffs.Count; i++)
            {
                NpcBuffEntry entry = Buffs[i];
                if (entry != null && entry.BuffType != 0)
                {
                    count++;
                }
            }

            return count;
        }

        private NpcBuffEntry GetOrCreateLegacyEntry(int index)
        {
            while (Buffs.Count <= index)
            {
                Buffs.Add(new NpcBuffEntry());
            }

            return Buffs[index];
        }

        private byte GetLegacyBuffId(int index)
        {
            if (index >= Buffs.Count || Buffs[index] == null)
            {
                return 0;
            }

            return checked((byte)Buffs[index].BuffType);
        }

        private short GetLegacyBuffTime(int index)
        {
            if (index >= Buffs.Count || Buffs[index] == null)
            {
                return 0;
            }

            return unchecked((short)Buffs[index].Time);
        }
    }
}
