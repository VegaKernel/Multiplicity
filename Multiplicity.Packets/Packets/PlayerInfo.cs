using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerInfo (0x04) packet for protocol 317/318.
    /// </summary>
    public class PlayerInfo : TerrariaPacket
    {
        private const short FixedPayloadLength = 36;

        public byte PlayerID { get; set; }
        public byte SkinVariant { get; set; }
        public byte VoiceVariant { get; set; }
        public float VoicePitchOffset { get; set; }
        public byte Hair { get; set; }
        public string Name { get; set; }
        public byte HairDye { get; set; }
        public ushort HideVisibleAccessory { get; set; }
        public byte HideMisc { get; set; }
        public ColorStruct HairColor { get; set; }
        public ColorStruct SkinColor { get; set; }
        public ColorStruct EyeColor { get; set; }
        public ColorStruct ShirtColor { get; set; }
        public ColorStruct UnderShirtColor { get; set; }
        public ColorStruct PantsColor { get; set; }
        public ColorStruct ShoeColor { get; set; }
        public PlayerDifficultyFlags DifficultyFlags { get; set; }
        public PlayerTorchAndCartFlags TorchAndCartFlags { get; set; }
        public PlayerConsumableUnlockFlags ConsumableUnlockFlags { get; set; }

        public PlayerInfo()
            : base((byte)PacketTypes.PlayerInfo)
        {
            Name = string.Empty;
        }

        public PlayerInfo(BinaryReader br)
            : base(br)
        {
            PlayerID = br.ReadByte();
            SkinVariant = br.ReadByte();
            VoiceVariant = br.ReadByte();
            VoicePitchOffset = br.ReadSingle();
            Hair = br.ReadByte();
            Name = br.ReadString();
            HairDye = br.ReadByte();
            HideVisibleAccessory = br.ReadUInt16();
            HideMisc = br.ReadByte();
            HairColor = br.ReadColor();
            SkinColor = br.ReadColor();
            EyeColor = br.ReadColor();
            ShirtColor = br.ReadColor();
            UnderShirtColor = br.ReadColor();
            PantsColor = br.ReadColor();
            ShoeColor = br.ReadColor();
            DifficultyFlags = (PlayerDifficultyFlags)br.ReadByte();
            TorchAndCartFlags = (PlayerTorchAndCartFlags)br.ReadByte();
            ConsumableUnlockFlags = (PlayerConsumableUnlockFlags)br.ReadByte();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(Name));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(PlayerID);
                bw.Write(SkinVariant);
                bw.Write(VoiceVariant);
                bw.Write(VoicePitchOffset);
                bw.Write(Hair);
                bw.Write(Name ?? string.Empty);
                bw.Write(HairDye);
                bw.Write(HideVisibleAccessory);
                bw.Write(HideMisc);
                bw.Write(HairColor);
                bw.Write(SkinColor);
                bw.Write(EyeColor);
                bw.Write(ShirtColor);
                bw.Write(UnderShirtColor);
                bw.Write(PantsColor);
                bw.Write(ShoeColor);
                bw.Write((byte)DifficultyFlags);
                bw.Write((byte)TorchAndCartFlags);
                bw.Write((byte)ConsumableUnlockFlags);
            }
        }
    }
}
