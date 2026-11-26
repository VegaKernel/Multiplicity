using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerUpdateView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerUpdateView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerUpdate, payload))
        {
        }

        public static PlayerUpdateView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerUpdateView(payload);
        }

        public PlayerUpdateView(PacketView packet)
            : this(packet, allowTrailing: false, out _)
        {
        }

        internal static int ReadPayloadPrefixLength(PacketView packet)
        {
            _ = new PlayerUpdateView(packet, allowTrailing: true, out int consumed);
            return consumed;
        }

        private PlayerUpdateView(PacketView packet, bool allowTrailing, out int consumed)
        {
            packet.EnsureType(PacketTypes.PlayerUpdate);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            UpdatePlayerControlFlags controlFlags = (UpdatePlayerControlFlags)reader.ReadByte();
            UpdatePlayerMovementFlags movementFlags = (UpdatePlayerMovementFlags)reader.ReadByte();
            UpdatePlayerMiscFlags1 miscFlags1 = (UpdatePlayerMiscFlags1)reader.ReadByte();
            UpdatePlayerMiscFlags2 miscFlags2 = (UpdatePlayerMiscFlags2)reader.ReadByte();
            byte selectedItem = reader.ReadByte();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = 0f;
            float velocityY = 0f;
            ushort mountType = 0;
            float potionOfReturnOriginalPositionX = 0f;
            float potionOfReturnOriginalPositionY = 0f;
            float potionOfReturnHomePositionX = 0f;
            float potionOfReturnHomePositionY = 0f;
            float cameraTargetX = 0f;
            float cameraTargetY = 0f;

            if ((movementFlags & UpdatePlayerMovementFlags.HasVelocity) != 0)
            {
                velocityX = reader.ReadSingle();
                velocityY = reader.ReadSingle();
            }

            if ((movementFlags & UpdatePlayerMovementFlags.HasMount) != 0)
            {
                mountType = reader.ReadUInt16();
            }

            if ((miscFlags1 & UpdatePlayerMiscFlags1.HasPotionOfReturnPositions) != 0)
            {
                potionOfReturnOriginalPositionX = reader.ReadSingle();
                potionOfReturnOriginalPositionY = reader.ReadSingle();
                potionOfReturnHomePositionX = reader.ReadSingle();
                potionOfReturnHomePositionY = reader.ReadSingle();
            }

            if ((miscFlags2 & UpdatePlayerMiscFlags2.HasCameraTarget) != 0)
            {
                cameraTargetX = reader.ReadSingle();
                cameraTargetY = reader.ReadSingle();
            }

            if (!allowTrailing)
            {
                reader.EnsureEnd();
            }

            consumed = reader.Consumed;

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            ControlFlags = controlFlags;
            MovementFlags = movementFlags;
            MiscFlags1 = miscFlags1;
            MiscFlags2 = miscFlags2;
            SelectedItem = selectedItem;
            PositionX = positionX;
            PositionY = positionY;
            VelocityX = velocityX;
            VelocityY = velocityY;
            MountType = mountType;
            PotionOfReturnOriginalPositionX = potionOfReturnOriginalPositionX;
            PotionOfReturnOriginalPositionY = potionOfReturnOriginalPositionY;
            PotionOfReturnHomePositionX = potionOfReturnHomePositionX;
            PotionOfReturnHomePositionY = potionOfReturnHomePositionY;
            CameraTargetX = cameraTargetX;
            CameraTargetY = cameraTargetY;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public UpdatePlayerControlFlags ControlFlags { get; }

        public UpdatePlayerMovementFlags MovementFlags { get; }

        public UpdatePlayerMiscFlags1 MiscFlags1 { get; }

        public UpdatePlayerMiscFlags2 MiscFlags2 { get; }

        public byte SelectedItem { get; }

        public float PositionX { get; }

        public float PositionY { get; }

        public float VelocityX { get; }

        public float VelocityY { get; }

        public ushort MountType { get; }

        public float PotionOfReturnOriginalPositionX { get; }

        public float PotionOfReturnOriginalPositionY { get; }

        public float PotionOfReturnHomePositionX { get; }

        public float PotionOfReturnHomePositionY { get; }

        public float CameraTargetX { get; }

        public float CameraTargetY { get; }

        public bool HasVelocity => (MovementFlags & UpdatePlayerMovementFlags.HasVelocity) != 0;

        public bool HasMount => (MovementFlags & UpdatePlayerMovementFlags.HasMount) != 0;

        public bool HasPotionOfReturnPositions => (MiscFlags1 & UpdatePlayerMiscFlags1.HasPotionOfReturnPositions) != 0;

        public bool HasCameraTarget => (MiscFlags2 & UpdatePlayerMiscFlags2.HasCameraTarget) != 0;
    }
}



