using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerSpawn (0x0C) packet for Terraria protocol 325.
    /// </summary>
    public class PlayerSpawn : TerrariaPacket
    {
        public const short PayloadLength = 15;

        public byte PlayerId { get; set; }

        public short SpawnX { get; set; }

        public short SpawnY { get; set; }

        public int RespawnTimer { get; set; }

        public short NumberOfDeathsPVE { get; set; }

        public short NumberOfDeathsPVP { get; set; }

        public byte Team { get; set; }

        /// <summary>Raw Terraria PlayerSpawnContext value.</summary>
        public byte SpawnContext { get; set; }

        public PlayerSpawn()
            : base((byte)PacketTypes.PlayerSpawn)
        {
        }

        public PlayerSpawn(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            SpawnX = br.ReadInt16();
            SpawnY = br.ReadInt16();
            RespawnTimer = br.ReadInt32();
            NumberOfDeathsPVE = br.ReadInt16();
            NumberOfDeathsPVP = br.ReadInt16();
            Team = br.ReadByte();
            SpawnContext = br.ReadByte();
        }

        public override string ToString() =>
            $"[PlayerSpawn: PlayerId = {PlayerId} SpawnX = {SpawnX} SpawnY = {SpawnY} RespawnTimer = {RespawnTimer} " +
            $"DeathsPVE = {NumberOfDeathsPVE} DeathsPVP = {NumberOfDeathsPVP} Team = {Team} Context = {SpawnContext}]";

        public override short GetLength() => PayloadLength;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter writer = new BinaryWriter(
                stream,
                new System.Text.UTF8Encoding(),
                leaveOpen: true);
            writer.Write(PlayerId);
            writer.Write(SpawnX);
            writer.Write(SpawnY);
            writer.Write(RespawnTimer);
            writer.Write(NumberOfDeathsPVE);
            writer.Write(NumberOfDeathsPVP);
            writer.Write(Team);
            writer.Write(SpawnContext);
        }
    }
}
