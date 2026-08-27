using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct WorldInfoView
    {
        private readonly PacketView _packet;
        private readonly WorldInfo _packetObject;

        public WorldInfoView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.WorldInfo, payload))
        {
        }

        public static WorldInfoView FromPayload(ReadOnlySpan<byte> payload) => new WorldInfoView(payload);

        public WorldInfoView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<WorldInfo>(packet, PacketTypes.WorldInfo);
        }

        public PacketHeader Header => _packet.Header;
        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;
        public WorldInfo Packet => _packetObject;
        public int Time => _packetObject.Time;
        public byte DayandMoonInfo => _packetObject.DayandMoonInfo;
        public byte MoonPhase => _packetObject.MoonPhase;
        public short MaxTilesX => _packetObject.MaxTilesX;
        public short MaxTilesY => _packetObject.MaxTilesY;
        public short SpawnX => _packetObject.SpawnX;
        public short SpawnY => _packetObject.SpawnY;
        public short WorldSurface => _packetObject.WorldSurface;
        public short RockLayer => _packetObject.RockLayer;
        public int WorldId => _packetObject.WorldId;
        public string WorldName => _packetObject.WorldName;
        public byte GameMode => _packetObject.GameMode;
        public ReadOnlyMemory<byte> WorldUniqueIdMemory => _packetObject.WorldUniqueIdMemory;
        public ulong WorldGeneratorVersion => _packetObject.WorldGeneratorVersion;
        public byte MoonType => _packetObject.MoonType;
        public byte TreeBackground => _packetObject.TreeBackground;
        public byte TreeBackground2 => _packetObject.TreeBackground2;
        public byte TreeBackground3 => _packetObject.TreeBackground3;
        public byte TreeBackground4 => _packetObject.TreeBackground4;
        public byte CorruptionBackground => _packetObject.CorruptionBackground;
        public byte JungleBackground => _packetObject.JungleBackground;
        public byte SnowBackground => _packetObject.SnowBackground;
        public byte HallowBackground => _packetObject.HallowBackground;
        public byte CrimsonBackground => _packetObject.CrimsonBackground;
        public byte DesertBackground => _packetObject.DesertBackground;
        public byte OceanBackground => _packetObject.OceanBackground;
        public byte MushroomBackground => _packetObject.MushroomBackground;
        public byte UnderworldBackground => _packetObject.UnderworldBackground;
        public byte IceBackStyle => _packetObject.IceBackStyle;
        public byte JungleBackStyle => _packetObject.JungleBackStyle;
        public byte HellBackStyle => _packetObject.HellBackStyle;
        public float WindSpeedSet => _packetObject.WindSpeedSet;
        public byte CloudNumber => _packetObject.CloudNumber;
        public int Tree1 => _packetObject.Tree1;
        public int Tree2 => _packetObject.Tree2;
        public int Tree3 => _packetObject.Tree3;
        public byte TreeStyle1 => _packetObject.TreeStyle1;
        public byte TreeStyle2 => _packetObject.TreeStyle2;
        public byte TreeStyle3 => _packetObject.TreeStyle3;
        public byte TreeStyle4 => _packetObject.TreeStyle4;
        public int CaveBack1 => _packetObject.CaveBack1;
        public int CaveBack2 => _packetObject.CaveBack2;
        public int CaveBack3 => _packetObject.CaveBack3;
        public byte CaveBackStyle1 => _packetObject.CaveBackStyle1;
        public byte CaveBackStyle2 => _packetObject.CaveBackStyle2;
        public byte CaveBackStyle3 => _packetObject.CaveBackStyle3;
        public byte CaveBackStyle4 => _packetObject.CaveBackStyle4;
        public ReadOnlyMemory<byte> TreeTopVariationsMemory => _packetObject.TreeTopVariationsMemory;
        public float Rain => _packetObject.Rain;
        public byte EventInfo => _packetObject.EventInfo;
        public byte EventInfo2 => _packetObject.EventInfo2;
        public byte EventInfo3 => _packetObject.EventInfo3;
        public byte EventInfo4 => _packetObject.EventInfo4;
        public byte EventInfo5 => _packetObject.EventInfo5;
        public byte EventInfo6 => _packetObject.EventInfo6;
        public byte EventInfo7 => _packetObject.EventInfo7;
        public byte EventInfo8 => _packetObject.EventInfo8;
        public byte EventInfo9 => _packetObject.EventInfo9;
        public byte EventInfo10 => _packetObject.EventInfo10;
        public byte EventInfo11 => _packetObject.EventInfo11;
        public byte SunDialCooldown => _packetObject.SunDialCooldown;
        public byte MoonDialCooldown => _packetObject.MoonDialCooldown;
        public short OreTierCopper => _packetObject.OreTierCopper;
        public short OreTierIron => _packetObject.OreTierIron;
        public short OreTierSilver => _packetObject.OreTierSilver;
        public short OreTierGold => _packetObject.OreTierGold;
        public short OreTierCobalt => _packetObject.OreTierCobalt;
        public short OreTierMythril => _packetObject.OreTierMythril;
        public short OreTierAdamantite => _packetObject.OreTierAdamantite;
        public sbyte InvasionType => _packetObject.InvasionType;
        public ulong LobbyId => _packetObject.LobbyId;
        public float SandstormSeverity => _packetObject.SandstormSeverity;
        public WorldSpawnPoint[] ExtraSpawnPoints => _packetObject.ExtraSpawnPoints;
        public short DungeonX => _packetObject.DungeonX;
        public short DungeonY => _packetObject.DungeonY;
        public ReadOnlyMemory<byte> TrailingDataMemory => _packetObject.TrailingDataMemory;
    }
}
