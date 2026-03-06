using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PasswordRequired (0x25) packet.
    /// </summary>
    public class PasswordRequired : TerrariaPacket
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordRequired"/> class.
        /// </summary>
        public PasswordRequired()
            : base((byte)PacketTypes.PasswordRequired)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordRequired"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PasswordRequired(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return $"[PasswordRequired:]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(0);
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
            }
        }

        #endregion

    }
}
