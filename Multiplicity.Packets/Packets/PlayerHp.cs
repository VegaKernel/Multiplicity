using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerHp (0x10) packet.
    /// </summary>
    public class PlayerHp : TerrariaPacket
    {

        public byte PlayerId { get; set; }

        public short Hp { get; set; }

        public short MaxHp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHp"/> class.
        /// </summary>
        public PlayerHp()
            : base((byte)PacketTypes.PlayerHp)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHp"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerHp(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.Hp = br.ReadInt16();
            this.MaxHp = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[PlayerHp: PlayerId = {PlayerId} Hp = {Hp} MaxHp = {MaxHp}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(5);
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
                br.Write(Hp);
                br.Write(MaxHp);
            }
        }

        #endregion

    }
}

