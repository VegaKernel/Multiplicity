using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcAddBuff (0x35) packet.
    /// </summary>
    public class NpcAddBuff : TerrariaPacket
    {
        public short NpcId { get; set; }

        public ushort Buff { get; set; }

        public short Time { get; set; }

        public NpcAddBuff()
            : base((byte)PacketTypes.NpcAddBuff)
        {
        }

        public NpcAddBuff(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadInt16();
            Buff = br.ReadUInt16();
            Time = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[NpcAddBuff: NpcId = {NpcId} Buff = {Buff} Time = {Time}]";
        }

        public override short GetLength()
        {
            return 6;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(NpcId);
                br.Write(Buff);
                br.Write(Time);
            }
        }
    }
}
