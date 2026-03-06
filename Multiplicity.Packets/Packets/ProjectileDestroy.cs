using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ProjectileDestroy (0x1D) packet.
    /// </summary>
    public class ProjectileDestroy : TerrariaPacket
    {

        public short ProjectileId { get; set; }

        /// <summary>
        /// Gets or sets the Owner - Player Id|
        /// </summary>
        public byte Owner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectileDestroy"/> class.
        /// </summary>
        public ProjectileDestroy()
            : base((byte)PacketTypes.ProjectileDestroy)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectileDestroy"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public ProjectileDestroy(BinaryReader br)
            : base(br)
        {
            this.ProjectileId = br.ReadInt16();
            this.Owner = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[ProjectileDestroy: ProjectileId = {ProjectileId} Owner = {Owner}]";
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
                br.Write(ProjectileId);
                br.Write(Owner);
            }
        }

        #endregion

    }
}

