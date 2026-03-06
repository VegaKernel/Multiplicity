namespace Multiplicity.Packets.Views
{
    public static class PacketViewExtensions
    {
        public static StringPacketView AsStringPacketView(this PacketView packet)
        {
            return new StringPacketView(packet);
        }

        public static GetSectionView AsGetSectionView(this PacketView packet)
        {
            return new GetSectionView(packet);
        }

        public static PlayerInfoView AsPlayerInfoView(this PacketView packet)
        {
            return new PlayerInfoView(packet);
        }

        public static PlayerInventorySlotView AsPlayerInventorySlotView(this PacketView packet)
        {
            return new PlayerInventorySlotView(packet);
        }

        public static SpawnPlayerView AsSpawnPlayerView(this PacketView packet)
        {
            return new SpawnPlayerView(packet);
        }

        public static UpdatePlayerView AsUpdatePlayerView(this PacketView packet)
        {
            return new UpdatePlayerView(packet);
        }

        public static PlayerActiveView AsPlayerActiveView(this PacketView packet)
        {
            return new PlayerActiveView(packet);
        }

        public static PlayerHpView AsPlayerHpView(this PacketView packet)
        {
            return new PlayerHpView(packet);
        }

        public static ModifyTileView AsModifyTileView(this PacketView packet)
        {
            return new ModifyTileView(packet);
        }

        public static ChatMessageView AsChatMessageView(this PacketView packet)
        {
            return new ChatMessageView(packet);
        }

        public static PlayerDamageView AsPlayerDamageView(this PacketView packet)
        {
            return new PlayerDamageView(packet);
        }

        public static ProjectileUpdateView AsProjectileUpdateView(this PacketView packet)
        {
            return new ProjectileUpdateView(packet);
        }

        public static TogglePvpView AsTogglePvpView(this PacketView packet)
        {
            return new TogglePvpView(packet);
        }

        public static PlayerZoneView AsPlayerZoneView(this PacketView packet)
        {
            return new PlayerZoneView(packet);
        }

        public static PlayerItemAnimationView AsPlayerItemAnimationView(this PacketView packet)
        {
            return new PlayerItemAnimationView(packet);
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

        public static FishOutNpcView AsFishOutNpcView(this PacketView packet)
        {
            return new FishOutNpcView(packet);
        }

        public static TamperWithNpcView AsTamperWithNpcView(this PacketView packet)
        {
            return new TamperWithNpcView(packet);
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

        public static RequestNpcBuffRemovalView AsRequestNpcBuffRemovalView(this PacketView packet)
        {
            return new RequestNpcBuffRemovalView(packet);
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

        public static TileEntityItemSyncPacketView AsTeDeadCellsDisplayJarView(this PacketView packet)
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

        public static TeLeashedEntityAnchorPlaceItemView AsTeLeashedEntityAnchorPlaceItemView(this PacketView packet)
        {
            return new TeLeashedEntityAnchorPlaceItemView(packet);
        }

        public static TeamChangeFromUiView AsTeamChangeFromUiView(this PacketView packet)
        {
            return new TeamChangeFromUiView(packet);
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

