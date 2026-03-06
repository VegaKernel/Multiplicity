using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetTeleportPylonModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(byte) + sizeof(short) + sizeof(short) + sizeof(byte);

        public NetTeleportPylonMessageType MessageType { get; set; }

        public short TileX { get; set; }

        public short TileY { get; set; }

        public byte PylonType { get; set; }

        public NetTeleportPylonModule()
            : base(NetworkModuleTypes.NetTeleportPylonModule)
        {
        }

        public NetTeleportPylonModule(BinaryReader br)
            : base(br)
        {
            MessageType = (NetTeleportPylonMessageType)br.ReadByte();
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            PylonType = br.ReadByte();
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

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write((byte)MessageType);
            bw.Write(TileX);
            bw.Write(TileY);
            bw.Write(PylonType);
        }
    }
}