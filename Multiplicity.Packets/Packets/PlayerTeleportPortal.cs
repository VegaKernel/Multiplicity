using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerTeleportPortal (0x60) packet.
    /// </summary>
    public class PlayerTeleportPortal : TerrariaPacket
    {

        public byte PlayerId { get; set; }

        public short PortalColorIndex { get; set; }

        public float NewPositionX { get; set; }

        public float NewPositionY { get; set; }

        public float VelocityX { get; set; }

        public float VelocityY { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTeleportPortal"/> class.
        /// </summary>
        public PlayerTeleportPortal()
            : base((byte)PacketTypes.PlayerTeleportPortal)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTeleportPortal"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerTeleportPortal(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.PortalColorIndex = br.ReadInt16();
            this.NewPositionX = br.ReadSingle();
            this.NewPositionY = br.ReadSingle();
            this.VelocityX = br.ReadSingle();
            this.VelocityY = br.ReadSingle();
        }

        public override string ToString()
        {
            return $"[PlayerTeleportPortal: PlayerId = {PlayerId} PortalColorIndex = {PortalColorIndex} NewPositionX = {NewPositionX} NewPositionY = {NewPositionY} VelocityX = {VelocityX} VelocityY = {VelocityY}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(19);
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
                br.Write(PortalColorIndex);
                br.Write(NewPositionX);
                br.Write(NewPositionY);
                br.Write(VelocityX);
                br.Write(VelocityY);
            }
        }

        #endregion

    }
}

