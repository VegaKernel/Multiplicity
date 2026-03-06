using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct DeadPlayerView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public DeadPlayerView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.DeadPlayer);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
    }

    public readonly ref struct RequestNPCBuffRemovalView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public RequestNPCBuffRemovalView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.RequestNPCBuffRemoval);
            PacketReader reader = packet.CreatePayloadReader();
            short npcId = reader.ReadInt16();
            ushort buffId = reader.ReadUInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            NpcId = npcId;
            BuffId = buffId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public short NpcId { get; }
        public ushort BuffId { get; }
    }

    public readonly ref struct SetCountsAsHostForGameplayView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SetCountsAsHostForGameplayView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SetCountsAsHostForGameplay);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            bool countsAsHostForGameplay = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            CountsAsHostForGameplay = countsAsHostForGameplay;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public bool CountsAsHostForGameplay { get; }
    }

    public readonly ref struct SetMiscEventValuesView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SetMiscEventValuesView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SetMiscEventValues);
            PacketReader reader = packet.CreatePayloadReader();
            MiscEventValueType eventType = (MiscEventValueType)reader.ReadByte();
            int value = reader.ReadInt32();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            EventType = eventType;
            Value = value;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public MiscEventValueType EventType { get; }
        public int Value { get; }
    }

    public readonly ref struct RequestLucyPopupView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public RequestLucyPopupView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.RequestLucyPopup);
            PacketReader reader = packet.CreatePayloadReader();
            LucyPopupMessageSource messageSource = (LucyPopupMessageSource)reader.ReadByte();
            byte variation = reader.ReadByte();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            int positionX = reader.ReadInt32();
            int positionY = reader.ReadInt32();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            MessageSource = messageSource;
            Variation = variation;
            VelocityX = velocityX;
            VelocityY = velocityY;
            PositionX = positionX;
            PositionY = positionY;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public LucyPopupMessageSource MessageSource { get; }
        public byte Variation { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public int PositionX { get; }
        public int PositionY { get; }
    }

    public readonly ref struct SyncProjectileTrackersView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SyncProjectileTrackersView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncProjectileTrackers);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short piggyBankOwnerIndex = reader.ReadInt16();
            short piggyBankIdentity = 0;
            short piggyBankProjectileType = 0;

            if (piggyBankOwnerIndex != -1)
            {
                piggyBankIdentity = reader.ReadInt16();
                piggyBankProjectileType = reader.ReadInt16();
            }

            short voidLensChestOwnerIndex = reader.ReadInt16();
            short voidLensChestIdentity = 0;
            short voidLensChestProjectileType = 0;

            if (voidLensChestOwnerIndex != -1)
            {
                voidLensChestIdentity = reader.ReadInt16();
                voidLensChestProjectileType = reader.ReadInt16();
            }

            reader.EnsureEnd();
            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            PiggyBankOwnerIndex = piggyBankOwnerIndex;
            PiggyBankIdentity = piggyBankIdentity;
            PiggyBankProjectileType = piggyBankProjectileType;
            VoidLensChestOwnerIndex = voidLensChestOwnerIndex;
            VoidLensChestIdentity = voidLensChestIdentity;
            VoidLensChestProjectileType = voidLensChestProjectileType;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public short PiggyBankOwnerIndex { get; }
        public short PiggyBankIdentity { get; }
        public short PiggyBankProjectileType { get; }
        public short VoidLensChestOwnerIndex { get; }
        public short VoidLensChestIdentity { get; }
        public short VoidLensChestProjectileType { get; }
        public bool HasPiggyBankProjectile => PiggyBankOwnerIndex != -1;
        public bool HasVoidLensChestProjectile => VoidLensChestOwnerIndex != -1;
    }

    public readonly ref struct ShimmerActionsView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public ShimmerActionsView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ShimmerActions);
            PacketReader reader = packet.CreatePayloadReader();
            ShimmerActionKind action = (ShimmerActionKind)reader.ReadByte();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            int coinLuckAmount = 0;

            if (action == ShimmerActionKind.AddCoinLuck)
            {
                coinLuckAmount = reader.ReadInt32();
            }

            reader.EnsureEnd();
            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            Action = action;
            PositionX = positionX;
            PositionY = positionY;
            CoinLuckAmount = coinLuckAmount;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public ShimmerActionKind Action { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public int CoinLuckAmount { get; }
        public bool HasCoinLuckAmount => Action == ShimmerActionKind.AddCoinLuck;
    }

    public readonly ref struct SyncLoadoutView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SyncLoadoutView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncLoadout);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte loadoutIndex = reader.ReadByte();
            ushort hiddenAccessoryVisibilityMask = reader.ReadUInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            LoadoutIndex = loadoutIndex;
            HiddenAccessoryVisibilityMask = hiddenAccessoryVisibilityMask;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public byte LoadoutIndex { get; }
        public ushort HiddenAccessoryVisibilityMask { get; }
    }
}



