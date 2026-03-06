using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcTalk (0x28) packet.
    /// </summary>
    public class NpcTalk : TerrariaPacket
    {

        public byte PlayerId { get; set; }

        public short NpcTalkTarget { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcTalk"/> class.
        /// </summary>
        public NpcTalk()
            : base((byte)PacketTypes.NpcTalk)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcTalk"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public NpcTalk(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.NpcTalkTarget = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[NpcTalk: PlayerId = {PlayerId} NpcTalkTarget = {NpcTalkTarget}]";
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
                br.Write(PlayerId);
                br.Write(NpcTalkTarget);
            }
        }

        #endregion

    }
}

