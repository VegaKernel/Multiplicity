using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerAnimation (0x29) packet.
    /// </summary>
    public class PlayerAnimation : TerrariaPacket
    {

        public byte PlayerId { get; set; }

        public float ItemRotation { get; set; }

        public short ItemAnimation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAnimation"/> class.
        /// </summary>
        public PlayerAnimation()
            : base((byte)PacketTypes.PlayerAnimation)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAnimation"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerAnimation(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.ItemRotation = br.ReadSingle();
            this.ItemAnimation = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[PlayerAnimation: PlayerId = {PlayerId} ItemRotation = {ItemRotation} ItemAnimation = {ItemAnimation}]";
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
                br.Write(PlayerId);
                br.Write(ItemRotation);
                br.Write(ItemAnimation);
            }
        }

        #endregion

    }
}

