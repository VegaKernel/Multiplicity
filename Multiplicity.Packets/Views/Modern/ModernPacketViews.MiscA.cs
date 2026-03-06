using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncRevengeMarkerView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public SyncRevengeMarkerView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncRevengeMarker);
            PacketReader reader = packet.CreatePayloadReader();
            int uniqueId = reader.ReadInt32();
            float locationX = reader.ReadSingle();
            float locationY = reader.ReadSingle();
            int npcNetId = reader.ReadInt32();
            float npcHpPercent = reader.ReadSingle();
            int npcTypeAgainstDiscouragement = reader.ReadInt32();
            int npcAiStyleAgainstDiscouragement = reader.ReadInt32();
            int coinsValue = reader.ReadInt32();
            float baseValue = reader.ReadSingle();
            bool spawnedFromStatue = reader.ReadBoolean();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            UniqueId = uniqueId;
            LocationX = locationX;
            LocationY = locationY;
            NpcNetId = npcNetId;
            NpcHpPercent = npcHpPercent;
            NpcTypeAgainstDiscouragement = npcTypeAgainstDiscouragement;
            NpcAiStyleAgainstDiscouragement = npcAiStyleAgainstDiscouragement;
            CoinsValue = coinsValue;
            BaseValue = baseValue;
            SpawnedFromStatue = spawnedFromStatue;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public int UniqueId { get; }
        public float LocationX { get; }
        public float LocationY { get; }
        public int NpcNetId { get; }
        public float NpcHpPercent { get; }
        public int NpcTypeAgainstDiscouragement { get; }
        public int NpcAiStyleAgainstDiscouragement { get; }
        public int CoinsValue { get; }
        public float BaseValue { get; }
        public bool SpawnedFromStatue { get; }
    }

    public readonly ref struct RemoveRevengeMarkerView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public RemoveRevengeMarkerView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.RemoveRevengeMarker);
            PacketReader reader = packet.CreatePayloadReader();
            int uniqueId = reader.ReadInt32();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            UniqueId = uniqueId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public int UniqueId { get; }
    }

    public readonly ref struct LandGolfBallInCupView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public LandGolfBallInCupView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.LandGolfBallInCup);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            ushort cupX = reader.ReadUInt16();
            ushort cupY = reader.ReadUInt16();
            ushort shotX = reader.ReadUInt16();
            ushort shotY = reader.ReadUInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            CupX = cupX;
            CupY = cupY;
            ShotX = shotX;
            ShotY = shotY;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte PlayerId { get; }
        public ushort CupX { get; }
        public ushort CupY { get; }
        public ushort ShotX { get; }
        public ushort ShotY { get; }
    }

    public readonly ref struct FishOutNpcView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public FishOutNpcView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.FishOutNPC);
            PacketReader reader = packet.CreatePayloadReader();
            ushort tileX = reader.ReadUInt16();
            ushort tileY = reader.ReadUInt16();
            short npcNetId = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            TileX = tileX;
            TileY = tileY;
            NpcNetId = npcNetId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public ushort TileX { get; }
        public ushort TileY { get; }
        public short NpcNetId { get; }
    }

    public readonly ref struct TamperWithNpcView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public TamperWithNpcView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TamperWithNPC);
            PacketReader reader = packet.CreatePayloadReader();
            ushort npcId = reader.ReadUInt16();
            TamperWithNpcActionType actionType = (TamperWithNpcActionType)reader.ReadByte();
            int immunityTime = 0;
            short playerId = 0;

            if (actionType == TamperWithNpcActionType.GrantImmunity)
            {
                immunityTime = reader.ReadInt32();
                playerId = reader.ReadInt16();
            }

            reader.EnsureEnd();
            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            NpcId = npcId;
            ActionType = actionType;
            ImmunityTime = immunityTime;
            PlayerId = playerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public ushort NpcId { get; }
        public TamperWithNpcActionType ActionType { get; }
        public int ImmunityTime { get; }
        public short PlayerId { get; }
        public bool HasImmunityPayload => ActionType == TamperWithNpcActionType.GrantImmunity;
    }

    public readonly ref struct PlayLegacySoundView
    {
        private const byte HasStyleFlag = 1 << 0;
        private const byte HasVolumeFlag = 1 << 1;
        private const byte HasPitchOffsetFlag = 1 << 2;

        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayLegacySoundView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayLegacySound);
            PacketReader reader = packet.CreatePayloadReader();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            ushort soundIndex = reader.ReadUInt16();
            byte flags = reader.ReadByte();
            int style = 0;
            float volume = 0f;
            float pitchOffset = 0f;

            if ((flags & HasStyleFlag) != 0)
            {
                style = reader.ReadInt32();
            }

            if ((flags & HasVolumeFlag) != 0)
            {
                volume = reader.ReadSingle();
            }

            if ((flags & HasPitchOffsetFlag) != 0)
            {
                pitchOffset = reader.ReadSingle();
            }

            reader.EnsureEnd();
            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PositionX = positionX;
            PositionY = positionY;
            SoundIndex = soundIndex;
            HasStyle = (flags & HasStyleFlag) != 0;
            HasVolume = (flags & HasVolumeFlag) != 0;
            HasPitchOffset = (flags & HasPitchOffsetFlag) != 0;
            Style = style;
            Volume = volume;
            PitchOffset = pitchOffset;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public float PositionX { get; }
        public float PositionY { get; }
        public ushort SoundIndex { get; }
        public bool HasStyle { get; }
        public int Style { get; }
        public bool HasVolume { get; }
        public float Volume { get; }
        public bool HasPitchOffset { get; }
        public float PitchOffset { get; }
    }

    public readonly ref struct UpdatePlayerLuckFactorsView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public UpdatePlayerLuckFactorsView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.UpdatePlayerLuckFactors);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            int ladyBugLuckTimeLeft = reader.ReadInt32();
            float torchLuck = reader.ReadSingle();
            byte luckPotion = reader.ReadByte();
            bool hasGardenGnomeNearby = reader.ReadBoolean();
            bool brokenMirrorBadLuck = reader.ReadBoolean();
            float equipmentBasedLuckBonus = reader.ReadSingle();
            float coinLuck = reader.ReadSingle();
            byte kiteLuckLevel = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            LadyBugLuckTimeLeft = ladyBugLuckTimeLeft;
            TorchLuck = torchLuck;
            LuckPotion = luckPotion;
            HasGardenGnomeNearby = hasGardenGnomeNearby;
            BrokenMirrorBadLuck = brokenMirrorBadLuck;
            EquipmentBasedLuckBonus = equipmentBasedLuckBonus;
            CoinLuck = coinLuck;
            KiteLuckLevel = kiteLuckLevel;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte PlayerId { get; }
        public int LadyBugLuckTimeLeft { get; }
        public float TorchLuck { get; }
        public byte LuckPotion { get; }
        public bool HasGardenGnomeNearby { get; }
        public bool BrokenMirrorBadLuck { get; }
        public float EquipmentBasedLuckBonus { get; }
        public float CoinLuck { get; }
        public byte KiteLuckLevel { get; }
    }

    public readonly ref struct SyncCavernMonsterTypeView
    {
        private const int Rows = 2;
        private const int Columns = 3;

        private readonly ReadOnlySpan<byte> _packetSpan;
        private readonly ReadOnlySpan<byte> _monsterTypesPayload;

        public SyncCavernMonsterTypeView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncCavernMonsterType);
            PacketReader reader = packet.CreatePayloadReader();
            ReadOnlySpan<byte> monsterTypesPayload = reader.ReadSpan(Rows * Columns * sizeof(ushort));
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            _monsterTypesPayload = monsterTypesPayload;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public ushort GetMonsterType(int row, int column)
        {
            if ((uint)row >= Rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            if ((uint)column >= Columns)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            int offset = ((row * Columns) + column) * sizeof(ushort);
            return BinaryPrimitives.ReadUInt16LittleEndian(_monsterTypesPayload.Slice(offset, sizeof(ushort)));
        }
    }
}
