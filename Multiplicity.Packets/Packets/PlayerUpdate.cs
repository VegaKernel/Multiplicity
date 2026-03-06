using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerUpdate (0x0D) packet for protocol 317/318.
    /// </summary>
    public class PlayerUpdate : TerrariaPacket
    {
        private const short FixedPayloadLength = 14;
        private const short VelocityPayloadLength = 8;
        private const short MountPayloadLength = 2;
        private const short PotionOfReturnPayloadLength = 16;
        private const short CameraTargetPayloadLength = 8;

        public byte PlayerId { get; set; }
        public UpdatePlayerControlFlags ControlFlags { get; set; }
        public UpdatePlayerMovementFlags MovementFlags { get; set; }
        public UpdatePlayerMiscFlags1 MiscFlags1 { get; set; }
        public UpdatePlayerMiscFlags2 MiscFlags2 { get; set; }
        public byte SelectedItem { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public ushort MountType { get; set; }
        public float PotionOfReturnOriginalPositionX { get; set; }
        public float PotionOfReturnOriginalPositionY { get; set; }
        public float PotionOfReturnHomePositionX { get; set; }
        public float PotionOfReturnHomePositionY { get; set; }
        public float CameraTargetX { get; set; }
        public float CameraTargetY { get; set; }

        public PlayerUpdate()
            : base((byte)PacketTypes.PlayerUpdate)
        {
        }

        public PlayerUpdate(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            ControlFlags = (UpdatePlayerControlFlags)br.ReadByte();
            MovementFlags = (UpdatePlayerMovementFlags)br.ReadByte();
            MiscFlags1 = (UpdatePlayerMiscFlags1)br.ReadByte();
            MiscFlags2 = (UpdatePlayerMiscFlags2)br.ReadByte();
            SelectedItem = br.ReadByte();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();

            if (HasVelocity)
            {
                VelocityX = br.ReadSingle();
                VelocityY = br.ReadSingle();
            }

            if (HasMount)
            {
                MountType = br.ReadUInt16();
            }

            if (HasPotionOfReturnPositions)
            {
                PotionOfReturnOriginalPositionX = br.ReadSingle();
                PotionOfReturnOriginalPositionY = br.ReadSingle();
                PotionOfReturnHomePositionX = br.ReadSingle();
                PotionOfReturnHomePositionY = br.ReadSingle();
            }

            if (HasCameraTarget)
            {
                CameraTargetX = br.ReadSingle();
                CameraTargetY = br.ReadSingle();
            }
        }

        public bool HasVelocity => (MovementFlags & UpdatePlayerMovementFlags.HasVelocity) != 0;
        public bool HasMount => (MovementFlags & UpdatePlayerMovementFlags.HasMount) != 0;
        public bool HasPotionOfReturnPositions => (MiscFlags1 & UpdatePlayerMiscFlags1.HasPotionOfReturnPositions) != 0;
        public bool HasCameraTarget => (MiscFlags2 & UpdatePlayerMiscFlags2.HasCameraTarget) != 0;

        public override short GetLength()
        {
            short length = FixedPayloadLength;

            if (HasVelocity)
            {
                length += VelocityPayloadLength;
            }

            if (HasMount)
            {
                length += MountPayloadLength;
            }

            if (HasPotionOfReturnPositions)
            {
                length += PotionOfReturnPayloadLength;
            }

            if (HasCameraTarget)
            {
                length += CameraTargetPayloadLength;
            }

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(PlayerId);
                bw.Write((byte)ControlFlags);
                bw.Write((byte)MovementFlags);
                bw.Write((byte)MiscFlags1);
                bw.Write((byte)MiscFlags2);
                bw.Write(SelectedItem);
                bw.Write(PositionX);
                bw.Write(PositionY);

                if (HasVelocity)
                {
                    bw.Write(VelocityX);
                    bw.Write(VelocityY);
                }

                if (HasMount)
                {
                    bw.Write(MountType);
                }

                if (HasPotionOfReturnPositions)
                {
                    bw.Write(PotionOfReturnOriginalPositionX);
                    bw.Write(PotionOfReturnOriginalPositionY);
                    bw.Write(PotionOfReturnHomePositionX);
                    bw.Write(PotionOfReturnHomePositionY);
                }

                if (HasCameraTarget)
                {
                    bw.Write(CameraTargetX);
                    bw.Write(CameraTargetY);
                }
            }
        }

        public override string ToString()
        {
            return $"[PlayerUpdate: PlayerId = {PlayerId} ControlFlags = {ControlFlags} MovementFlags = {MovementFlags} MiscFlags1 = {MiscFlags1} MiscFlags2 = {MiscFlags2} SelectedItem = {SelectedItem} PositionX = {PositionX} PositionY = {PositionY} VelocityX = {VelocityX} VelocityY = {VelocityY} MountType = {MountType}]";
        }
    }
}

