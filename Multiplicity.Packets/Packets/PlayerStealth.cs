using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerStealth (0x54) packet.
    /// </summary>
    public class PlayerStealth : TerrariaPacket
    {

        public byte Player { get; set; }

        public float Stealth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStealth"/> class.
        /// </summary>
        public PlayerStealth()
            : base((byte)PacketTypes.PlayerStealth)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStealth"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerStealth(BinaryReader br)
            : base(br)
        {
            this.Player = br.ReadByte();
            this.Stealth = br.ReadSingle();
        }

        public override string ToString()
        {
            return $"[PlayerStealth: Player = {Player} Stealth = {Stealth}]";
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
                br.Write(Player);
                br.Write(Stealth);
            }
        }

        #endregion

    }
}
