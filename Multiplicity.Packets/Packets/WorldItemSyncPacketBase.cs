using System.IO;

namespace Multiplicity.Packets
{
    public abstract class WorldItemSyncPacketBase : TerrariaPacket
    {
        // Named FixedPayloadLength deliberately: TerrariaPacket metadata treats packets
        // that expose this marker as variable-length and parses them through the object model.
        protected const short FixedPayloadLength = 24;

        public short ItemIndex { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public short Stack { get; set; }
        public byte Prefix { get; set; }
        public WorldItemSyncFlags Flags { get; set; }
        public short ItemNetId { get; set; }
        public bool Shimmered { get; set; }
        public float ShimmerTime { get; set; }
        public byte EnemyGrabDelayTime { get; set; }

        public NewItemOwnership Ownership
        {
            get => (NewItemOwnership)((byte)Flags & (byte)WorldItemSyncFlags.OwnershipMask);
            set => Flags = (WorldItemSyncFlags)(((byte)Flags & ~(byte)WorldItemSyncFlags.OwnershipMask) | ((byte)value & 0x03));
        }

        protected WorldItemSyncPacketBase(byte id)
            : base(id)
        {
        }

        protected WorldItemSyncPacketBase(BinaryReader br)
            : base(br)
        {
            ItemIndex = br.ReadInt16();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            Stack = br.ReadInt16();
            Prefix = br.ReadByte();
            Flags = (WorldItemSyncFlags)br.ReadByte();
            ItemNetId = br.ReadInt16();

            if ((Flags & WorldItemSyncFlags.HasShimmerData) != 0)
            {
                Shimmered = br.ReadBoolean();
                ShimmerTime = br.ReadSingle();
            }

            if ((Flags & WorldItemSyncFlags.HasEnemyGrabDelay) != 0)
                EnemyGrabDelayTime = br.ReadByte();
        }

        public override short GetLength()
        {
            WorldItemSyncFlags flags = BuildFlags();
            short length = FixedPayloadLength;

            if ((flags & WorldItemSyncFlags.HasShimmerData) != 0)
                length += sizeof(byte) + sizeof(float);
            if ((flags & WorldItemSyncFlags.HasEnemyGrabDelay) != 0)
                length += sizeof(byte);

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            WorldItemSyncFlags flags = BuildFlags();

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(ItemIndex);
                bw.Write(PositionX);
                bw.Write(PositionY);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(Stack);
                bw.Write(Prefix);
                bw.Write((byte)flags);
                bw.Write(ItemNetId);

                if ((flags & WorldItemSyncFlags.HasShimmerData) != 0)
                {
                    bw.Write(Shimmered);
                    bw.Write(ShimmerTime);
                }

                if ((flags & WorldItemSyncFlags.HasEnemyGrabDelay) != 0)
                    bw.Write(EnemyGrabDelayTime);
            }
        }

        private WorldItemSyncFlags BuildFlags()
        {
            WorldItemSyncFlags flags = Flags;

            if (Shimmered || ShimmerTime != 0f)
                flags |= WorldItemSyncFlags.HasShimmerData;
            if (EnemyGrabDelayTime != 0)
                flags |= WorldItemSyncFlags.HasEnemyGrabDelay;

            return flags;
        }
    }
}
