using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcUpdateBuff (0x36) packet.
    /// </summary>
    public class NpcUpdateBuff : TerrariaPacket
    {

        public short NpcId { get; set; }

        public byte BuffId { get; set; }

        public short Time { get; set; }

        public byte BuffId2 { get; set; }

        public short Time2 { get; set; }

        public byte BuffId3 { get; set; }

        public short Time3 { get; set; }

        public byte BuffId4 { get; set; }

        public short Time4 { get; set; }

        public byte BuffId5 { get; set; }

        public short Time5 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcUpdateBuff"/> class.
        /// </summary>
        public NpcUpdateBuff()
            : base((byte)PacketTypes.NpcUpdateBuff)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcUpdateBuff"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public NpcUpdateBuff(BinaryReader br)
            : base(br)
        {
            this.NpcId = br.ReadInt16();
            this.BuffId = br.ReadByte();
            this.Time = br.ReadInt16();
            this.BuffId2 = br.ReadByte();
            this.Time2 = br.ReadInt16();
            this.BuffId3 = br.ReadByte();
            this.Time3 = br.ReadInt16();
            this.BuffId4 = br.ReadByte();
            this.Time4 = br.ReadInt16();
            this.BuffId5 = br.ReadByte();
            this.Time5 = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[NpcUpdateBuff: NpcId = {NpcId} BuffId = {BuffId} Time = {Time} BuffId2 = {BuffId2} Time2 = {Time2} BuffId3 = {BuffId3} Time3 = {Time3} BuffId4 = {BuffId4} Time4 = {Time4} BuffId5 = {BuffId5} Time5 = {Time5}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(17);
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
                br.Write(BuffId);
                br.Write(Time);
                br.Write(BuffId2);
                br.Write(Time2);
                br.Write(BuffId3);
                br.Write(Time3);
                br.Write(BuffId4);
                br.Write(Time4);
                br.Write(BuffId5);
                br.Write(Time5);
            }
        }

        #endregion

    }
}

