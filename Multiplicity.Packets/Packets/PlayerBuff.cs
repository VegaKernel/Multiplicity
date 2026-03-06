using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerBuff (0x32) packet.
    /// </summary>
    public class PlayerBuff : TerrariaPacket
    {

        public byte PlayerId { get; set; }

        private byte[] _buffType;

        public byte[] BuffType
        {
            get
            {
                return _buffType ??= BuffTypeMemory.ToArray();
            }
            set
            {
                _buffType = value;
                BuffTypeMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> BuffTypeMemory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBuff"/> class.
        /// </summary>
        public PlayerBuff()
            : base((byte)PacketTypes.PlayerBuff)
        {
            BuffTypeMemory = ReadOnlyMemory<byte>.Empty;
            _buffType = Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBuff"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerBuff(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadByte();
            this.BuffTypeMemory = br.ReadMemory(22);
        }

        public override string ToString()
        {
            return $"[PlayerBuff: PlayerId = {PlayerId}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(23);
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
                ReadOnlySpan<byte> buffSpan = BuffTypeMemory.Length == 22 ? BuffTypeMemory.Span : BuffType;
                br.Write(buffSpan);
            }
        }

        #endregion

    }
}

