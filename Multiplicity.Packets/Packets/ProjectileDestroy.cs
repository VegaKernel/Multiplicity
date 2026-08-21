using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ProjectileDestroy (0x1D) packet for protocol 325.
    /// </summary>
    public class ProjectileDestroy : TerrariaPacket
    {
        public ProjectileKey Key { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public ProjectileDestroy()
            : base((byte)PacketTypes.ProjectileDestroy)
        {
        }

        public ProjectileDestroy(BinaryReader br)
            : base(br)
        {
            Key = ProjectileKey.FromPacked(br.ReadInt32());
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
        }

        public override string ToString() =>
            $"[ProjectileDestroy: Key = {Key} PositionX = {PositionX} PositionY = {PositionY}]";

        public override short GetLength() => 12;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Key.ToInt32());
                bw.Write(PositionX);
                bw.Write(PositionY);
            }
        }
    }
}
