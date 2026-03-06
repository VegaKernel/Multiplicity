using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SpectatePlayerView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SpectatePlayerView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SpectatePlayer);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short targetPlayerId = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            TargetPlayerId = targetPlayerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public short TargetPlayerId { get; }
    }

    public readonly ref struct SyncItemDespawnView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SyncItemDespawnView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncItemDespawn);
            PacketReader reader = packet.CreatePayloadReader();
            short itemId = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            ItemId = itemId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public short ItemId { get; }
    }

    public readonly ref struct ItemUseSoundView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public ItemUseSoundView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ItemUseSound);
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

    public readonly ref struct NpcHurtByDebuffView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NpcHurtByDebuffView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.NpcHurtByDebuff);
            PacketReader reader = packet.CreatePayloadReader();
            byte npcId = reader.ReadByte();
            short damage = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            NpcId = npcId;
            Damage = damage;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte NpcId { get; }
        public short Damage { get; }
    }

    public readonly ref struct TELeashedEntityAnchorPlaceItemView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TELeashedEntityAnchorPlaceItemView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TELeashedEntityAnchorPlaceItem);
            PacketReader reader = packet.CreatePayloadReader();
            short tileX = reader.ReadInt16();
            short tileY = reader.ReadInt16();
            short itemType = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            TileX = tileX;
            TileY = tileY;
            ItemType = itemType;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public short TileX { get; }
        public short TileY { get; }
        public short ItemType { get; }
    }

    public readonly ref struct TeamChangeFromUIView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TeamChangeFromUIView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TeamChangeFromUI);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte team = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            Team = team;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public byte Team { get; }
    }

    public readonly ref struct ExtraSpawnSectionLoadedView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public ExtraSpawnSectionLoadedView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ExtraSpawnSectionLoaded);
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

    public readonly ref struct RequestSectionView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public RequestSectionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.RequestSection);
            PacketReader reader = packet.CreatePayloadReader();
            ushort sectionX = reader.ReadUInt16();
            ushort sectionY = reader.ReadUInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            SectionX = sectionX;
            SectionY = sectionY;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public ushort SectionX { get; }
        public ushort SectionY { get; }
    }

    public readonly ref struct SyncItemPositionView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SyncItemPositionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncItemPosition);
            PacketReader reader = packet.CreatePayloadReader();
            short itemId = reader.ReadInt16();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            ItemId = itemId;
            PositionX = positionX;
            PositionY = positionY;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public short ItemId { get; }
        public float PositionX { get; }
        public float PositionY { get; }
    }
}



