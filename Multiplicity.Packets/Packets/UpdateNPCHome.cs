using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateNPCHome (0x3C) packet.
    /// </summary>
    public class UpdateNPCHome : TerrariaPacket
    {

        public short NpcId { get; set; }

        public short HomeTileX { get; set; }

        public short HomeTileY { get; set; }

        public byte Homeless { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNPCHome"/> class.
        /// </summary>
        public UpdateNPCHome()
            : base((byte)PacketTypes.UpdateNPCHome)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNPCHome"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public UpdateNPCHome(BinaryReader br)
            : base(br)
        {
            this.NpcId = br.ReadInt16();
            this.HomeTileX = br.ReadInt16();
            this.HomeTileY = br.ReadInt16();
            this.Homeless = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[UpdateNPCHome: NpcId = {NpcId} HomeTileX = {HomeTileX} HomeTileY = {HomeTileY} Homeless = {Homeless}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(7);
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
                br.Write(HomeTileX);
                br.Write(HomeTileY);
                br.Write(Homeless);
            }
        }

        #endregion

    }
}

