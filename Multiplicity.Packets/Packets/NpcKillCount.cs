using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcKillCount (0x53) packet.
    /// </summary>
    public class NpcKillCount : TerrariaPacket
    {

        public short NpcType { get; set; }

        public int KillCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcKillCount"/> class.
        /// </summary>
        public NpcKillCount()
            : base((byte)PacketTypes.NpcKillCount)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcKillCount"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public NpcKillCount(BinaryReader br)
            : base(br)
        {
            this.NpcType = br.ReadInt16();
            this.KillCount = br.ReadInt32();
        }

        public override string ToString()
        {
            return $"[NpcKillCount: NpcType = {NpcType} KillCount = {KillCount}]";
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
                br.Write(NpcType);
                br.Write(KillCount);
            }
        }

        #endregion

    }
}

