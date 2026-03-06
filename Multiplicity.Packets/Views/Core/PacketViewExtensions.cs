namespace Multiplicity.Packets.Views
{
    public static class PacketViewExtensions
    {
        public static StringPacketView AsStringPacketView(this PacketView packet)
        {
            return new StringPacketView(packet);
        }

        public static TileGetSectionView AsTileGetSectionView(this PacketView packet)
        {
            return new TileGetSectionView(packet);
        }

        public static PlayerInfoView AsPlayerInfoView(this PacketView packet)
        {
            return new PlayerInfoView(packet);
        }

        public static PlayerSlotView AsPlayerSlotView(this PacketView packet)
        {
            return new PlayerSlotView(packet);
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

        public static TogglePvpView AsTogglePvpView(this PacketView packet)
        {
            return new TogglePvpView(packet);
        }

        public static ZonesView AsZonesView(this PacketView packet)
        {
            return new ZonesView(packet);
        }

        public static PlayerAnimationView AsPlayerAnimationView(this PacketView packet)
        {
            return new PlayerAnimationView(packet);
        }

        public static PlayerManaView AsPlayerManaView(this PacketView packet)
        {
            return new PlayerManaView(packet);
        }

        public static PlayerTeamView AsPlayerTeamView(this PacketView packet)
        {
            return new PlayerTeamView(packet);
        }

        public static PlaceObjectView AsPlaceObjectView(this PacketView packet)
        {
            return new PlaceObjectView(packet);
        }

        public static LoadNetModuleView AsLoadNetModuleView(this PacketView packet)
        {
            return new LoadNetModuleView(packet);
        }

        public static NpcUpdateView AsNpcUpdateView(this PacketView packet)
        {
            return new NpcUpdateView(packet);
        }

        public static WorldItemSyncView AsWorldItemSyncView(this PacketView packet)
        {
            return new WorldItemSyncView(packet);
        }

        public static PlayerHurtV2View AsPlayerHurtV2View(this PacketView packet)
        {
            return new PlayerHurtV2View(packet);
        }

        public static PlayerDeathV2View AsPlayerDeathV2View(this PacketView packet)
        {
            return new PlayerDeathV2View(packet);
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

        public static TileEntityItemSyncPacketView AsWeaponsRackTryPlacingView(this PacketView packet)
        {
            return new TileEntityItemSyncPacketView(packet, PacketTypes.WeaponsRackTryPlacing);
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

        public static EmptyPayloadPacketView AsFinishedConnectingToServerView(this PacketView packet)
        {
            return new EmptyPayloadPacketView(packet, PacketTypes.FinishedConnectingToServer);
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

        public static TileEntityItemSyncPacketView AsFoodPlatterTryPlacingView(this PacketView packet)
        {
            return new TileEntityItemSyncPacketView(packet, PacketTypes.FoodPlatterTryPlacing);
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

        public static EmptyPayloadPacketView AsClientSyncedInventoryView(this PacketView packet)
        {
            return new EmptyPayloadPacketView(packet, PacketTypes.ClientSyncedInventory);
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

        public static EmptyPayloadPacketView AsCrystalInvasionRequestedToSkipWaitTimeView(this PacketView packet)
        {
            return new EmptyPayloadPacketView(packet, PacketTypes.CrystalInvasionRequestedToSkipWaitTime);
        }

        public static EmptyPayloadPacketView AsRequestQuestEffectView(this PacketView packet)
        {
            return new EmptyPayloadPacketView(packet, PacketTypes.RequestQuestEffect);
        }

        public static ShimmerActionsView AsShimmerActionsView(this PacketView packet)
        {
            return new ShimmerActionsView(packet);
        }

        public static SyncLoadoutView AsSyncLoadoutView(this PacketView packet)
        {
            return new SyncLoadoutView(packet);
        }

        public static TileEntityItemSyncPacketView AsTEDeadCellsDisplayJarView(this PacketView packet)
        {
            return new TileEntityItemSyncPacketView(packet, PacketTypes.TEDeadCellsDisplayJar);
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

        public static EmptyPayloadPacketView AsPingView(this PacketView packet)
        {
            return new EmptyPayloadPacketView(packet, PacketTypes.Ping);
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
    }
}


