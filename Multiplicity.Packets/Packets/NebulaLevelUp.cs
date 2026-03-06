using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NebulaLevelUp (0x66) packet.
    /// </summary>
    public class NebulaLevelUp : TerrariaPacket
    {
        private const short PayloadLength = 11;

        public byte PlayerId { get; set; }
        public NebulaLevelUpType LevelUpType { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }

        public NebulaLevelUp()
            : base((byte)PacketTypes.NebulaLevelUp)
        {
        }

        public NebulaLevelUp(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            LevelUpType = (NebulaLevelUpType)br.ReadUInt16();
            OriginX = br.ReadSingle();
            OriginY = br.ReadSingle();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(PlayerId);
                bw.Write((ushort)LevelUpType);
                bw.Write(OriginX);
                bw.Write(OriginY);
            }
        }

        public override string ToString()
        {
            return $"[NebulaLevelUp: PlayerId = {PlayerId}, LevelUpType = {LevelUpType}, OriginX = {OriginX}, OriginY = {OriginY}]";
        }
    }
}

