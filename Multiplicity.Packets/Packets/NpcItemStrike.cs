using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcItemStrike (0x18) packet.
    /// </summary>
    public class NpcItemStrike : TerrariaPacket
    {

        public short NpcId { get; set; }

        public byte PlayerId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcItemStrike"/> class.
        /// </summary>
        public NpcItemStrike()
            : base((byte)PacketTypes.NpcItemStrike)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcItemStrike"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public NpcItemStrike(BinaryReader br)
            : base(br)
        {
            this.NpcId = br.ReadInt16();
            this.PlayerId = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[NpcItemStrike: NpcId = {NpcId} PlayerId = {PlayerId}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(3);
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
                br.Write(PlayerId);
            }
        }

        #endregion

    }
}

