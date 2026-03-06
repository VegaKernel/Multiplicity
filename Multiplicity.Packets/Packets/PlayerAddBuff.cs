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

        public byte Buff { get; set; }

        public int Time { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAddBuff"/> class.
        /// </summary>
        public PlayerAddBuff()
            : base((byte)PacketTypes.PlayerAddBuff)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAddBuff"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerAddBuff(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.Buff = br.ReadByte();
            this.Time = br.ReadInt32();
        }

        public override string ToString()
        {
            return $"[PlayerAddBuff: PlayerId = {PlayerId} Buff = {Buff} Time = {Time}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(6);
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
                br.Write(PlayerId);
                br.Write(Buff);
                br.Write(Time);
            }
        }

        #endregion

    }
}

