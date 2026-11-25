namespace Multiplicity.Packets.Views
{
    public static class PacketViewExtensions
    {
        public static StringPacketView AsStringPacketView(this PacketView packet)
        {
            return new StringPacketView(packet);
        }

        public static WorldItemSyncView AsWorldItemSyncView(this PacketView packet)
        {
            return new WorldItemSyncView(packet);
        }

        public static TileEntityItemSyncPacketView AsTileEntityItemSyncPacketView(this PacketView packet, PacketTypes expectedPacketType)
        {
            return new TileEntityItemSyncPacketView(packet, expectedPacketType);
        }

        public static EmptyPayloadPacketView AsEmptyPayloadPacketView(this PacketView packet, PacketTypes expectedPacketType)
        {
            return new EmptyPayloadPacketView(packet, expectedPacketType);
        }

        public static ConnectRequestView AsConnectRequestView(this PacketView packet)
        {
            return new ConnectRequestView(packet);
        }

        public static DisconnectView AsDisconnectView(this PacketView packet)
        {
            return new DisconnectView(packet);
        }

        public static ContinueConnectingView AsContinueConnectingView(this PacketView packet)
        {
            return new ContinueConnectingView(packet);
        }

        public static PlayerInfoView AsPlayerInfoView(this PacketView packet)
        {
            return new PlayerInfoView(packet);
        }

        public static PlayerSlotView AsPlayerSlotView(this PacketView packet)
        {
            return new PlayerSlotView(packet);
        }

        public static ContinueConnecting2View AsContinueConnecting2View(this PacketView packet)
        {
            return new ContinueConnecting2View(packet);
        }

        public static WorldInfoView AsWorldInfoView(this PacketView packet)
        {
            return new WorldInfoView(packet);
        }

        public static TileGetSectionView AsTileGetSectionView(this PacketView packet)
        {
            return new TileGetSectionView(packet);
        }

        public static StatusView AsStatusView(this PacketView packet)
        {
            return new StatusView(packet);
        }

        public static TileSendSectionView AsTileSendSectionView(this PacketView packet)
        {
            return new TileSendSectionView(packet);
        }

        public static TileFrameSectionView AsTileFrameSectionView(this PacketView packet)
        {
            return new TileFrameSectionView(packet);
        }

        public static PlayerSpawnView AsPlayerSpawnView(this PacketView packet)
        {
            return new PlayerSpawnView(packet);
        }

        public static PlayerUpdateView AsPlayerUpdateView(this PacketView packet)
        {
            return new PlayerUpdateView(packet);
        }

        public static PlayerActiveView AsPlayerActiveView(this PacketView packet)
        {
            return new PlayerActiveView(packet);
        }

        public static PlayerHpView AsPlayerHpView(this PacketView packet)
        {
            return new PlayerHpView(packet);
        }

        public static TileView AsTileView(this PacketView packet)
        {
            return new TileView(packet);
        }

        public static TimeSetView AsTimeSetView(this PacketView packet)
        {
            return new TimeSetView(packet);
        }

        public static DoorUseView AsDoorUseView(this PacketView packet)
        {
            return new DoorUseView(packet);
        }

        public static TileSendSquareView AsTileSendSquareView(this PacketView packet)
        {
            return new TileSendSquareView(packet);
        }

        public static ItemDropView AsItemDropView(this PacketView packet)
        {
            return new ItemDropView(packet);
        }

        public static ItemOwnerView AsItemOwnerView(this PacketView packet)
        {
            return new ItemOwnerView(packet);
        }

        public static NpcUpdateView AsNpcUpdateView(this PacketView packet)
        {
            return new NpcUpdateView(packet);
        }

        public static NpcItemStrikeView AsNpcItemStrikeView(this PacketView packet)
        {
            return new NpcItemStrikeView(packet);
        }

        public static ChatMessageView AsChatMessageView(this PacketView packet)
        {
            return new ChatMessageView(packet);
        }

        public static PlayerDamageView AsPlayerDamageView(this PacketView packet)
        {
            return new PlayerDamageView(packet);
        }

        public static ProjectileNewView AsProjectileNewView(this PacketView packet)
        {
            return new ProjectileNewView(packet);
        }

        public static NpcStrikeView AsNpcStrikeView(this PacketView packet)
        {
            return new NpcStrikeView(packet);
        }

        public static ProjectileDestroyView AsProjectileDestroyView(this PacketView packet)
        {
            return new ProjectileDestroyView(packet);
        }

        public static TogglePvpView AsTogglePvpView(this PacketView packet)
        {
            return new TogglePvpView(packet);
        }

        public static ChestGetContentsView AsChestGetContentsView(this PacketView packet)
        {
            return new ChestGetContentsView(packet);
        }

        public static ChestItemView AsChestItemView(this PacketView packet)
        {
            return new ChestItemView(packet);
        }

        public static ChestOpenView AsChestOpenView(this PacketView packet)
        {
            return new ChestOpenView(packet);
        }

        public static PlaceChestView AsPlaceChestView(this PacketView packet)
        {
            return new PlaceChestView(packet);
        }

        public static EffectHealView AsEffectHealView(this PacketView packet)
        {
            return new EffectHealView(packet);
        }

        public static ZonesView AsZonesView(this PacketView packet)
        {
            return new ZonesView(packet);
        }

        public static PasswordRequiredView AsPasswordRequiredView(this PacketView packet)
        {
            return new PasswordRequiredView(packet);
        }

        public static PasswordSendView AsPasswordSendView(this PacketView packet)
        {
            return new PasswordSendView(packet);
        }

        public static RemoveItemOwnerView AsRemoveItemOwnerView(this PacketView packet)
        {
            return new RemoveItemOwnerView(packet);
        }

        public static NpcTalkView AsNpcTalkView(this PacketView packet)
        {
            return new NpcTalkView(packet);
        }

        public static PlayerAnimationView AsPlayerAnimationView(this PacketView packet)
        {
            return new PlayerAnimationView(packet);
        }

        public static PlayerManaView AsPlayerManaView(this PacketView packet)
        {
            return new PlayerManaView(packet);
        }

        public static EffectManaView AsEffectManaView(this PacketView packet)
        {
            return new EffectManaView(packet);
        }

        public static PlayerDeathView AsPlayerDeathView(this PacketView packet)
        {
            return new PlayerDeathView(packet);
        }

        public static PlayerTeamView AsPlayerTeamView(this PacketView packet)
        {
            return new PlayerTeamView(packet);
        }

        public static SignReadView AsSignReadView(this PacketView packet)
        {
            return new SignReadView(packet);
        }

        public static SignNewView AsSignNewView(this PacketView packet)
        {
            return new SignNewView(packet);
        }

        public static LiquidSetView AsLiquidSetView(this PacketView packet)
        {
            return new LiquidSetView(packet);
        }

        public static PlayerSpawnSelfView AsPlayerSpawnSelfView(this PacketView packet)
        {
            return new PlayerSpawnSelfView(packet);
        }

        public static PlayerBuffView AsPlayerBuffView(this PacketView packet)
        {
            return new PlayerBuffView(packet);
        }

        public static NpcSpecialView AsNpcSpecialView(this PacketView packet)
        {
            return new NpcSpecialView(packet);
        }

        public static ChestUnlockView AsChestUnlockView(this PacketView packet)
        {
            return new ChestUnlockView(packet);
        }

        public static NpcAddBuffView AsNpcAddBuffView(this PacketView packet)
        {
            return new NpcAddBuffView(packet);
        }

        public static NpcUpdateBuffView AsNpcUpdateBuffView(this PacketView packet)
        {
            return new NpcUpdateBuffView(packet);
        }

        public static PlayerAddBuffView AsPlayerAddBuffView(this PacketView packet)
        {
            return new PlayerAddBuffView(packet);
        }

        public static UpdateNPCNameView AsUpdateNPCNameView(this PacketView packet)
        {
            return new UpdateNPCNameView(packet);
        }

        public static UpdateGoodEvilView AsUpdateGoodEvilView(this PacketView packet)
        {
            return new UpdateGoodEvilView(packet);
        }

        public static PlayHarpView AsPlayHarpView(this PacketView packet)
        {
            return new PlayHarpView(packet);
        }

        public static HitSwitchView AsHitSwitchView(this PacketView packet)
        {
            return new HitSwitchView(packet);
        }

        public static UpdateNPCHomeView AsUpdateNPCHomeView(this PacketView packet)
        {
            return new UpdateNPCHomeView(packet);
        }

        public static SpawnBossorInvasionView AsSpawnBossorInvasionView(this PacketView packet)
        {
            return new SpawnBossorInvasionView(packet);
        }

        public static PlayerDodgeView AsPlayerDodgeView(this PacketView packet)
        {
            return new PlayerDodgeView(packet);
        }

        public static PaintTileView AsPaintTileView(this PacketView packet)
        {
            return new PaintTileView(packet);
        }

        public static PaintWallView AsPaintWallView(this PacketView packet)
        {
            return new PaintWallView(packet);
        }

        public static TeleportView AsTeleportView(this PacketView packet)
        {
            return new TeleportView(packet);
        }

        public static PlayerHealOtherView AsPlayerHealOtherView(this PacketView packet)
        {
            return new PlayerHealOtherView(packet);
        }

        public static PlaceholderView AsPlaceholderView(this PacketView packet)
        {
            return new PlaceholderView(packet);
        }

        public static ClientUUIDView AsClientUUIDView(this PacketView packet)
        {
            return new ClientUUIDView(packet);
        }

        public static ChestNameView AsChestNameView(this PacketView packet)
        {
            return new ChestNameView(packet);
        }

        public static CatchNPCView AsCatchNPCView(this PacketView packet)
        {
            return new CatchNPCView(packet);
        }

        public static ReleaseNPCView AsReleaseNPCView(this PacketView packet)
        {
            return new ReleaseNPCView(packet);
        }

        public static TravellingMerchantInventoryView AsTravellingMerchantInventoryView(this PacketView packet)
        {
            return new TravellingMerchantInventoryView(packet);
        }

        public static TeleportationPotionView AsTeleportationPotionView(this PacketView packet)
        {
            return new TeleportationPotionView(packet);
        }

        public static AnglerQuestView AsAnglerQuestView(this PacketView packet)
        {
            return new AnglerQuestView(packet);
        }

        public static CompleteAnglerQuestView AsCompleteAnglerQuestView(this PacketView packet)
        {
            return new CompleteAnglerQuestView(packet);
        }

        public static NumberOfAnglerQuestsCompletedView AsNumberOfAnglerQuestsCompletedView(this PacketView packet)
        {
            return new NumberOfAnglerQuestsCompletedView(packet);
        }

        public static CreateTemporaryAnimationView AsCreateTemporaryAnimationView(this PacketView packet)
        {
            return new CreateTemporaryAnimationView(packet);
        }

        public static ReportInvasionProgressView AsReportInvasionProgressView(this PacketView packet)
        {
            return new ReportInvasionProgressView(packet);
        }

        public static PlaceObjectView AsPlaceObjectView(this PacketView packet)
        {
            return new PlaceObjectView(packet);
        }

        public static SyncPlayerChestIndexView AsSyncPlayerChestIndexView(this PacketView packet)
        {
            return new SyncPlayerChestIndexView(packet);
        }

        public static CreateCombatTextView AsCreateCombatTextView(this PacketView packet)
        {
            return new CreateCombatTextView(packet);
        }

        public static LoadNetModuleView AsLoadNetModuleView(this PacketView packet)
        {
            return new LoadNetModuleView(packet);
        }

        public static NpcKillCountView AsNpcKillCountView(this PacketView packet)
        {
            return new NpcKillCountView(packet);
        }

        public static PlayerStealthView AsPlayerStealthView(this PacketView packet)
        {
            return new PlayerStealthView(packet);
        }

        public static ForceItemIntoNearestChestView AsForceItemIntoNearestChestView(this PacketView packet)
        {
            return new ForceItemIntoNearestChestView(packet);
        }

        public static UpdateTileEntityView AsUpdateTileEntityView(this PacketView packet)
        {
            return new UpdateTileEntityView(packet);
        }

        public static PlaceTileEntityView AsPlaceTileEntityView(this PacketView packet)
        {
            return new PlaceTileEntityView(packet);
        }

        public static TweakItemView AsTweakItemView(this PacketView packet)
        {
            return new TweakItemView(packet);
        }

        public static PlaceItemFrameView AsPlaceItemFrameView(this PacketView packet)
        {
            return new PlaceItemFrameView(packet);
        }

        public static UpdateItemDropView AsUpdateItemDropView(this PacketView packet)
        {
            return new UpdateItemDropView(packet);
        }

        public static EmoteBubbleView AsEmoteBubbleView(this PacketView packet)
        {
            return new EmoteBubbleView(packet);
        }

        public static SyncExtraValueView AsSyncExtraValueView(this PacketView packet)
        {
            return new SyncExtraValueView(packet);
        }

        public static SocialHandshakeView AsSocialHandshakeView(this PacketView packet)
        {
            return new SocialHandshakeView(packet);
        }

        public static DebugCommandsView AsDebugCommandsView(this PacketView packet)
        {
            return new DebugCommandsView(packet);
        }

        public static KillPortalView AsKillPortalView(this PacketView packet)
        {
            return new KillPortalView(packet);
        }

        public static PlayerTeleportPortalView AsPlayerTeleportPortalView(this PacketView packet)
        {
            return new PlayerTeleportPortalView(packet);
        }

        public static NotifyPlayerNpcKilledView AsNotifyPlayerNpcKilledView(this PacketView packet)
        {
            return new NotifyPlayerNpcKilledView(packet);
        }

        public static NotifyPlayerOfEventView AsNotifyPlayerOfEventView(this PacketView packet)
        {
            return new NotifyPlayerOfEventView(packet);
        }

        public static UpdateMinionTargetView AsUpdateMinionTargetView(this PacketView packet)
        {
            return new UpdateMinionTargetView(packet);
        }

        public static NpcTeleportPortalView AsNpcTeleportPortalView(this PacketView packet)
        {
            return new NpcTeleportPortalView(packet);
        }

        public static UpdateShieldStrengthsView AsUpdateShieldStrengthsView(this PacketView packet)
        {
            return new UpdateShieldStrengthsView(packet);
        }

        public static NebulaLevelUpView AsNebulaLevelUpView(this PacketView packet)
        {
            return new NebulaLevelUpView(packet);
        }

        public static MoonLordCountdownView AsMoonLordCountdownView(this PacketView packet)
        {
            return new MoonLordCountdownView(packet);
        }

        public static NpcShopItemView AsNpcShopItemView(this PacketView packet)
        {
            return new NpcShopItemView(packet);
        }

        public static GemLockToggleView AsGemLockToggleView(this PacketView packet)
        {
            return new GemLockToggleView(packet);
        }

        public static PoofOfSmokeView AsPoofOfSmokeView(this PacketView packet)
        {
            return new PoofOfSmokeView(packet);
        }

        public static SmartTextMessageView AsSmartTextMessageView(this PacketView packet)
        {
            return new SmartTextMessageView(packet);
        }

        public static WiredCannonShotView AsWiredCannonShotView(this PacketView packet)
        {
            return new WiredCannonShotView(packet);
        }

        public static MassWireOperationView AsMassWireOperationView(this PacketView packet)
        {
            return new MassWireOperationView(packet);
        }

        public static MassWireOperationPayView AsMassWireOperationPayView(this PacketView packet)
        {
            return new MassWireOperationPayView(packet);
        }

        public static TogglePartyView AsTogglePartyView(this PacketView packet)
        {
            return new TogglePartyView(packet);
        }

        public static TreeGrowFXView AsTreeGrowFXView(this PacketView packet)
        {
            return new TreeGrowFXView(packet);
        }

        public static CrystalInvasionStartView AsCrystalInvasionStartView(this PacketView packet)
        {
            return new CrystalInvasionStartView(packet);
        }

        public static CrystalInvasionWipeAllView AsCrystalInvasionWipeAllView(this PacketView packet)
        {
            return new CrystalInvasionWipeAllView(packet);
        }

        public static MinionAttackTargetUpdateView AsMinionAttackTargetUpdateView(this PacketView packet)
        {
            return new MinionAttackTargetUpdateView(packet);
        }

        public static CrystalInvasionSendWaitTimeView AsCrystalInvasionSendWaitTimeView(this PacketView packet)
        {
            return new CrystalInvasionSendWaitTimeView(packet);
        }

        public static PlayerHurtV2View AsPlayerHurtV2View(this PacketView packet)
        {
            return new PlayerHurtV2View(packet);
        }

        public static PlayerDeathV2View AsPlayerDeathV2View(this PacketView packet)
        {
            return new PlayerDeathV2View(packet);
        }

        public static CreateCombatTextExtendedView AsCreateCombatTextExtendedView(this PacketView packet)
        {
            return new CreateCombatTextExtendedView(packet);
        }

        public static EmojiView AsEmojiView(this PacketView packet)
        {
            return new EmojiView(packet);
        }

        public static TileEntityDisplayDollItemSyncView AsTileEntityDisplayDollItemSyncView(this PacketView packet)
        {
            return new TileEntityDisplayDollItemSyncView(packet);
        }

        public static RequestTileEntityInteractionView AsRequestTileEntityInteractionView(this PacketView packet)
        {
            return new RequestTileEntityInteractionView(packet);
        }

        public static WeaponsRackTryPlacingView AsWeaponsRackTryPlacingView(this PacketView packet)
        {
            return new WeaponsRackTryPlacingView(packet);
        }

        public static TileEntityHatRackItemSyncView AsTileEntityHatRackItemSyncView(this PacketView packet)
        {
            return new TileEntityHatRackItemSyncView(packet);
        }

        public static SyncTilePickingView AsSyncTilePickingView(this PacketView packet)
        {
            return new SyncTilePickingView(packet);
        }

        public static SyncRevengeMarkerView AsSyncRevengeMarkerView(this PacketView packet)
        {
            return new SyncRevengeMarkerView(packet);
        }

        public static RemoveRevengeMarkerView AsRemoveRevengeMarkerView(this PacketView packet)
        {
            return new RemoveRevengeMarkerView(packet);
        }

        public static LandGolfBallInCupView AsLandGolfBallInCupView(this PacketView packet)
        {
            return new LandGolfBallInCupView(packet);
        }

        public static FinishedConnectingToServerView AsFinishedConnectingToServerView(this PacketView packet)
        {
            return new FinishedConnectingToServerView(packet);
        }

        public static FishOutNPCView AsFishOutNPCView(this PacketView packet)
        {
            return new FishOutNPCView(packet);
        }

        public static TamperWithNPCView AsTamperWithNPCView(this PacketView packet)
        {
            return new TamperWithNPCView(packet);
        }

        public static PlayLegacySoundView AsPlayLegacySoundView(this PacketView packet)
        {
            return new PlayLegacySoundView(packet);
        }

        public static FoodPlatterTryPlacingView AsFoodPlatterTryPlacingView(this PacketView packet)
        {
            return new FoodPlatterTryPlacingView(packet);
        }

        public static UpdatePlayerLuckFactorsView AsUpdatePlayerLuckFactorsView(this PacketView packet)
        {
            return new UpdatePlayerLuckFactorsView(packet);
        }

        public static DeadPlayerView AsDeadPlayerView(this PacketView packet)
        {
            return new DeadPlayerView(packet);
        }

        public static SyncCavernMonsterTypeView AsSyncCavernMonsterTypeView(this PacketView packet)
        {
            return new SyncCavernMonsterTypeView(packet);
        }

        public static RequestNPCBuffRemovalView AsRequestNPCBuffRemovalView(this PacketView packet)
        {
            return new RequestNPCBuffRemovalView(packet);
        }

        public static ClientSyncedInventoryView AsClientSyncedInventoryView(this PacketView packet)
        {
            return new ClientSyncedInventoryView(packet);
        }

        public static SetCountsAsHostForGameplayView AsSetCountsAsHostForGameplayView(this PacketView packet)
        {
            return new SetCountsAsHostForGameplayView(packet);
        }

        public static SetMiscEventValuesView AsSetMiscEventValuesView(this PacketView packet)
        {
            return new SetMiscEventValuesView(packet);
        }

        public static RequestLucyPopupView AsRequestLucyPopupView(this PacketView packet)
        {
            return new RequestLucyPopupView(packet);
        }

        public static SyncProjectileTrackersView AsSyncProjectileTrackersView(this PacketView packet)
        {
            return new SyncProjectileTrackersView(packet);
        }

        public static CrystalInvasionRequestedToSkipWaitTimeView AsCrystalInvasionRequestedToSkipWaitTimeView(this PacketView packet)
        {
            return new CrystalInvasionRequestedToSkipWaitTimeView(packet);
        }

        public static RequestQuestEffectView AsRequestQuestEffectView(this PacketView packet)
        {
            return new RequestQuestEffectView(packet);
        }

        public static SyncItemsWithShimmerView AsSyncItemsWithShimmerView(this PacketView packet)
        {
            return new SyncItemsWithShimmerView(packet);
        }

        public static ShimmerActionsView AsShimmerActionsView(this PacketView packet)
        {
            return new ShimmerActionsView(packet);
        }

        public static SyncLoadoutView AsSyncLoadoutView(this PacketView packet)
        {
            return new SyncLoadoutView(packet);
        }

        public static SyncItemCannotBeTakenByEnemiesView AsSyncItemCannotBeTakenByEnemiesView(this PacketView packet)
        {
            return new SyncItemCannotBeTakenByEnemiesView(packet);
        }

        public static TEDeadCellsDisplayJarView AsTEDeadCellsDisplayJarView(this PacketView packet)
        {
            return new TEDeadCellsDisplayJarView(packet);
        }

        public static SpectatePlayerView AsSpectatePlayerView(this PacketView packet)
        {
            return new SpectatePlayerView(packet);
        }

        public static SyncItemDespawnView AsSyncItemDespawnView(this PacketView packet)
        {
            return new SyncItemDespawnView(packet);
        }

        public static ItemUseSoundView AsItemUseSoundView(this PacketView packet)
        {
            return new ItemUseSoundView(packet);
        }

        public static NpcHurtByDebuffView AsNpcHurtByDebuffView(this PacketView packet)
        {
            return new NpcHurtByDebuffView(packet);
        }

        public static PingView AsPingView(this PacketView packet)
        {
            return new PingView(packet);
        }

        public static SyncChestSizeView AsSyncChestSizeView(this PacketView packet)
        {
            return new SyncChestSizeView(packet);
        }

        public static TELeashedEntityAnchorPlaceItemView AsTELeashedEntityAnchorPlaceItemView(this PacketView packet)
        {
            return new TELeashedEntityAnchorPlaceItemView(packet);
        }

        public static TeamChangeFromUIView AsTeamChangeFromUIView(this PacketView packet)
        {
            return new TeamChangeFromUIView(packet);
        }

        public static ExtraSpawnSectionLoadedView AsExtraSpawnSectionLoadedView(this PacketView packet)
        {
            return new ExtraSpawnSectionLoadedView(packet);
        }

        public static RequestSectionView AsRequestSectionView(this PacketView packet)
        {
            return new RequestSectionView(packet);
        }

        public static SyncItemPositionView AsSyncItemPositionView(this PacketView packet)
        {
            return new SyncItemPositionView(packet);
        }

        public static HostTokenView AsHostTokenView(this PacketView packet)
        {
            return new HostTokenView(packet);
        }

        public static ServerInfoView AsServerInfoView(this PacketView packet)
        {
            return new ServerInfoView(packet);
        }

        public static PlayerPlatformInfoView AsPlayerPlatformInfoView(this PacketView packet)
        {
            return new PlayerPlatformInfoView(packet);
        }

        public static CustomServerAckView AsCustomServerAckView(this PacketView packet)
        {
            return new CustomServerAckView(packet);
        }

    }
}
