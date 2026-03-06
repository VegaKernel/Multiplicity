using System;
using System.IO;

namespace Multiplicity.Packets
{
    public struct WorldSpawnPoint
    {
        public short X;
        public short Y;

        public WorldSpawnPoint(short x, short y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// The WorldInfo (0x07) packet for protocol 317/318.
    /// </summary>
    public class WorldInfo : TerrariaPacket
    {
        private const int WorldUniqueIdLength = 16;
        private const int TreeTopVariationCount = 13;
        private const int MaximumExtraSpawnPoints = byte.MaxValue;

        public int Time { get; set; }
        public byte DayandMoonInfo { get; set; }
        public byte MoonPhase { get; set; }
        public short MaxTilesX { get; set; }
        public short MaxTilesY { get; set; }
        public short SpawnX { get; set; }
        public short SpawnY { get; set; }
        public short WorldSurface { get; set; }
        public short RockLayer { get; set; }
        public int WorldID { get; set; }
        public string WorldName { get; set; }
        public byte GameMode { get; set; }
        private byte[] _worldUniqueId;
        public byte[] WorldUniqueID
        {
            get
            {
                return _worldUniqueId ??= WorldUniqueIdMemory.ToArray();
            }
            set
            {
                _worldUniqueId = value;
                WorldUniqueIdMemory = value ?? Array.Empty<byte>();
            }
        }
        public ReadOnlyMemory<byte> WorldUniqueIdMemory { get; set; }
        public ulong WorldGeneratorVersion { get; set; }
        public byte MoonType { get; set; }

        public byte TreeBackground { get; set; }
        public byte TreeBackground2 { get; set; }
        public byte TreeBackground3 { get; set; }
        public byte TreeBackground4 { get; set; }
        public byte CorruptionBackground { get; set; }
        public byte JungleBackground { get; set; }
        public byte SnowBackground { get; set; }
        public byte HallowBackground { get; set; }
        public byte CrimsonBackground { get; set; }
        public byte DesertBackground { get; set; }
        public byte OceanBackground { get; set; }
        public byte MushroomBackground { get; set; }
        public byte UnderworldBackground { get; set; }

        public byte IceBackStyle { get; set; }
        public byte JungleBackStyle { get; set; }
        public byte HellBackStyle { get; set; }

        public float WindSpeedSet { get; set; }
        public byte CloudNumber { get; set; }

        public int Tree1 { get; set; }
        public int Tree2 { get; set; }
        public int Tree3 { get; set; }
        public byte TreeStyle1 { get; set; }
        public byte TreeStyle2 { get; set; }
        public byte TreeStyle3 { get; set; }
        public byte TreeStyle4 { get; set; }

        public int CaveBack1 { get; set; }
        public int CaveBack2 { get; set; }
        public int CaveBack3 { get; set; }
        public byte CaveBackStyle1 { get; set; }
        public byte CaveBackStyle2 { get; set; }
        public byte CaveBackStyle3 { get; set; }
        public byte CaveBackStyle4 { get; set; }

        // WorldGen.TreeTops.SyncSend sends exactly 13 bytes in protocol 317/318.
        private byte[] _treeTopVariations;
        public byte[] TreeTopVariations
        {
            get
            {
                return _treeTopVariations ??= TreeTopVariationsMemory.ToArray();
            }
            set
            {
                _treeTopVariations = value;
                TreeTopVariationsMemory = value ?? Array.Empty<byte>();
            }
        }
        public ReadOnlyMemory<byte> TreeTopVariationsMemory { get; set; }

        public float Rain { get; set; }

        // Event flags blocks (BitsByte 6..16 in NetMessage.SendData(7)).
        public byte EventInfo { get; set; }
        public byte EventInfo2 { get; set; }
        public byte EventInfo3 { get; set; }
        public byte EventInfo4 { get; set; }
        public byte EventInfo5 { get; set; }
        public byte EventInfo6 { get; set; }
        public byte EventInfo7 { get; set; }
        public byte EventInfo8 { get; set; }
        public byte EventInfo9 { get; set; }
        public byte EventInfo10 { get; set; }
        public byte EventInfo11 { get; set; }

        public byte SunDialCooldown { get; set; }
        public byte MoonDialCooldown { get; set; }

        public short OreTierCopper { get; set; }
        public short OreTierIron { get; set; }
        public short OreTierSilver { get; set; }
        public short OreTierGold { get; set; }
        public short OreTierCobalt { get; set; }
        public short OreTierMythril { get; set; }
        public short OreTierAdamantite { get; set; }

        public sbyte InvasionType { get; set; }
        public ulong LobbyID { get; set; }
        public float SandstormSeverity { get; set; }

        public WorldSpawnPoint[] ExtraSpawnPoints { get; set; }

        // Keep any unknown tail bytes for forward compatibility.
        private byte[] _trailingData;
        public byte[] TrailingData
        {
            get
            {
                return _trailingData ??= TrailingDataMemory.ToArray();
            }
            set
            {
                _trailingData = value;
                TrailingDataMemory = value ?? Array.Empty<byte>();
            }
        }
        public ReadOnlyMemory<byte> TrailingDataMemory { get; set; }

        public WorldInfo()
            : base((byte)PacketTypes.WorldInfo)
        {
            WorldName = string.Empty;
            WorldUniqueIdMemory = new byte[WorldUniqueIdLength];
            TreeTopVariationsMemory = new byte[TreeTopVariationCount];
            ExtraSpawnPoints = Array.Empty<WorldSpawnPoint>();
            TrailingDataMemory = ReadOnlyMemory<byte>.Empty;
        }

        public WorldInfo(BinaryReader br)
            : base(br)
        {
            WorldName = string.Empty;
            WorldUniqueIdMemory = new byte[WorldUniqueIdLength];
            TreeTopVariationsMemory = new byte[TreeTopVariationCount];
            ExtraSpawnPoints = Array.Empty<WorldSpawnPoint>();
            TrailingDataMemory = ReadOnlyMemory<byte>.Empty;

            Time = br.ReadInt32();
            DayandMoonInfo = br.ReadByte();
            MoonPhase = br.ReadByte();
            MaxTilesX = br.ReadInt16();
            MaxTilesY = br.ReadInt16();
            SpawnX = br.ReadInt16();
            SpawnY = br.ReadInt16();
            WorldSurface = br.ReadInt16();
            RockLayer = br.ReadInt16();
            WorldID = br.ReadInt32();
            WorldName = br.ReadString();
            GameMode = br.ReadByte();
            WorldUniqueIdMemory = br.ReadMemory(WorldUniqueIdLength);
            WorldGeneratorVersion = br.ReadUInt64();
            MoonType = br.ReadByte();

            TreeBackground = br.ReadByte();
            TreeBackground2 = br.ReadByte();
            TreeBackground3 = br.ReadByte();
            TreeBackground4 = br.ReadByte();
            CorruptionBackground = br.ReadByte();
            JungleBackground = br.ReadByte();
            SnowBackground = br.ReadByte();
            HallowBackground = br.ReadByte();
            CrimsonBackground = br.ReadByte();
            DesertBackground = br.ReadByte();
            OceanBackground = br.ReadByte();
            MushroomBackground = br.ReadByte();
            UnderworldBackground = br.ReadByte();

            IceBackStyle = br.ReadByte();
            JungleBackStyle = br.ReadByte();
            HellBackStyle = br.ReadByte();

            WindSpeedSet = br.ReadSingle();
            CloudNumber = br.ReadByte();

            Tree1 = br.ReadInt32();
            Tree2 = br.ReadInt32();
            Tree3 = br.ReadInt32();
            TreeStyle1 = br.ReadByte();
            TreeStyle2 = br.ReadByte();
            TreeStyle3 = br.ReadByte();
            TreeStyle4 = br.ReadByte();

            CaveBack1 = br.ReadInt32();
            CaveBack2 = br.ReadInt32();
            CaveBack3 = br.ReadInt32();
            CaveBackStyle1 = br.ReadByte();
            CaveBackStyle2 = br.ReadByte();
            CaveBackStyle3 = br.ReadByte();
            CaveBackStyle4 = br.ReadByte();

            TreeTopVariationsMemory = br.ReadMemory(TreeTopVariationCount);
            Rain = br.ReadSingle();

            EventInfo = br.ReadByte();
            EventInfo2 = br.ReadByte();
            EventInfo3 = br.ReadByte();
            EventInfo4 = br.ReadByte();
            EventInfo5 = br.ReadByte();
            EventInfo6 = br.ReadByte();
            EventInfo7 = br.ReadByte();
            EventInfo8 = br.ReadByte();
            EventInfo9 = br.ReadByte();
            EventInfo10 = br.ReadByte();
            EventInfo11 = br.ReadByte();

            SunDialCooldown = br.ReadByte();
            MoonDialCooldown = br.ReadByte();

            OreTierCopper = br.ReadInt16();
            OreTierIron = br.ReadInt16();
            OreTierSilver = br.ReadInt16();
            OreTierGold = br.ReadInt16();
            OreTierCobalt = br.ReadInt16();
            OreTierMythril = br.ReadInt16();
            OreTierAdamantite = br.ReadInt16();

            InvasionType = br.ReadSByte();
            LobbyID = br.ReadUInt64();
            SandstormSeverity = br.ReadSingle();

            byte extraSpawnCount = br.ReadByte();
            ExtraSpawnPoints = new WorldSpawnPoint[extraSpawnCount];
            for (int i = 0; i < extraSpawnCount; i++)
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                ExtraSpawnPoints[i] = new WorldSpawnPoint(x, y);
            }

            int trailingLength = (int)(br.BaseStream.Length - br.BaseStream.Position);
            if (trailingLength > 0)
            {
                TrailingDataMemory = br.ReadMemory(trailingLength);
            }
        }

        public override short GetLength()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ToStream(ms, includeHeader: false);
                return (short)ms.Length;
            }
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Time);
                bw.Write(DayandMoonInfo);
                bw.Write(MoonPhase);
                bw.Write(MaxTilesX);
                bw.Write(MaxTilesY);
                bw.Write(SpawnX);
                bw.Write(SpawnY);
                bw.Write(WorldSurface);
                bw.Write(RockLayer);
                bw.Write(WorldID);
                bw.Write(WorldName ?? string.Empty);
                bw.Write(GameMode);

                bw.Write(PacketSerializationHelper.NormalizeFixedBuffer(
                    WorldUniqueIdMemory.Length == WorldUniqueIdLength ? WorldUniqueIdMemory.ToArray() : WorldUniqueID,
                    WorldUniqueIdLength));

                bw.Write(WorldGeneratorVersion);
                bw.Write(MoonType);

                bw.Write(TreeBackground);
                bw.Write(TreeBackground2);
                bw.Write(TreeBackground3);
                bw.Write(TreeBackground4);
                bw.Write(CorruptionBackground);
                bw.Write(JungleBackground);
                bw.Write(SnowBackground);
                bw.Write(HallowBackground);
                bw.Write(CrimsonBackground);
                bw.Write(DesertBackground);
                bw.Write(OceanBackground);
                bw.Write(MushroomBackground);
                bw.Write(UnderworldBackground);

                bw.Write(IceBackStyle);
                bw.Write(JungleBackStyle);
                bw.Write(HellBackStyle);

                bw.Write(WindSpeedSet);
                bw.Write(CloudNumber);

                bw.Write(Tree1);
                bw.Write(Tree2);
                bw.Write(Tree3);
                bw.Write(TreeStyle1);
                bw.Write(TreeStyle2);
                bw.Write(TreeStyle3);
                bw.Write(TreeStyle4);

                bw.Write(CaveBack1);
                bw.Write(CaveBack2);
                bw.Write(CaveBack3);
                bw.Write(CaveBackStyle1);
                bw.Write(CaveBackStyle2);
                bw.Write(CaveBackStyle3);
                bw.Write(CaveBackStyle4);

                bw.Write(PacketSerializationHelper.NormalizeFixedBuffer(
                    TreeTopVariationsMemory.Length == TreeTopVariationCount ? TreeTopVariationsMemory.ToArray() : TreeTopVariations,
                    TreeTopVariationCount));

                bw.Write(Rain);

                bw.Write(EventInfo);
                bw.Write(EventInfo2);
                bw.Write(EventInfo3);
                bw.Write(EventInfo4);
                bw.Write(EventInfo5);
                bw.Write(EventInfo6);
                bw.Write(EventInfo7);
                bw.Write(EventInfo8);
                bw.Write(EventInfo9);
                bw.Write(EventInfo10);
                bw.Write(EventInfo11);

                bw.Write(SunDialCooldown);
                bw.Write(MoonDialCooldown);

                bw.Write(OreTierCopper);
                bw.Write(OreTierIron);
                bw.Write(OreTierSilver);
                bw.Write(OreTierGold);
                bw.Write(OreTierCobalt);
                bw.Write(OreTierMythril);
                bw.Write(OreTierAdamantite);

                bw.Write(InvasionType);
                bw.Write(LobbyID);
                bw.Write(SandstormSeverity);

                int extraSpawnCount = ExtraSpawnPoints?.Length ?? 0;
                if (extraSpawnCount > MaximumExtraSpawnPoints)
                {
                    extraSpawnCount = MaximumExtraSpawnPoints;
                }

                bw.Write((byte)extraSpawnCount);
                for (int i = 0; i < extraSpawnCount; i++)
                {
                    bw.Write(ExtraSpawnPoints[i].X);
                    bw.Write(ExtraSpawnPoints[i].Y);
                }

                if (TrailingDataMemory.Length > 0)
                {
                    bw.Write(TrailingDataMemory.Span);
                }
            }
        }
    }
}
