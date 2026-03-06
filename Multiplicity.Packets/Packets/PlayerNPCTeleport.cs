using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerNPCTeleport (0x41) packet.
    /// </summary>
    public class PlayerNPCTeleport : TerrariaPacket
    {
        private const short FixedPayloadLength = 12;
        private const byte OperationMask = 0b0000_0011;
        private const byte UseCurrentEntityPositionMask = 0b0000_0100;
        private const byte HasExtraInfoMask = 0b0000_1000;

        public PlayerNPCTeleportOperation Operation { get; set; }
        public bool UseCurrentEntityPosition { get; set; }
        public short TargetId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public byte TeleportStyle { get; set; }
        public bool HasExtraInfo { get; set; }
        public int ExtraInfo { get; set; }

        public PlayerNPCTeleport()
            : base((byte)PacketTypes.PlayerNPCTeleport)
        {
        }

        public PlayerNPCTeleport(BinaryReader br)
            : base(br)
        {
            byte flags = br.ReadByte();
            Operation = (PlayerNPCTeleportOperation)(flags & OperationMask);
            UseCurrentEntityPosition = (flags & UseCurrentEntityPositionMask) != 0;
            HasExtraInfo = (flags & HasExtraInfoMask) != 0;

            TargetId = br.ReadInt16();
            X = br.ReadSingle();
            Y = br.ReadSingle();
            TeleportStyle = br.ReadByte();

            if (HasExtraInfo)
            {
                ExtraInfo = br.ReadInt32();
            }
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + (HasExtraInfo ? sizeof(int) : 0));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(BuildFlags());
                bw.Write(TargetId);
                bw.Write(X);
                bw.Write(Y);
                bw.Write(TeleportStyle);

                if (HasExtraInfo)
                {
                    bw.Write(ExtraInfo);
                }
            }
        }

        public override string ToString()
        {
            return $"[PlayerNPCTeleport: Operation = {Operation}, TargetId = {TargetId}, X = {X}, Y = {Y}, TeleportStyle = {TeleportStyle}, UseCurrentEntityPosition = {UseCurrentEntityPosition}, HasExtraInfo = {HasExtraInfo}, ExtraInfo = {ExtraInfo}]";
        }

        private byte BuildFlags()
        {
            byte flags = (byte)((byte)Operation & OperationMask);

            if (UseCurrentEntityPosition)
            {
                flags |= UseCurrentEntityPositionMask;
            }

            if (HasExtraInfo)
            {
                flags |= HasExtraInfoMask;
            }

            return flags;
        }
    }
}
