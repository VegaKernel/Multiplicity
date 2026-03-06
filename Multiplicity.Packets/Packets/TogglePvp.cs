using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The TogglePvp (0x1E) packet.
    /// </summary>
    public class TogglePvp : TerrariaPacket
    {

        public byte PlayerId { get; set; }

        public bool PvpEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TogglePvp"/> class.
        /// </summary>
        public TogglePvp()
            : base((byte)PacketTypes.TogglePvp)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TogglePvp"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public TogglePvp(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.PvpEnabled = br.ReadBoolean();
        }

        public override string ToString()
        {
            return $"[TogglePvp: PlayerId = {PlayerId} PvpEnabled = {PvpEnabled}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(2);
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
                br.Write(PvpEnabled);
            }
        }

        #endregion

    }
}

