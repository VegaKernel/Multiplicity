using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PoofOfSmoke (0x6A) packet.
    /// </summary>
    public class PoofOfSmoke : TerrariaPacket
    {

        /// <summary>
        /// Gets or sets the PackedVector - Two Int16's packed into 4 bytes.|
        /// </summary>
        public int PackedVector { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoofOfSmoke"/> class.
        /// </summary>
        public PoofOfSmoke()
            : base((byte)PacketTypes.PoofOfSmoke)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoofOfSmoke"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PoofOfSmoke(BinaryReader br)
            : base(br)
        {
            this.PackedVector = br.ReadInt32();
        }

        public override string ToString()
        {
            return $"[PoofOfSmoke: PackedVector = {PackedVector}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(4);
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
                br.Write(PackedVector);
            }
        }

        #endregion

    }
}

