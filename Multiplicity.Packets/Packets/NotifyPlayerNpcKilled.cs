using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NotifyPlayerNpcKilled (0x61) packet.
    /// </summary>
    public class NotifyPlayerNpcKilled : TerrariaPacket
    {

        public short NpcId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPlayerNpcKilled"/> class.
        /// </summary>
        public NotifyPlayerNpcKilled()
            : base((byte)PacketTypes.NotifyPlayerNpcKilled)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPlayerNpcKilled"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public NotifyPlayerNpcKilled(BinaryReader br)
            : base(br)
        {
            this.NpcId = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[NotifyPlayerNpcKilled: NpcId = {NpcId}]";
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
                br.Write(NpcId);
            }
        }

        #endregion

    }
}

