using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerAddBuff (0x37) packet.
    /// </summary>
    public class PlayerAddBuff : TerrariaPacket
    {
        public byte PlayerId { get; set; }

        public ushort Buff { get; set; }

        public int Time { get; set; }

        public PlayerAddBuff()
            : base((byte)PacketTypes.PlayerAddBuff)
        {
        }

        public PlayerAddBuff(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            Buff = br.ReadUInt16();
            Time = br.ReadInt32();
        }

        public override string ToString()
        {
            return $"[PlayerAddBuff: PlayerId = {PlayerId} Buff = {Buff} Time = {Time}]";
        }

        public override short GetLength()
        {
            return 7;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(PlayerId);
                br.Write(Buff);
                br.Write(Time);
            }
        }
    }
}
