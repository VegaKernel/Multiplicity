using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetBestiaryModule : TerrariaNetModule
    {
        private const short FixedPayloadLength = sizeof(byte) + sizeof(short);

        public NetBestiaryUnlockType UnlockType { get; set; }

        public short NpcNetId { get; set; }

        public int KillCount { get; set; }

        public NetBestiaryModule()
            : base(NetworkModuleTypes.NetBestiaryModule)
        {
        }

        public NetBestiaryModule(BinaryReader br)
            : base(br)
        {
            UnlockType = (NetBestiaryUnlockType)br.ReadByte();
            NpcNetId = br.ReadInt16();

            if (UnlockType == NetBestiaryUnlockType.Kill)
            {
                KillCount = br.Read7BitEncodedInt();
            }
        }

        public override short GetLength()
        {
            short length = FixedPayloadLength;
            if (UnlockType == NetBestiaryUnlockType.Kill)
            {
                length += (short)PacketSerializationHelper.Get7BitEncodedIntLength(KillCount);
            }

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write((byte)UnlockType);
            bw.Write(NpcNetId);
            if (UnlockType == NetBestiaryUnlockType.Kill)
            {
                bw.Write7BitEncodedInt(KillCount);
            }
        }
    }
}