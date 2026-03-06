using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ChestName (0x45) packet.
    /// </summary>
    public class ChestName : TerrariaPacket
    {
        private const short FixedPayloadLength = 6;

        public short ChestId { get; set; }

        public short ChestX { get; set; }

        public short ChestY { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChestName"/> class.
        /// </summary>
        public ChestName()
            : base((byte)PacketTypes.ChestName)
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChestName"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public ChestName(BinaryReader br)
            : base(br)
        {
            this.ChestId = br.ReadInt16();
            this.ChestX = br.ReadInt16();
            this.ChestY = br.ReadInt16();
            this.Name = br.ReadString();
        }

        public override string ToString()
        {
            return $"[ChestName: ChestId = {ChestId} ChestX = {ChestX} ChestY = {ChestY} Name = {Name}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(Name));
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
                br.Write(ChestId);
                br.Write(ChestX);
                br.Write(ChestY);
                br.Write(Name ?? string.Empty);
            }
        }

        #endregion

    }
}

