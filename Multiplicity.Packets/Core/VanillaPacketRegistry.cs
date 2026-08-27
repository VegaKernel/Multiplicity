using System;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Compile-time registry for the vanilla Terraria desktop packet surface.
    /// No assembly scanning, constructor lookup or reflected constant discovery is used.
    /// </summary>
    internal static class VanillaPacketRegistry
    {
        private static readonly PacketRegistration[] Registrations =
        {
            V(PacketTypes.ConnectRequest, br => new ConnectRequest(br), () => new ConnectRequest()),
            V(PacketTypes.Disconnect, br => new Disconnect(br), () => new Disconnect()),
            F(PacketTypes.ContinueConnecting, br => new ContinueConnecting(br), () => new ContinueConnecting()),
            V(PacketTypes.PlayerInfo, br => new PlayerInfo(br), () => new PlayerInfo()),
            F(PacketTypes.PlayerSlot, br => new PlayerSlot(br), () => new PlayerSlot()),
            V(PacketTypes.ContinueConnecting2, br => new ContinueConnecting2(br), () => new ContinueConnecting2()),
            V(PacketTypes.WorldInfo, br => new WorldInfo(br), () => new WorldInfo()),
            F(PacketTypes.TileGetSection, br => new TileGetSection(br), () => new TileGetSection()),
            V(PacketTypes.Status, br => new Status(br), () => new Status()),
            V(PacketTypes.TileSendSection, br => new TileSendSection(br), () => new TileSendSection()),
            F(PacketTypes.TileFrameSection, br => new TileFrameSection(br), () => new TileFrameSection()),
            F(PacketTypes.PlayerSpawn, br => new PlayerSpawn(br), () => new PlayerSpawn()),
            V(PacketTypes.PlayerUpdate, br => new PlayerUpdate(br), () => new PlayerUpdate()),
            V(PacketTypes.PlayerActive, br => new PlayerActive(br), () => new PlayerActive()),
            V(PacketTypes.Null, br => new Null(br), () => new Null()),
            V(PacketTypes.PlayerHp, br => new PlayerHp(br), () => new PlayerHp()),
            F(PacketTypes.Tile, br => new Tile(br), () => new Tile()),
            V(PacketTypes.TimeSet, br => new TimeSet(br), () => new TimeSet()),
            F(PacketTypes.DoorUse, br => new DoorUse(br), () => new DoorUse()),
            V(PacketTypes.TileSendSquare, br => new TileSendSquare(br), () => new TileSendSquare()),
            V(PacketTypes.ItemDrop, br => new ItemDrop(br), () => new ItemDrop()),
            V(PacketTypes.ItemOwner, br => new ItemOwner(br), () => new ItemOwner()),
            V(PacketTypes.NpcUpdate, br => new NpcUpdate(br), () => new NpcUpdate()),
            V(PacketTypes.ProjectileNew, br => new ProjectileNew(br), () => new ProjectileNew()),
            V(PacketTypes.NpcStrike, br => new NpcStrike(br), () => new NpcStrike()),
            V(PacketTypes.ProjectileDestroy, br => new ProjectileDestroy(br), () => new ProjectileDestroy()),
            V(PacketTypes.TogglePvp, br => new TogglePvp(br), () => new TogglePvp()),
            F(PacketTypes.ChestGetContents, br => new ChestGetContents(br), () => new ChestGetContents()),
            F(PacketTypes.ChestItem, br => new ChestItem(br), () => new ChestItem()),
            V(PacketTypes.ChestOpen, br => new ChestOpen(br), () => new ChestOpen()),
            F(PacketTypes.PlaceChest, br => new PlaceChest(br), () => new PlaceChest()),
            V(PacketTypes.EffectHeal, br => new EffectHeal(br), () => new EffectHeal()),
            F(PacketTypes.Zones, br => new Zones(br), () => new Zones()),
            V(PacketTypes.PasswordRequired, br => new PasswordRequired(br), () => new PasswordRequired()),
            V(PacketTypes.PasswordSend, br => new PasswordSend(br), () => new PasswordSend()),
            V(PacketTypes.RemoveItemOwner, br => new RemoveItemOwner(br), () => new RemoveItemOwner()),
            V(PacketTypes.NpcTalk, br => new NpcTalk(br), () => new NpcTalk()),
            V(PacketTypes.PlayerAnimation, br => new PlayerAnimation(br), () => new PlayerAnimation()),
            V(PacketTypes.PlayerMana, br => new PlayerMana(br), () => new PlayerMana()),
            V(PacketTypes.EffectMana, br => new EffectMana(br), () => new EffectMana()),
            V(PacketTypes.PlayerTeam, br => new PlayerTeam(br), () => new PlayerTeam()),
            V(PacketTypes.SignRead, br => new SignRead(br), () => new SignRead()),
            V(PacketTypes.SignNew, br => new SignNew(br), () => new SignNew()),
            V(PacketTypes.PlayerSpawnSelf, br => new PlayerSpawnSelf(br), () => new PlayerSpawnSelf()),
            V(PacketTypes.PlayerBuff, br => new PlayerBuff(br), () => new PlayerBuff()),
            F(PacketTypes.NpcSpecial, br => new NpcSpecial(br), () => new NpcSpecial()),
            F(PacketTypes.ChestUnlock, br => new ChestUnlock(br), () => new ChestUnlock()),
            F(PacketTypes.NpcAddBuff, br => new NpcAddBuff(br), () => new NpcAddBuff()),
            V(PacketTypes.NpcUpdateBuff, br => new NpcUpdateBuff(br), () => new NpcUpdateBuff()),
            F(PacketTypes.PlayerAddBuff, br => new PlayerAddBuff(br), () => new PlayerAddBuff()),
            V(PacketTypes.UpdateNPCName, br => new UpdateNPCName(br), () => new UpdateNPCName()),
            V(PacketTypes.UpdateGoodEvil, br => new UpdateGoodEvil(br), () => new UpdateGoodEvil()),
            V(PacketTypes.PlayHarp, br => new PlayHarp(br), () => new PlayHarp()),
            V(PacketTypes.HitSwitch, br => new HitSwitch(br), () => new HitSwitch()),
            V(PacketTypes.UpdateNPCHome, br => new UpdateNPCHome(br), () => new UpdateNPCHome()),
            V(PacketTypes.SpawnBossorInvasion, br => new SpawnBossorInvasion(br), () => new SpawnBossorInvasion()),
            V(PacketTypes.PlayerDodge, br => new PlayerDodge(br), () => new PlayerDodge()),
            V(PacketTypes.PaintTile, br => new PaintTile(br), () => new PaintTile()),
            V(PacketTypes.PaintWall, br => new PaintWall(br), () => new PaintWall()),
            V(PacketTypes.Teleport, br => new Teleport(br), () => new Teleport()),
            V(PacketTypes.PlayerHealOther, br => new PlayerHealOther(br), () => new PlayerHealOther()),
            V(PacketTypes.Placeholder, br => new Placeholder(br), () => new Placeholder()),
            V(PacketTypes.ClientUUID, br => new ClientUUID(br), () => new ClientUUID()),
            V(PacketTypes.ChestName, br => new ChestName(br), () => new ChestName()),
            F(PacketTypes.CatchNPC, br => new CatchNPC(br), () => new CatchNPC()),
            V(PacketTypes.ReleaseNPC, br => new ReleaseNPC(br), () => new ReleaseNPC()),
            V(PacketTypes.TravellingMerchantInventory, br => new TravellingMerchantInventory(br), () => new TravellingMerchantInventory()),
            F(PacketTypes.TeleportationPotion, br => new TeleportationPotion(br), () => new TeleportationPotion()),
            V(PacketTypes.AnglerQuest, br => new AnglerQuest(br), () => new AnglerQuest()),
            V(PacketTypes.CompleteAnglerQuest, br => new CompleteAnglerQuest(br), () => new CompleteAnglerQuest()),
            V(PacketTypes.NumberOfAnglerQuestsCompleted, br => new NumberOfAnglerQuestsCompleted(br), () => new NumberOfAnglerQuestsCompleted()),
            V(PacketTypes.CreateTemporaryAnimation, br => new CreateTemporaryAnimation(br), () => new CreateTemporaryAnimation()),
            V(PacketTypes.ReportInvasionProgress, br => new ReportInvasionProgress(br), () => new ReportInvasionProgress()),
            F(PacketTypes.PlaceObject, br => new PlaceObject(br), () => new PlaceObject()),
            V(PacketTypes.SyncPlayerChestIndex, br => new SyncPlayerChestIndex(br), () => new SyncPlayerChestIndex()),
            V(PacketTypes.CreateCombatText, br => new CreateCombatText(br), () => new CreateCombatText()),
            V(PacketTypes.LoadNetModule, br => new LoadNetModule(br), () => new LoadNetModule()),
            V(PacketTypes.PlayerStealth, br => new PlayerStealth(br), () => new PlayerStealth()),
            V(PacketTypes.ForceItemIntoNearestChest, br => new ForceItemIntoNearestChest(br), () => new ForceItemIntoNearestChest()),
            V(PacketTypes.UpdateTileEntity, br => new UpdateTileEntity(br), () => new UpdateTileEntity()),
            F(PacketTypes.PlaceTileEntity, br => new PlaceTileEntity(br), () => new PlaceTileEntity()),
            V(PacketTypes.TweakItem, br => new TweakItem(br), () => new TweakItem()),
            V(PacketTypes.PlaceItemFrame, br => new PlaceItemFrame(br), () => new PlaceItemFrame()),
            V(PacketTypes.UpdateItemDrop, br => new UpdateItemDrop(br), () => new UpdateItemDrop()),
            V(PacketTypes.EmoteBubble, br => new EmoteBubble(br), () => new EmoteBubble()),
            F(PacketTypes.SyncExtraValue, br => new SyncExtraValue(br), () => new SyncExtraValue()),
            V(PacketTypes.SocialHandshake, br => new SocialHandshake(br), () => new SocialHandshake()),
            V(PacketTypes.DebugCommands, br => new DebugCommands(br), () => new DebugCommands()),
            F(PacketTypes.KillPortal, br => new KillPortal(br), () => new KillPortal()),
            V(PacketTypes.PlayerTeleportPortal, br => new PlayerTeleportPortal(br), () => new PlayerTeleportPortal()),
            V(PacketTypes.NotifyPlayerNpcKilled, br => new NotifyPlayerNpcKilled(br), () => new NotifyPlayerNpcKilled()),
            V(PacketTypes.NotifyPlayerOfEvent, br => new NotifyPlayerOfEvent(br), () => new NotifyPlayerOfEvent()),
            V(PacketTypes.UpdateMinionTarget, br => new UpdateMinionTarget(br), () => new UpdateMinionTarget()),
            V(PacketTypes.NpcTeleportPortal, br => new NpcTeleportPortal(br), () => new NpcTeleportPortal()),
            F(PacketTypes.UpdateShieldStrengths, br => new UpdateShieldStrengths(br), () => new UpdateShieldStrengths()),
            F(PacketTypes.NebulaLevelUp, br => new NebulaLevelUp(br), () => new NebulaLevelUp()),
            V(PacketTypes.MoonLordCountdown, br => new MoonLordCountdown(br), () => new MoonLordCountdown()),
            F(PacketTypes.NpcShopItem, br => new NpcShopItem(br), () => new NpcShopItem()),
            V(PacketTypes.GemLockToggle, br => new GemLockToggle(br), () => new GemLockToggle()),
            V(PacketTypes.PoofOfSmoke, br => new PoofOfSmoke(br), () => new PoofOfSmoke()),
            V(PacketTypes.SmartTextMessage, br => new SmartTextMessage(br), () => new SmartTextMessage()),
            F(PacketTypes.WiredCannonShot, br => new WiredCannonShot(br), () => new WiredCannonShot()),
            F(PacketTypes.MassWireOperation, br => new MassWireOperation(br), () => new MassWireOperation()),
            F(PacketTypes.MassWireOperationPay, br => new MassWireOperationPay(br), () => new MassWireOperationPay()),
            V(PacketTypes.ToggleParty, br => new ToggleParty(br), () => new ToggleParty()),
            V(PacketTypes.TreeGrowFX, br => new TreeGrowFX(br), () => new TreeGrowFX()),
            V(PacketTypes.CrystalInvasionStart, br => new CrystalInvasionStart(br), () => new CrystalInvasionStart()),
            V(PacketTypes.CrystalInvasionWipeAll, br => new CrystalInvasionWipeAll(br), () => new CrystalInvasionWipeAll()),
            V(PacketTypes.MinionAttackTargetUpdate, br => new MinionAttackTargetUpdate(br), () => new MinionAttackTargetUpdate()),
            V(PacketTypes.CrystalInvasionSendWaitTime, br => new CrystalInvasionSendWaitTime(br), () => new CrystalInvasionSendWaitTime()),
            V(PacketTypes.PlayerHurtV2, br => new PlayerHurtV2(br), () => new PlayerHurtV2()),
            V(PacketTypes.PlayerDeathV2, br => new PlayerDeathV2(br), () => new PlayerDeathV2()),
            V(PacketTypes.CreateCombatTextExtended, br => new CreateCombatTextExtended(br), () => new CreateCombatTextExtended()),
            F(PacketTypes.Emoji, br => new Emoji(br), () => new Emoji()),
            F(PacketTypes.TileEntityDisplayDollItemSync, br => new TileEntityDisplayDollItemSync(br), () => new TileEntityDisplayDollItemSync()),
            F(PacketTypes.RequestTileEntityInteraction, br => new RequestTileEntityInteraction(br), () => new RequestTileEntityInteraction()),
            V(PacketTypes.WeaponsRackTryPlacing, br => new WeaponsRackTryPlacing(br), () => new WeaponsRackTryPlacing()),
            F(PacketTypes.TileEntityHatRackItemSync, br => new TileEntityHatRackItemSync(br), () => new TileEntityHatRackItemSync()),
            F(PacketTypes.SyncTilePicking, br => new SyncTilePicking(br), () => new SyncTilePicking()),
            V(PacketTypes.SyncRevengeMarker, br => new SyncRevengeMarker(br), () => new SyncRevengeMarker()),
            F(PacketTypes.RemoveRevengeMarker, br => new RemoveRevengeMarker(br), () => new RemoveRevengeMarker()),
            F(PacketTypes.LandGolfBallInCup, br => new LandGolfBallInCup(br), () => new LandGolfBallInCup()),
            F(PacketTypes.FinishedConnectingToServer, br => new FinishedConnectingToServer(br), () => new FinishedConnectingToServer()),
            F(PacketTypes.FishOutNPC, br => new FishOutNPC(br), () => new FishOutNPC()),
            V(PacketTypes.TamperWithNPC, br => new TamperWithNPC(br), () => new TamperWithNPC()),
            V(PacketTypes.PlayLegacySound, br => new PlayLegacySound(br), () => new PlayLegacySound()),
            V(PacketTypes.FoodPlatterTryPlacing, br => new FoodPlatterTryPlacing(br), () => new FoodPlatterTryPlacing()),
            F(PacketTypes.UpdatePlayerLuckFactors, br => new UpdatePlayerLuckFactors(br), () => new UpdatePlayerLuckFactors()),
            F(PacketTypes.DeadPlayer, br => new DeadPlayer(br), () => new DeadPlayer()),
            F(PacketTypes.SyncCavernMonsterType, br => new SyncCavernMonsterType(br), () => new SyncCavernMonsterType()),
            F(PacketTypes.RequestNPCBuffRemoval, br => new RequestNPCBuffRemoval(br), () => new RequestNPCBuffRemoval()),
            F(PacketTypes.ClientSyncedInventory, br => new ClientSyncedInventory(br), () => new ClientSyncedInventory()),
            F(PacketTypes.SetCountsAsHostForGameplay, br => new SetCountsAsHostForGameplay(br), () => new SetCountsAsHostForGameplay()),
            F(PacketTypes.SetMiscEventValues, br => new SetMiscEventValues(br), () => new SetMiscEventValues()),
            F(PacketTypes.RequestLucyPopup, br => new RequestLucyPopup(br), () => new RequestLucyPopup()),
            V(PacketTypes.SyncProjectileTrackers, br => new SyncProjectileTrackers(br), () => new SyncProjectileTrackers()),
            F(PacketTypes.CrystalInvasionRequestedToSkipWaitTime, br => new CrystalInvasionRequestedToSkipWaitTime(br), () => new CrystalInvasionRequestedToSkipWaitTime()),
            F(PacketTypes.RequestQuestEffect, br => new RequestQuestEffect(br), () => new RequestQuestEffect()),
            V(PacketTypes.ShimmerActions, br => new ShimmerActions(br), () => new ShimmerActions()),
            F(PacketTypes.SyncLoadout, br => new SyncLoadout(br), () => new SyncLoadout()),
            V(PacketTypes.TEDeadCellsDisplayJar, br => new TEDeadCellsDisplayJar(br), () => new TEDeadCellsDisplayJar()),
            F(PacketTypes.SpectatePlayer, br => new SpectatePlayer(br), () => new SpectatePlayer()),
            F(PacketTypes.SyncItemDespawn, br => new SyncItemDespawn(br), () => new SyncItemDespawn()),
            F(PacketTypes.ItemUseSound, br => new ItemUseSound(br), () => new ItemUseSound()),
            F(PacketTypes.NpcHurtByDebuff, br => new NpcHurtByDebuff(br), () => new NpcHurtByDebuff()),
            F(PacketTypes.Ping, br => new Ping(br), () => new Ping()),
            F(PacketTypes.SyncChestSize, br => new SyncChestSize(br), () => new SyncChestSize()),
            F(PacketTypes.TELeashedEntityAnchorPlaceItem, br => new TELeashedEntityAnchorPlaceItem(br), () => new TELeashedEntityAnchorPlaceItem()),
            F(PacketTypes.TeamChangeFromUI, br => new TeamChangeFromUI(br), () => new TeamChangeFromUI()),
            F(PacketTypes.ExtraSpawnSectionLoaded, br => new ExtraSpawnSectionLoaded(br), () => new ExtraSpawnSectionLoaded()),
            F(PacketTypes.RequestSection, br => new RequestSection(br), () => new RequestSection()),
            F(PacketTypes.SyncItemPosition, br => new SyncItemPosition(br), () => new SyncItemPosition()),
            V(PacketTypes.HostToken, br => new HostToken(br), () => new HostToken()),
            F(PacketTypes.DamageNPCAck, br => new DamageNPCAck(br), () => new DamageNPCAck())
        };

        public static Dictionary<PacketTypes, Func<BinaryReader, TerrariaPacket>> CreateDeserializerMap()
        {
            Dictionary<PacketTypes, Func<BinaryReader, TerrariaPacket>> map =
                new Dictionary<PacketTypes, Func<BinaryReader, TerrariaPacket>>(Registrations.Length);

            for (int i = 0; i < Registrations.Length; i++)
            {
                PacketRegistration registration = Registrations[i];
                map.Add(registration.PacketType, registration.Deserialize);
            }

            return map;
        }

        public static IReadOnlyDictionary<PacketTypes, PacketPayloadMetadata> CreatePayloadMetadataMap()
        {
            Dictionary<PacketTypes, PacketPayloadMetadata> map =
                new Dictionary<PacketTypes, PacketPayloadMetadata>(Registrations.Length);

            for (int i = 0; i < Registrations.Length; i++)
            {
                PacketRegistration registration = Registrations[i];
                map.Add(registration.PacketType, registration.CreateMetadata());
            }

            return map;
        }

        private static PacketRegistration F<TPacket>(
            PacketTypes packetType,
            Func<BinaryReader, TPacket> deserialize,
            Func<TPacket> createDefault)
            where TPacket : TerrariaPacket =>
            PacketRegistration.Fixed(packetType, deserialize, createDefault);

        private static PacketRegistration V<TPacket>(
            PacketTypes packetType,
            Func<BinaryReader, TPacket> deserialize,
            Func<TPacket> createDefault)
            where TPacket : TerrariaPacket =>
            PacketRegistration.Variable(packetType, deserialize, createDefault);
    }
}
