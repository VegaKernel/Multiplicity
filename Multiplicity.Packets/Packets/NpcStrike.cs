using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcStrike (0x1C) packet.
    /// </summary>
    public class NpcStrike : TerrariaPacket
    {

        public short NpcId { get; set; }

        public short Damage { get; set; }

        public float Knockback { get; set; }

        public byte Direction { get; set; }

        public bool Crit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcStrike"/> class.
        /// </summary>
        public NpcStrike()
            : base((byte)PacketTypes.NpcStrike)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcStrike"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public NpcStrike(BinaryReader br)
            : base(br)
        {
            this.NpcId = br.ReadInt16();
            this.Damage = br.ReadInt16();
            this.Knockback = br.ReadSingle();
            this.Direction = br.ReadByte();
            this.Crit = br.ReadBoolean();
        }

        public override string ToString()
        {
            return $"[NpcStrike: NpcId = {NpcId} Damage = {Damage} Knockback = {Knockback} Direction = {Direction} Crit = {Crit}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(10);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            /*
             * Length and Id headers get written in the base packet class.
             */
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            /*
             * Always make sure to not close the stream when serializing.
             * 
             * It is up to the caller to decide if the underlying stream
             * gets closed.  If this is a network stream we do not want
             * the regressions of unconditionally closing the TCP socket
             * once the payload of data has been sent to the client.
             */
            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(NpcId);
                br.Write(Damage);
                br.Write(Knockback);
                br.Write(Direction);
                br.Write(Crit);
            }
        }

        #endregion

    }
}

