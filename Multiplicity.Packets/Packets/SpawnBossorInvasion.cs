using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SpawnBossorInvasion (0x3D) packet.
    /// </summary>
    public class SpawnBossorInvasion : TerrariaPacket
    {

        public short PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the Type - Negative Values: -1 = GoblinInvasion, -2 = FrostInvasion, -3 = PirateInvasion, -4 = PumpkinMoon, -5 = SnowMoon, -6 = Eclipse, -7 = Martian Moon Positive Values: Spawns any of these Npcs:4,13,50,126,125,134,127,128,131,129,130,222,245,266,370,75,398,439,493,507,422,517|
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnBossorInvasion"/> class.
        /// </summary>
        public SpawnBossorInvasion()
            : base((byte)PacketTypes.SpawnBossorInvasion)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnBossorInvasion"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public SpawnBossorInvasion(BinaryReader br)
            : base(br)
        {
            this.PlayerId = br.ReadInt16();
            this.Type = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[SpawnBossorInvasion: PlayerId = {PlayerId} Type = {Type}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(4);
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
                br.Write(Type);
            }
        }

        #endregion

    }
}

