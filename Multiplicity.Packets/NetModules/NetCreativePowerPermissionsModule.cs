using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetCreativePowerPermissionsModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(byte) + sizeof(ushort) + sizeof(byte);

        public NetCreativePowerPermissionsMessageType MessageType { get; set; } =
            NetCreativePowerPermissionsMessageType.SetPermissionLevel;

        public ushort PowerId { get; set; }

        public byte PermissionLevel { get; set; }

        public NetCreativePowerPermissionsModule()
            : base(NetworkModuleTypes.NetCreativePowerPermissionsModule)
        {
        }

        public NetCreativePowerPermissionsModule(BinaryReader br)
            : base(br)
        {
            MessageType = (NetCreativePowerPermissionsMessageType)br.ReadByte();
            PowerId = br.ReadUInt16();
            PermissionLevel = br.ReadByte();
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
            bw.Write(PowerId);
            bw.Write(PermissionLevel);
        }
    }
}