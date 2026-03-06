using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Abstract base class generically representing a terraria packet.
    /// </summary>
    public abstract class TerrariaPacket : TerrariaNetworkObject
    {
        public const short PacketHeaderLength = 3;
        public byte[] TestRawBuffer { get; set; }

        protected short _length = 0;

        /// <summary>
        /// The deserializer map.
        /// 
        /// Deserializer maps point to a function to return a fully qualified packet
        /// from one supplied BinaryReader object.  Derivatives of TerrariaPacket
        /// should make sure that they return a valid packet structure when passed a
        /// BinaryReader to deserialize from.
        /// </summary>
        public static Dictionary<PacketTypes, Func<BinaryReader, TerrariaPacket>> DeserializerMap = new Dictionary<PacketTypes, Func<BinaryReader, TerrariaPacket>>() {
            /*001*/ { PacketTypes.ConnectRequest, (br) => new ConnectRequest(br) },
            /*002*/ { PacketTypes.Disconnect, (br) => new Disconnect(br) },
            /*003*/ { PacketTypes.ContinueConnecting, (br) => new ContinueConnecting(br) },
            /*004*/ { PacketTypes.PlayerInfo, (br) => new PlayerInfo(br) },
            /*005*/ { PacketTypes.PlayerSlot, (br) => new PlayerSlot(br) },
            /*006*/ { PacketTypes.ContinueConnecting2, (br) => new ContinueConnecting2(br) },
            /*007*/ { PacketTypes.WorldInfo, (br) => new WorldInfo(br) },
            /*008*/ { PacketTypes.TileGetSection, (br) => new TileGetSection(br) },
            /*009*/ { PacketTypes.Status, (br) => new Status(br) },
            /*010*/ { PacketTypes.TileSendSection, (br) => new TileSendSection(br) },
            /*011*/ { PacketTypes.TileFrameSection, (br) => new TileFrameSection(br) },
            /*012*/ { PacketTypes.PlayerSpawn, (br) => new PlayerSpawn(br) },
            /*013*/ { PacketTypes.PlayerUpdate, (br) => new PlayerUpdate(br) },
            /*014*/ { PacketTypes.PlayerActive, (br) => new PlayerActive(br) },
            /*015*/ { PacketTypes.Null, (br) => new Null(br) },
            /*016*/ { PacketTypes.PlayerHp, (br) => new PlayerHp(br) },
            /*017*/ { PacketTypes.Tile, (br) => new Tile(br) },
            /*018*/ { PacketTypes.TimeSet, (br) => new TimeSet(br) },
            /*019*/ { PacketTypes.DoorUse, (br) => new DoorUse(br) },
            /*020*/ { PacketTypes.TileSendSquare, (br) => new TileSendSquare(br) },
            /*021*/ { PacketTypes.ItemDrop, (br) => new ItemDrop(br) },
            /*022*/ { PacketTypes.ItemOwner, (br) => new ItemOwner(br) },
            /*023*/ { PacketTypes.NpcUpdate, (br) => new NpcUpdate(br) },
            /*024*/ { PacketTypes.NpcItemStrike, (br) => new NpcItemStrike(br) },
            /*025*/ { PacketTypes.ChatMessage, (br) => new ChatMessage(br) },
            /*026*/ { PacketTypes.PlayerDamage, (br) => new PlayerDamage(br) },
            /*027*/ { PacketTypes.ProjectileNew, (br) => new ProjectileNew(br) },
            /*028*/ { PacketTypes.NpcStrike, (br) => new NpcStrike(br) },
            /*029*/ { PacketTypes.ProjectileDestroy, (br) => new ProjectileDestroy(br) },
            /*030*/ { PacketTypes.TogglePvp, (br) => new TogglePvp(br) },
            /*031*/ { PacketTypes.ChestGetContents, (br) => new ChestGetContents(br) },
            /*032*/ { PacketTypes.ChestItem, (br) => new ChestItem(br) },
            /*033*/ { PacketTypes.ChestOpen, (br) => new ChestOpen(br) },
            /*034*/ { PacketTypes.PlaceChest, (br) => new PlaceChest(br) },
            /*035*/ { PacketTypes.EffectHeal, (br) => new EffectHeal(br) },
            /*036*/ { PacketTypes.Zones, (br) => new Zones(br) },
            /*037*/ { PacketTypes.PasswordRequired, (br) => new PasswordRequired(br) },
            /*038*/ { PacketTypes.PasswordSend, (br) => new PasswordSend(br) },
            /*039*/ { PacketTypes.RemoveItemOwner, (br) => new RemoveItemOwner(br) },
            /*040*/ { PacketTypes.NpcTalk, (br) => new NpcTalk(br) },
            /*041*/ { PacketTypes.PlayerAnimation, (br) => new PlayerAnimation(br) },
            /*042*/ { PacketTypes.PlayerMana, (br) => new PlayerMana(br) },
            /*043*/ { PacketTypes.EffectMana, (br) => new EffectMana(br) },
            /*044*/ { PacketTypes.PlayerDeath, (br) => new PlayerDeath(br) },
            /*045*/ { PacketTypes.PlayerTeam, (br) => new PlayerTeam(br) },
            /*046*/ { PacketTypes.SignRead, (br) => new SignRead(br) },
            /*047*/ { PacketTypes.SignNew, (br) => new SignNew(br) },
            /*048*/ { PacketTypes.LiquidSet, (br) => new LiquidSet(br) },
            /*049*/ { PacketTypes.PlayerSpawnSelf, (br) => new PlayerSpawnSelf(br) },
            /*050*/ { PacketTypes.PlayerBuff, (br) => new PlayerBuff(br) },
            /*051*/ { PacketTypes.NpcSpecial, (br) => new NpcSpecial(br) },
            /*052*/ { PacketTypes.ChestUnlock, (br) => new ChestUnlock(br) },
            /*053*/ { PacketTypes.NpcAddBuff, (br) => new NpcAddBuff(br) },
            /*054*/ { PacketTypes.NpcUpdateBuff, (br) => new NpcUpdateBuff(br) },
            /*055*/ { PacketTypes.PlayerAddBuff, (br) => new PlayerAddBuff(br) },
            /*056*/ { PacketTypes.UpdateNPCName, (br) => new UpdateNPCName(br) },
            /*057*/ { PacketTypes.UpdateGoodEvil, (br) => new UpdateGoodEvil(br) },
            /*058*/ { PacketTypes.PlayHarp, (br) => new PlayHarp(br) },
            /*059*/ { PacketTypes.HitSwitch, (br) => new HitSwitch(br) },
            /*060*/ { PacketTypes.UpdateNPCHome, (br) => new UpdateNPCHome(br) },
            /*061*/ { PacketTypes.SpawnBossorInvasion, (br) => new SpawnBossorInvasion(br) },
            /*062*/ { PacketTypes.PlayerDodge, (br) => new PlayerDodge(br) },
            /*063*/ { PacketTypes.PaintTile, (br) => new PaintTile(br) },
            /*064*/ { PacketTypes.PaintWall, (br) => new PaintWall(br) },
            /*065*/ { PacketTypes.Teleport, (br) => new Teleport(br) },
            /*066*/ { PacketTypes.PlayerHealOther, (br) => new PlayerHealOther(br) },
            /*067*/ { PacketTypes.Placeholder, (br) => new Placeholder(br) },
            /*068*/ { PacketTypes.ClientUUID, (br) => new ClientUUID(br) },
            /*069*/ { PacketTypes.ChestName, (br) => new ChestName(br) },
            /*070*/ { PacketTypes.CatchNPC, (br) => new CatchNPC(br) },
            /*071*/ { PacketTypes.ReleaseNPC, (br) => new ReleaseNPC(br) },
            /*072*/ { PacketTypes.TravellingMerchantInventory, (br) => new TravellingMerchantInventory(br) },
            /*073*/ { PacketTypes.TeleportationPotion, (br) => new TeleportationPotion(br) },
            /*074*/ { PacketTypes.AnglerQuest, (br) => new AnglerQuest(br) },
            /*075*/ { PacketTypes.CompleteAnglerQuest, (br) => new CompleteAnglerQuest(br) },
            /*076*/ { PacketTypes.NumberOfAnglerQuestsCompleted, (br) => new NumberOfAnglerQuestsCompleted(br) },
            /*077*/ { PacketTypes.CreateTemporaryAnimation, (br) => new CreateTemporaryAnimation(br) },
            /*078*/ { PacketTypes.ReportInvasionProgress, (br) => new ReportInvasionProgress(br) },
            /*079*/ { PacketTypes.PlaceObject, (br) => new PlaceObject(br) },
            /*080*/ { PacketTypes.SyncPlayerChestIndex, (br) => new SyncPlayerChestIndex(br) },
            /*081*/ { PacketTypes.CreateCombatText, (br) => new CreateCombatText(br) },
            /*082*/ { PacketTypes.LoadNetModule, (br) => new LoadNetModule(br) },
            /*083*/ { PacketTypes.NpcKillCount, (br) => new NpcKillCount(br) },
            /*084*/ { PacketTypes.PlayerStealth, (br) => new PlayerStealth(br) },
            /*085*/ { PacketTypes.ForceItemIntoNearestChest, (br) => new ForceItemIntoNearestChest(br) },
            /*086*/ { PacketTypes.UpdateTileEntity, (br) => new UpdateTileEntity(br) },
            /*087*/ { PacketTypes.PlaceTileEntity, (br) => new PlaceTileEntity(br) },
            /*088*/ { PacketTypes.TweakItem, (br) => new TweakItem(br) },
            /*089*/ { PacketTypes.PlaceItemFrame, (br) => new PlaceItemFrame(br) },
            /*090*/ { PacketTypes.UpdateItemDrop, (br) => new UpdateItemDrop(br) },
            /*091*/ { PacketTypes.EmoteBubble, (br) => new EmoteBubble(br) },
            /*092*/ { PacketTypes.SyncExtraValue, (br) => new SyncExtraValue(br) },
            /*093*/ { PacketTypes.SocialHandshake, (br) => new SocialHandshake(br) },
            /*094*/ { PacketTypes.DebugCommands, (br) => new DebugCommands(br) },
            /*095*/ { PacketTypes.KillPortal, (br) => new KillPortal(br) },
            /*096*/ { PacketTypes.PlayerTeleportPortal, (br) => new PlayerTeleportPortal(br) },
            /*097*/ { PacketTypes.NotifyPlayerNpcKilled, (br) => new NotifyPlayerNpcKilled(br) },
            /*098*/ { PacketTypes.NotifyPlayerOfEvent, (br) => new NotifyPlayerOfEvent(br) },
            /*099*/ { PacketTypes.UpdateMinionTarget, (br) => new UpdateMinionTarget(br) },
            /*100*/ { PacketTypes.NpcTeleportPortal, (br) => new NpcTeleportPortal(br) },
            /*101*/ { PacketTypes.UpdateShieldStrengths, (br) => new UpdateShieldStrengths(br) },
            /*102*/ { PacketTypes.NebulaLevelUp, (br) => new NebulaLevelUp(br) },
            /*103*/ { PacketTypes.MoonLordCountdown, (br) => new MoonLordCountdown(br) },
            /*104*/ { PacketTypes.NpcShopItem, (br) => new NpcShopItem(br) },
            /*105*/ { PacketTypes.GemLockToggle, (br) => new GemLockToggle(br) },
            /*106*/ { PacketTypes.PoofOfSmoke, (br) => new PoofOfSmoke(br) },
            /*107*/ { PacketTypes.SmartTextMessage, (br) => new SmartTextMessage(br) },
            /*108*/ { PacketTypes.WiredCannonShot, (br) => new WiredCannonShot(br) },
            /*109*/ { PacketTypes.MassWireOperation, (br) => new MassWireOperation(br) },
            /*110*/ { PacketTypes.MassWireOperationPay, (br) => new MassWireOperationPay(br) },
            /*111*/ { PacketTypes.ToggleParty, (br) => new ToggleParty(br) },
            /*112*/ { PacketTypes.TreeGrowFX, (br) => new TreeGrowFX(br) },
            /*113*/ { PacketTypes.CrystalInvasionStart, (br) => new CrystalInvasionStart(br) },
            /*114*/ { PacketTypes.CrystalInvasionWipeAll, (br) => new CrystalInvasionWipeAll(br) },
            /*115*/ { PacketTypes.MinionAttackTargetUpdate, (br) => new MinionAttackTargetUpdate(br) },
            /*116*/ { PacketTypes.CrystalInvasionSendWaitTime, (br) => new CrystalInvasionSendWaitTime(br) },
            /*117*/ { PacketTypes.PlayerHurtV2, (br) => new PlayerHurtV2(br) },
            /*118*/ { PacketTypes.PlayerDeathV2, (br) => new PlayerDeathV2(br) },
            /*119*/ { PacketTypes.CreateCombatTextExtended, (br) => new CreateCombatTextExtended(br) },
            /*120*/ { PacketTypes.Emoji, (br) => new Emoji(br) },
            /*121*/ { PacketTypes.TileEntityDisplayDollItemSync, (br) => new TileEntityDisplayDollItemSync(br) },
            /*122*/ { PacketTypes.RequestTileEntityInteraction, (br) => new RequestTileEntityInteraction(br) },
            /*123*/ { PacketTypes.WeaponsRackTryPlacing, (br) => new WeaponsRackTryPlacing(br) },
            /*124*/ { PacketTypes.TileEntityHatRackItemSync, (br) => new TileEntityHatRackItemSync(br) },
            /*125*/ { PacketTypes.SyncTilePicking, (br) => new SyncTilePicking(br) },
            /*126*/ { PacketTypes.SyncRevengeMarker, (br) => new SyncRevengeMarker(br) },
            /*127*/ { PacketTypes.RemoveRevengeMarker, (br) => new RemoveRevengeMarker(br) },
            /*128*/ { PacketTypes.LandGolfBallInCup, (br) => new LandGolfBallInCup(br) },
            /*129*/ { PacketTypes.FinishedConnectingToServer, (br) => new FinishedConnectingToServer(br) },
            /*130*/ { PacketTypes.FishOutNPC, (br) => new FishOutNPC(br) },
            /*131*/ { PacketTypes.TamperWithNPC, (br) => new TamperWithNPC(br) },
            /*132*/ { PacketTypes.PlayLegacySound, (br) => new PlayLegacySound(br) },
            /*133*/ { PacketTypes.FoodPlatterTryPlacing, (br) => new FoodPlatterTryPlacing(br) },
            /*134*/ { PacketTypes.UpdatePlayerLuckFactors, (br) => new UpdatePlayerLuckFactors(br) },
            /*135*/ { PacketTypes.DeadPlayer, (br) => new DeadPlayer(br) },
            /*136*/ { PacketTypes.SyncCavernMonsterType, (br) => new SyncCavernMonsterType(br) },
            /*137*/ { PacketTypes.RequestNPCBuffRemoval, (br) => new RequestNPCBuffRemoval(br) },
            /*138*/ { PacketTypes.ClientSyncedInventory, (br) => new ClientSyncedInventory(br) },
            /*139*/ { PacketTypes.SetCountsAsHostForGameplay, (br) => new SetCountsAsHostForGameplay(br) },
            /*140*/ { PacketTypes.SetMiscEventValues, (br) => new SetMiscEventValues(br) },
            /*141*/ { PacketTypes.RequestLucyPopup, (br) => new RequestLucyPopup(br) },
            /*142*/ { PacketTypes.SyncProjectileTrackers, (br) => new SyncProjectileTrackers(br) },
            /*143*/ { PacketTypes.CrystalInvasionRequestedToSkipWaitTime, (br) => new CrystalInvasionRequestedToSkipWaitTime(br) },
            /*144*/ { PacketTypes.RequestQuestEffect, (br) => new RequestQuestEffect(br) },
            /*145*/ { PacketTypes.SyncItemsWithShimmer, (br) => new SyncItemsWithShimmer(br) },
            /*146*/ { PacketTypes.ShimmerActions, (br) => new ShimmerActions(br) },
            /*147*/ { PacketTypes.SyncLoadout, (br) => new SyncLoadout(br) },
            /*148*/ { PacketTypes.SyncItemCannotBeTakenByEnemies, (br) => new SyncItemCannotBeTakenByEnemies(br) },
            /*149*/ { PacketTypes.TEDeadCellsDisplayJar, (br) => new TEDeadCellsDisplayJar(br) },
            /*150*/ { PacketTypes.SpectatePlayer, (br) => new SpectatePlayer(br) },
            /*151*/ { PacketTypes.SyncItemDespawn, (br) => new SyncItemDespawn(br) },
            /*152*/ { PacketTypes.ItemUseSound, (br) => new ItemUseSound(br) },
            /*153*/ { PacketTypes.NpcHurtByDebuff, (br) => new NpcHurtByDebuff(br) },
            /*154*/ { PacketTypes.Ping, (br) => new Ping(br) },
            /*155*/ { PacketTypes.SyncChestSize, (br) => new SyncChestSize(br) },
            /*156*/ { PacketTypes.TELeashedEntityAnchorPlaceItem, (br) => new TELeashedEntityAnchorPlaceItem(br) },
            /*157*/ { PacketTypes.TeamChangeFromUI, (br) => new TeamChangeFromUI(br) },
            /*158*/ { PacketTypes.ExtraSpawnSectionLoaded, (br) => new ExtraSpawnSectionLoaded(br) },
            /*159*/ { PacketTypes.RequestSection, (br) => new RequestSection(br) },
            /*160*/ { PacketTypes.SyncItemPosition, (br) => new SyncItemPosition(br) },
            /*161*/ { PacketTypes.HostToken, (br) => new HostToken(br) },
            /*162*/ { PacketTypes.ServerInfo, (br) => new ServerInfo(br) },
            /*163*/ { PacketTypes.PlayerPlatformInfo, (br) => new PlayerPlatformInfo(br) }
        };

        /// <summary>
        /// Gets or sets the packet identifier.
        /// </summary>
        public byte Id { get; protected set; }

        /// <summary>
        /// Gets the type of the packet.
        /// </summary>
        public PacketTypes PacketType
        {
            get
            {
                return (PacketTypes)this.Id;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrariaPacket"/> with 
        /// the specified BinaryReader object to deserialize a derivative on.
        /// </summary>
        /// <param name="br">
        /// A reference to a BinaryReader which contains binary payload to be deserialized into
        /// a fully-qualified TerrariaPacket.
        /// </param>
        protected TerrariaPacket(BinaryReader br)
        {
            _length = br.ReadInt16();
            this.Id = br.ReadByte();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrariaPacket"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        protected TerrariaPacket(byte id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Deserializes a packet from the specified binary reader and returns a TerrariaPacket 
        /// derivative according to the deserializer methods in DeserializerMap.
        /// </summary>
        /// <param name="br">
        /// An instance of a BinaryReader which contains a binary terraria packet payload in 
        /// which to deserialize an object from
        /// </param>
        /// <param name="id">
        /// Packet identifier that is used to find the deserializer method via DeserializerMap
        /// </param>
        public static TerrariaPacket Deserialize(BinaryReader br, byte id)
        {
            br.BaseStream.Seek(0, SeekOrigin.Begin);

            if (!DeserializerMap.TryGetValue((PacketTypes)id, out Func<BinaryReader, TerrariaPacket> deserializer))
            {
                return new UnknownPacket(br);
            }

            return deserializer(br);
        }

        public static TerrariaPacket Deserialize(ReadOnlyMemory<byte> packetBuffer)
        {
            ValidatePacketBuffer(packetBuffer);
            using PacketBinaryReader br = new PacketBinaryReader(packetBuffer);
            return Deserialize(br);
        }

        public static TerrariaPacket Deserialize(byte[] packetBuffer, int offset, int length)
        {
            return Deserialize(new ReadOnlyMemory<byte>(packetBuffer, offset, length));
        }

        public static TerrariaPacket DeserializePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer)
        {
            ValidatePayloadBuffer(payloadBuffer);
            using PacketBinaryReader br = new PacketBinaryReader(packetType, payloadBuffer);
            return Deserialize(br, (byte)packetType);
        }

        public static TerrariaPacket DeserializePayload(byte packetId, ReadOnlyMemory<byte> payloadBuffer)
        {
            ValidatePayloadBuffer(payloadBuffer);
            using PacketBinaryReader br = new PacketBinaryReader(packetId, payloadBuffer);
            return Deserialize(br, packetId);
        }

        public static TerrariaPacket DeserializePayload(PacketTypes packetType, byte[] payloadBuffer, int offset, int length)
        {
            return DeserializePayload(packetType, new ReadOnlyMemory<byte>(payloadBuffer, offset, length));
        }

        public static TerrariaPacket DeserializePayload(byte packetId, byte[] payloadBuffer, int offset, int length)
        {
            return DeserializePayload(packetId, new ReadOnlyMemory<byte>(payloadBuffer, offset, length));
        }

        public static bool TryDeserialize(ReadOnlyMemory<byte> packetBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = Deserialize(packetBuffer);
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }

        public static bool TryDeserialize(byte[] packetBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserialize(new ReadOnlyMemory<byte>(packetBuffer, offset, length), out packet);
        }

        public static bool TryDeserializePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = DeserializePayload(packetType, payloadBuffer);
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }

        public static bool TryDeserializePayload(byte packetId, ReadOnlyMemory<byte> payloadBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = DeserializePayload(packetId, payloadBuffer);
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }

        public static bool TryDeserializePayload(PacketTypes packetType, byte[] payloadBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserializePayload(packetType, new ReadOnlyMemory<byte>(payloadBuffer, offset, length), out packet);
        }

        public static bool TryDeserializePayload(byte packetId, byte[] payloadBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserializePayload(packetId, new ReadOnlyMemory<byte>(payloadBuffer, offset, length), out packet);
        }

        /// <summary>
        /// Deserializes a packet from the specified binary reader and returns a TerrariaPacket 
        /// derivative according to the deserializer methods in DeserializerMap.
        /// </summary>
        /// <param name="br">
        /// An instance of a BinaryReader which contains a binary terraria packet payload in 
        /// which to deserialize an object from
        /// </param>
        public static TerrariaPacket Deserialize(BinaryReader br)
        {
            br.ReadInt16();
            byte id = br.ReadByte();

            return Deserialize(br, id);
        }

        private static void ValidatePacketBuffer(ReadOnlyMemory<byte> packetBuffer)
        {
            if (packetBuffer.Length < PacketHeaderLength)
            {
                throw new InvalidDataException($"Packet buffer is too short. Expected at least {PacketHeaderLength} bytes, got {packetBuffer.Length}.");
            }

            short declaredLength = BinaryPrimitives.ReadInt16LittleEndian(packetBuffer.Span);
            if (declaredLength < PacketHeaderLength)
            {
                throw new InvalidDataException($"Invalid packet header length {declaredLength}. Minimum valid length is {PacketHeaderLength}.");
            }

            if (declaredLength != packetBuffer.Length)
            {
                throw new InvalidDataException($"Packet buffer length mismatch. Header says {declaredLength} bytes, actual slice is {packetBuffer.Length} bytes.");
            }
        }

        private static void ValidatePayloadBuffer(ReadOnlyMemory<byte> payloadBuffer)
        {
            if (payloadBuffer.Length > short.MaxValue - PacketHeaderLength)
            {
                throw new InvalidDataException(
                    $"Payload buffer is too large. Maximum payload size is {short.MaxValue - PacketHeaderLength} bytes, got {payloadBuffer.Length}.");
            }
        }

        /// <summary>
        /// Serializes this TerrariaNetworkObject instance into the provided stream.
        /// </summary>
        /// <param name="stream">
        /// A reference to a valid, open, and writable stream object in which to serialize this
        /// instance to.
        /// </param>
        public override void ToStream(Stream stream, bool includeHeader = true)
        {
          if (includeHeader == false)
          {
            return;
          }

          using (BinaryWriter br = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
          {
            br.Write((short)(GetLength() + PacketHeaderLength));
            br.Write(Id);
          }
        }
	}
}




