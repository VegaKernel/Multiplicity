using System.IO;

using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class Emoji : TerrariaPacket
    {
        private const short PayloadLength = 2;

        public byte PlayerId { get; set; }

        public byte EmoteId { get; set; }

        public Emoji()
            : base((byte)PacketTypes.Emoji)
        {
        }

        public Emoji(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            EmoteId = br.ReadByte();
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
                bw.Write(EmoteId);
            }
        }
    }

    public sealed class TileEntityDisplayDollItemSync : TerrariaPacket
    {
        private const short BasePayloadLength = 7;
        private const short PosePayloadLength = 1;

        public byte PlayerId { get; set; }

        public int TileEntityId { get; set; }

        public byte ItemIndex { get; set; }

        public DisplayDollSyncCommand Command { get; set; }

        public byte Pose { get; set; }

        public ItemSyncData ItemData { get; set; } = new ItemSyncData();

        public TileEntityDisplayDollItemSync()
            : base((byte)PacketTypes.TileEntityDisplayDollItemSync)
        {
        }

        public TileEntityDisplayDollItemSync(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            TileEntityId = br.ReadInt32();
            ItemIndex = br.ReadByte();
            Command = (DisplayDollSyncCommand)br.ReadByte();

            if (Command == DisplayDollSyncCommand.Pose)
            {
                Pose = br.ReadByte();
                return;
            }

            ItemData = new ItemSyncData(br);
        }

        public override short GetLength()
        {
            return (short)(BasePayloadLength + (Command == DisplayDollSyncCommand.Pose ? PosePayloadLength : ItemSyncData.PayloadLength));
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
                bw.Write(TileEntityId);
                bw.Write(ItemIndex);
                bw.Write((byte)Command);

                if (Command == DisplayDollSyncCommand.Pose)
                {
                    bw.Write(Pose);
                    return;
                }

                (ItemData ?? new ItemSyncData()).ToStream(bw);
            }
        }
    }

    public sealed class RequestTileEntityInteraction : TerrariaPacket
    {
        private const short PayloadLength = 5;

        public int TileEntityId { get; set; }

        public byte PlayerId { get; set; }

        public RequestTileEntityInteraction()
            : base((byte)PacketTypes.RequestTileEntityInteraction)
        {
        }

        public RequestTileEntityInteraction(BinaryReader br)
            : base(br)
        {
            TileEntityId = br.ReadInt32();
            PlayerId = br.ReadByte();
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
                bw.Write(TileEntityId);
                bw.Write(PlayerId);
            }
        }
    }

    public sealed class WeaponsRackTryPlacing : TileEntityItemSyncPacketBase
    {
        public WeaponsRackTryPlacing()
            : base((byte)PacketTypes.WeaponsRackTryPlacing)
        {
        }

        public WeaponsRackTryPlacing(BinaryReader br)
            : base(br)
        {
        }
    }

    public sealed class TileEntityHatRackItemSync : TerrariaPacket
    {
        private const short PayloadLength = 11;

        public byte PlayerId { get; set; }

        public int TileEntityId { get; set; }

        public byte EncodedSlot { get; set; }

        public ItemSyncData ItemData { get; set; } = new ItemSyncData();

        public TileEntityHatRackItemSync()
            : base((byte)PacketTypes.TileEntityHatRackItemSync)
        {
        }

        public TileEntityHatRackItemSync(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            TileEntityId = br.ReadInt32();
            EncodedSlot = br.ReadByte();
            ItemData = new ItemSyncData(br);
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
                bw.Write(TileEntityId);
                bw.Write(EncodedSlot);
                (ItemData ?? new ItemSyncData()).ToStream(bw);
            }
        }
    }

    public sealed class SyncTilePicking : TerrariaPacket
    {
        private const short PayloadLength = 6;

        public byte PlayerId { get; set; }

        public short TileX { get; set; }

        public short TileY { get; set; }

        public byte PickPower { get; set; }

        public SyncTilePicking()
            : base((byte)PacketTypes.SyncTilePicking)
        {
        }

        public SyncTilePicking(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            PickPower = br.ReadByte();
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
                bw.Write(TileX);
                bw.Write(TileY);
                bw.Write(PickPower);
            }
        }
    }

    public sealed class SyncRevengeMarker : TerrariaPacket
    {
        public RevengeMarkerData Marker { get; set; } = new RevengeMarkerData();

        public SyncRevengeMarker()
            : base((byte)PacketTypes.SyncRevengeMarker)
        {
        }

        public SyncRevengeMarker(BinaryReader br)
            : base(br)
        {
            Marker = new RevengeMarkerData(br);
        }

        public override short GetLength()
        {
            return RevengeMarkerData.PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                (Marker ?? new RevengeMarkerData()).ToStream(bw);
            }
        }
    }

    public sealed class RemoveRevengeMarker : TerrariaPacket
    {
        private const short PayloadLength = 4;

        public int UniqueId { get; set; }

        public RemoveRevengeMarker()
            : base((byte)PacketTypes.RemoveRevengeMarker)
        {
        }

        public RemoveRevengeMarker(BinaryReader br)
            : base(br)
        {
            UniqueId = br.ReadInt32();
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
                bw.Write(UniqueId);
            }
        }
    }

    public sealed class LandGolfBallInCup : TerrariaPacket
    {
        private const short PayloadLength = 9;

        public byte PlayerId { get; set; }

        public ushort CupX { get; set; }

        public ushort CupY { get; set; }

        public ushort ShotX { get; set; }

        public ushort ShotY { get; set; }

        public LandGolfBallInCup()
            : base((byte)PacketTypes.LandGolfBallInCup)
        {
        }

        public LandGolfBallInCup(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            CupX = br.ReadUInt16();
            CupY = br.ReadUInt16();
            ShotX = br.ReadUInt16();
            ShotY = br.ReadUInt16();
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
                bw.Write(CupX);
                bw.Write(CupY);
                bw.Write(ShotX);
                bw.Write(ShotY);
            }
        }
    }

    public sealed class FinishedConnectingToServer : TerrariaPacket
    {
        private const short PayloadLength = 0;

        public FinishedConnectingToServer()
            : base((byte)PacketTypes.FinishedConnectingToServer)
        {
        }

        public FinishedConnectingToServer(BinaryReader br)
            : base(br)
        {
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
        }
    }

    public sealed class FishOutNPC : TerrariaPacket
    {
        private const short PayloadLength = 6;

        public ushort TileX { get; set; }

        public ushort TileY { get; set; }

        public short NpcNetId { get; set; }

        public FishOutNPC()
            : base((byte)PacketTypes.FishOutNPC)
        {
        }

        public FishOutNPC(BinaryReader br)
            : base(br)
        {
            TileX = br.ReadUInt16();
            TileY = br.ReadUInt16();
            NpcNetId = br.ReadInt16();
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
                bw.Write(TileX);
                bw.Write(TileY);
                bw.Write(NpcNetId);
            }
        }
    }

    public sealed class TamperWithNPC : TerrariaPacket
    {
        private const short BasePayloadLength = 3;
        private const short ImmunityPayloadLength = 9;

        public ushort NpcId { get; set; }

        public TamperWithNpcActionType ActionType { get; set; }

        public int ImmunityTime { get; set; }

        public short PlayerId { get; set; }

        public TamperWithNPC()
            : base((byte)PacketTypes.TamperWithNPC)
        {
        }

        public TamperWithNPC(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadUInt16();
            ActionType = (TamperWithNpcActionType)br.ReadByte();

            if (ActionType == TamperWithNpcActionType.GrantImmunity)
            {
                ImmunityTime = br.ReadInt32();
                PlayerId = br.ReadInt16();
            }
        }

        public override short GetLength()
        {
            return ActionType == TamperWithNpcActionType.GrantImmunity ? ImmunityPayloadLength : BasePayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(NpcId);
                bw.Write((byte)ActionType);

                if (ActionType == TamperWithNpcActionType.GrantImmunity)
                {
                    bw.Write(ImmunityTime);
                    bw.Write(PlayerId);
                }
            }
        }
    }

    public sealed class PlayLegacySound : TerrariaPacket
    {
        public NetSoundInfoData SoundInfo { get; set; } = new NetSoundInfoData();

        public PlayLegacySound()
            : base((byte)PacketTypes.PlayLegacySound)
        {
        }

        public PlayLegacySound(BinaryReader br)
            : base(br)
        {
            SoundInfo = new NetSoundInfoData(br);
        }

        public override short GetLength()
        {
            return (SoundInfo ?? new NetSoundInfoData()).GetLength();
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                (SoundInfo ?? new NetSoundInfoData()).ToStream(bw);
            }
        }
    }

    public sealed class FoodPlatterTryPlacing : TileEntityItemSyncPacketBase
    {
        public FoodPlatterTryPlacing()
            : base((byte)PacketTypes.FoodPlatterTryPlacing)
        {
        }

        public FoodPlatterTryPlacing(BinaryReader br)
            : base(br)
        {
        }
    }

    public sealed class UpdatePlayerLuckFactors : TerrariaPacket
    {
        private const short PayloadLength = 21;

        public byte PlayerId { get; set; }

        public int LadyBugLuckTimeLeft { get; set; }

        public float TorchLuck { get; set; }

        public byte LuckPotion { get; set; }

        public bool HasGardenGnomeNearby { get; set; }

        public bool BrokenMirrorBadLuck { get; set; }

        public float EquipmentBasedLuckBonus { get; set; }

        public float CoinLuck { get; set; }

        public byte KiteLuckLevel { get; set; }

        public UpdatePlayerLuckFactors()
            : base((byte)PacketTypes.UpdatePlayerLuckFactors)
        {
        }

        public UpdatePlayerLuckFactors(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            LadyBugLuckTimeLeft = br.ReadInt32();
            TorchLuck = br.ReadSingle();
            LuckPotion = br.ReadByte();
            HasGardenGnomeNearby = br.ReadBoolean();
            BrokenMirrorBadLuck = br.ReadBoolean();
            EquipmentBasedLuckBonus = br.ReadSingle();
            CoinLuck = br.ReadSingle();
            KiteLuckLevel = br.ReadByte();
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
                bw.Write(LadyBugLuckTimeLeft);
                bw.Write(TorchLuck);
                bw.Write(LuckPotion);
                bw.Write(HasGardenGnomeNearby);
                bw.Write(BrokenMirrorBadLuck);
                bw.Write(EquipmentBasedLuckBonus);
                bw.Write(CoinLuck);
                bw.Write(KiteLuckLevel);
            }
        }
    }

    public sealed class DeadPlayer : TerrariaPacket
    {
        private const short PayloadLength = 1;

        public byte PlayerId { get; set; }

        public DeadPlayer()
            : base((byte)PacketTypes.DeadPlayer)
        {
        }

        public DeadPlayer(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
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
            }
        }
    }

    public sealed class SyncCavernMonsterType : TerrariaPacket
    {
        private const int VariantRows = 2;
        private const int VariantColumns = 3;
        private const short PayloadLength = VariantRows * VariantColumns * sizeof(ushort);

        public ushort[,] MonsterTypes { get; } = new ushort[VariantRows, VariantColumns];

        public SyncCavernMonsterType()
            : base((byte)PacketTypes.SyncCavernMonsterType)
        {
        }

        public SyncCavernMonsterType(BinaryReader br)
            : base(br)
        {
            for (int row = 0; row < VariantRows; row++)
            {
                for (int column = 0; column < VariantColumns; column++)
                {
                    MonsterTypes[row, column] = br.ReadUInt16();
                }
            }
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
                for (int row = 0; row < VariantRows; row++)
                {
                    for (int column = 0; column < VariantColumns; column++)
                    {
                        bw.Write(MonsterTypes[row, column]);
                    }
                }
            }
        }
    }

    public sealed class RequestNPCBuffRemoval : TerrariaPacket
    {
        private const short PayloadLength = 4;

        public short NpcId { get; set; }

        public ushort BuffId { get; set; }

        public RequestNPCBuffRemoval()
            : base((byte)PacketTypes.RequestNPCBuffRemoval)
        {
        }

        public RequestNPCBuffRemoval(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadInt16();
            BuffId = br.ReadUInt16();
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
                bw.Write(NpcId);
                bw.Write(BuffId);
            }
        }
    }

    public sealed class ClientSyncedInventory : TerrariaPacket
    {
        private const short PayloadLength = 0;

        public ClientSyncedInventory()
            : base((byte)PacketTypes.ClientSyncedInventory)
        {
        }

        public ClientSyncedInventory(BinaryReader br)
            : base(br)
        {
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
        }
    }

    public sealed class SetCountsAsHostForGameplay : TerrariaPacket
    {
        private const short PayloadLength = 2;

        public byte PlayerId { get; set; }

        public bool CountsAsHostForGameplay { get; set; }

        public SetCountsAsHostForGameplay()
            : base((byte)PacketTypes.SetCountsAsHostForGameplay)
        {
        }

        public SetCountsAsHostForGameplay(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            CountsAsHostForGameplay = br.ReadBoolean();
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
                bw.Write(CountsAsHostForGameplay);
            }
        }
    }

    public sealed class SetMiscEventValues : TerrariaPacket
    {
        private const short PayloadLength = 5;

        public MiscEventValueType EventType { get; set; }

        public int Value { get; set; }

        public SetMiscEventValues()
            : base((byte)PacketTypes.SetMiscEventValues)
        {
        }

        public SetMiscEventValues(BinaryReader br)
            : base(br)
        {
            EventType = (MiscEventValueType)br.ReadByte();
            Value = br.ReadInt32();
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
                bw.Write((byte)EventType);
                bw.Write(Value);
            }
        }
    }

    public sealed class RequestLucyPopup : TerrariaPacket
    {
        private const short PayloadLength = 18;

        public LucyPopupMessageSource MessageSource { get; set; }

        public byte Variation { get; set; }

        public float VelocityX { get; set; }

        public float VelocityY { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public RequestLucyPopup()
            : base((byte)PacketTypes.RequestLucyPopup)
        {
        }

        public RequestLucyPopup(BinaryReader br)
            : base(br)
        {
            MessageSource = (LucyPopupMessageSource)br.ReadByte();
            Variation = br.ReadByte();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            PositionX = br.ReadInt32();
            PositionY = br.ReadInt32();
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
                bw.Write((byte)MessageSource);
                bw.Write(Variation);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(PositionX);
                bw.Write(PositionY);
            }
        }
    }

    public sealed class SyncProjectileTrackers : TerrariaPacket
    {
        private const short BasePayloadLength = 1;

        public byte PlayerId { get; set; }

        public TrackedProjectileReferenceData PiggyBankProjectile { get; set; } = new TrackedProjectileReferenceData();

        public TrackedProjectileReferenceData VoidLensChestProjectile { get; set; } = new TrackedProjectileReferenceData();

        public SyncProjectileTrackers()
            : base((byte)PacketTypes.SyncProjectileTrackers)
        {
        }

        public SyncProjectileTrackers(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            PiggyBankProjectile = new TrackedProjectileReferenceData(br);
            VoidLensChestProjectile = new TrackedProjectileReferenceData(br);
        }

        public override short GetLength()
        {
            return (short)(BasePayloadLength +
                (PiggyBankProjectile ?? new TrackedProjectileReferenceData()).GetLength() +
                (VoidLensChestProjectile ?? new TrackedProjectileReferenceData()).GetLength());
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
                (PiggyBankProjectile ?? new TrackedProjectileReferenceData()).ToStream(bw);
                (VoidLensChestProjectile ?? new TrackedProjectileReferenceData()).ToStream(bw);
            }
        }
    }

    public sealed class CrystalInvasionRequestedToSkipWaitTime : TerrariaPacket
    {
        private const short PayloadLength = 0;

        public CrystalInvasionRequestedToSkipWaitTime()
            : base((byte)PacketTypes.CrystalInvasionRequestedToSkipWaitTime)
        {
        }

        public CrystalInvasionRequestedToSkipWaitTime(BinaryReader br)
            : base(br)
        {
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
        }
    }

    public sealed class RequestQuestEffect : TerrariaPacket
    {
        private const short PayloadLength = 0;

        public RequestQuestEffect()
            : base((byte)PacketTypes.RequestQuestEffect)
        {
        }

        public RequestQuestEffect(BinaryReader br)
            : base(br)
        {
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
        }
    }

    public sealed class SyncItemsWithShimmer : WorldItemSyncPacketBase
    {
        public SyncItemsWithShimmer()
            : base((byte)PacketTypes.SyncItemsWithShimmer)
        {
        }

        public SyncItemsWithShimmer(BinaryReader br)
            : base(br)
        {
        }
    }

    public sealed class ShimmerActions : TerrariaPacket
    {
        private const short BasePayloadLength = 9;
        private const short CoinLuckPayloadLength = 13;

        public ShimmerActionKind Action { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public int CoinLuckAmount { get; set; }

        public ShimmerActions()
            : base((byte)PacketTypes.ShimmerActions)
        {
        }

        public ShimmerActions(BinaryReader br)
            : base(br)
        {
            Action = (ShimmerActionKind)br.ReadByte();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();

            if (Action == ShimmerActionKind.AddCoinLuck)
            {
                CoinLuckAmount = br.ReadInt32();
            }
        }

        public override short GetLength()
        {
            return Action == ShimmerActionKind.AddCoinLuck ? CoinLuckPayloadLength : BasePayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write((byte)Action);
                bw.Write(PositionX);
                bw.Write(PositionY);

                if (Action == ShimmerActionKind.AddCoinLuck)
                {
                    bw.Write(CoinLuckAmount);
                }
            }
        }
    }

    public sealed class SyncLoadout : TerrariaPacket
    {
        private const short PayloadLength = 4;

        public byte PlayerId { get; set; }

        public byte LoadoutIndex { get; set; }

        public ushort HiddenAccessoryVisibilityMask { get; set; }

        public SyncLoadout()
            : base((byte)PacketTypes.SyncLoadout)
        {
        }

        public SyncLoadout(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            LoadoutIndex = br.ReadByte();
            HiddenAccessoryVisibilityMask = br.ReadUInt16();
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
                bw.Write(LoadoutIndex);
                bw.Write(HiddenAccessoryVisibilityMask);
            }
        }
    }

    public sealed class SyncItemCannotBeTakenByEnemies : WorldItemSyncPacketBase
    {
        public SyncItemCannotBeTakenByEnemies()
            : base((byte)PacketTypes.SyncItemCannotBeTakenByEnemies)
        {
        }

        public SyncItemCannotBeTakenByEnemies(BinaryReader br)
            : base(br)
        {
        }
    }

    public sealed class TEDeadCellsDisplayJar : TileEntityItemSyncPacketBase
    {
        public TEDeadCellsDisplayJar()
            : base((byte)PacketTypes.TEDeadCellsDisplayJar)
        {
        }

        public TEDeadCellsDisplayJar(BinaryReader br)
            : base(br)
        {
        }
    }

    public sealed class SpectatePlayer : TerrariaPacket
    {
        private const short PayloadLength = 3;

        public byte PlayerId { get; set; }

        public short TargetPlayerId { get; set; }

        public SpectatePlayer()
            : base((byte)PacketTypes.SpectatePlayer)
        {
        }

        public SpectatePlayer(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            TargetPlayerId = br.ReadInt16();
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
                bw.Write(TargetPlayerId);
            }
        }
    }

    public sealed class SyncItemDespawn : TerrariaPacket
    {
        private const short PayloadLength = 2;

        public short ItemId { get; set; }

        public SyncItemDespawn()
            : base((byte)PacketTypes.SyncItemDespawn)
        {
        }

        public SyncItemDespawn(BinaryReader br)
            : base(br)
        {
            ItemId = br.ReadInt16();
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
                bw.Write(ItemId);
            }
        }
    }

    public sealed class ItemUseSound : TerrariaPacket
    {
        private const short PayloadLength = 1;

        public byte PlayerId { get; set; }

        public ItemUseSound()
            : base((byte)PacketTypes.ItemUseSound)
        {
        }

        public ItemUseSound(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
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
            }
        }
    }

    public sealed class NpcHurtByDebuff : TerrariaPacket
    {
        private const short PayloadLength = 3;

        public byte NpcId { get; set; }

        public short Damage { get; set; }

        public NpcHurtByDebuff()
            : base((byte)PacketTypes.NpcHurtByDebuff)
        {
        }

        public NpcHurtByDebuff(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadByte();
            Damage = br.ReadInt16();
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
                bw.Write(NpcId);
                bw.Write(Damage);
            }
        }
    }

    public sealed class Ping : TerrariaPacket
    {
        private const short PayloadLength = 0;

        public Ping()
            : base((byte)PacketTypes.Ping)
        {
        }

        public Ping(BinaryReader br)
            : base(br)
        {
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
        }
    }

    public sealed class TELeashedEntityAnchorPlaceItem : TerrariaPacket
    {
        private const short PayloadLength = 6;

        public short TileX { get; set; }

        public short TileY { get; set; }

        public short ItemType { get; set; }

        public TELeashedEntityAnchorPlaceItem()
            : base((byte)PacketTypes.TELeashedEntityAnchorPlaceItem)
        {
        }

        public TELeashedEntityAnchorPlaceItem(BinaryReader br)
            : base(br)
        {
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            ItemType = br.ReadInt16();
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
                bw.Write(TileX);
                bw.Write(TileY);
                bw.Write(ItemType);
            }
        }
    }

    public sealed class TeamChangeFromUI : TerrariaPacket
    {
        private const short PayloadLength = 2;

        public byte PlayerId { get; set; }

        public byte Team { get; set; }

        public TeamChangeFromUI()
            : base((byte)PacketTypes.TeamChangeFromUI)
        {
        }

        public TeamChangeFromUI(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            Team = br.ReadByte();
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
                bw.Write(Team);
            }
        }
    }

    public sealed class ExtraSpawnSectionLoaded : TerrariaPacket
    {
        private const short PayloadLength = 1;

        public byte PlayerId { get; set; }

        public ExtraSpawnSectionLoaded()
            : base((byte)PacketTypes.ExtraSpawnSectionLoaded)
        {
        }

        public ExtraSpawnSectionLoaded(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
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
            }
        }
    }

    public sealed class RequestSection : TerrariaPacket
    {
        private const short PayloadLength = 4;

        public ushort SectionX { get; set; }

        public ushort SectionY { get; set; }

        public RequestSection()
            : base((byte)PacketTypes.RequestSection)
        {
        }

        public RequestSection(BinaryReader br)
            : base(br)
        {
            SectionX = br.ReadUInt16();
            SectionY = br.ReadUInt16();
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
                bw.Write((short)SectionX);
                bw.Write((short)SectionY);
            }
        }
    }

    public sealed class SyncItemPosition : TerrariaPacket
    {
        private const short PayloadLength = 10;

        public short ItemId { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public SyncItemPosition()
            : base((byte)PacketTypes.SyncItemPosition)
        {
        }

        public SyncItemPosition(BinaryReader br)
            : base(br)
        {
            ItemId = br.ReadInt16();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
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
                bw.Write(ItemId);
                bw.Write(PositionX);
                bw.Write(PositionY);
            }
        }
    }

    public sealed class HostToken : TerrariaPacket
    {
        public string Token { get; set; } = string.Empty;

        public HostToken()
            : base((byte)PacketTypes.HostToken)
        {
        }

        public HostToken(BinaryReader br)
            : base(br)
        {
            Token = br.ReadString();
        }

        public override short GetLength()
        {
            return (short)PacketSerializationHelper.GetSerializedStringLength(Token);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Token ?? string.Empty);
            }
        }
    }

    public sealed class ServerInfo : RawPayloadPacket
    {
        public ServerInfo()
            : base((byte)PacketTypes.ServerInfo)
        {
        }

        public ServerInfo(BinaryReader br)
            : base(br)
        {
        }
    }

    public sealed class PlayerPlatformInfo : RawPayloadPacket
    {
        public PlayerPlatformInfo()
            : base((byte)PacketTypes.PlayerPlatformInfo)
        {
        }

        public PlayerPlatformInfo(BinaryReader br)
            : base(br)
        {
        }
    }

}

