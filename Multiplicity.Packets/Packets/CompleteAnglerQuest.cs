using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The CompleteAnglerQuest (0x4B) packet.
    /// </summary>
    public class CompleteAnglerQuest : TerrariaPacket
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteAnglerQuest"/> class.
        /// </summary>
        public CompleteAnglerQuest()
            : base((byte)PacketTypes.CompleteAnglerQuest)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteAnglerQuest"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public CompleteAnglerQuest(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return $"[CompleteAnglerQuest:]";
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
