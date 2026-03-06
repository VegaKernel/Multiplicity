using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerInfoView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerInfoView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerInfo, payload))
        {
        }

        public static PlayerInfoView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerInfoView(payload);
        }

        public PlayerInfoView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerInfo);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte skinVariant = reader.ReadByte();
            byte voiceVariant = reader.ReadByte();
            float voicePitchOffset = reader.ReadSingle();
            byte hair = reader.ReadByte();
            ReadOnlySpan<byte> nameUtf8 = reader.ReadLengthPrefixedBytes();
            byte hairDye = reader.ReadByte();
            ushort hideVisibleAccessory = reader.ReadUInt16();
            byte hideMisc = reader.ReadByte();
            ColorStruct hairColor = reader.ReadColor();
            ColorStruct skinColor = reader.ReadColor();
            ColorStruct eyeColor = reader.ReadColor();
            ColorStruct shirtColor = reader.ReadColor();
            ColorStruct underShirtColor = reader.ReadColor();
            ColorStruct pantsColor = reader.ReadColor();
            ColorStruct shoeColor = reader.ReadColor();
            PlayerDifficultyFlags difficultyFlags = (PlayerDifficultyFlags)reader.ReadByte();
            PlayerTorchAndCartFlags torchAndCartFlags = (PlayerTorchAndCartFlags)reader.ReadByte();
            PlayerConsumableUnlockFlags consumableUnlockFlags = (PlayerConsumableUnlockFlags)reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            SkinVariant = skinVariant;
            VoiceVariant = voiceVariant;
            VoicePitchOffset = voicePitchOffset;
            Hair = hair;
            NameUtf8 = nameUtf8;
            HairDye = hairDye;
            HideVisibleAccessory = hideVisibleAccessory;
            HideMisc = hideMisc;
            HairColor = hairColor;
            SkinColor = skinColor;
            EyeColor = eyeColor;
            ShirtColor = shirtColor;
            UnderShirtColor = underShirtColor;
            PantsColor = pantsColor;
            ShoeColor = shoeColor;
            DifficultyFlags = difficultyFlags;
            TorchAndCartFlags = torchAndCartFlags;
            ConsumableUnlockFlags = consumableUnlockFlags;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public byte SkinVariant { get; }

        public byte VoiceVariant { get; }

        public float VoicePitchOffset { get; }

        public byte Hair { get; }

        public ReadOnlySpan<byte> NameUtf8 { get; }

        public byte HairDye { get; }

        public ushort HideVisibleAccessory { get; }

        public byte HideMisc { get; }

        public ColorStruct HairColor { get; }

        public ColorStruct SkinColor { get; }

        public ColorStruct EyeColor { get; }

        public ColorStruct ShirtColor { get; }

        public ColorStruct UnderShirtColor { get; }

        public ColorStruct PantsColor { get; }

        public ColorStruct ShoeColor { get; }

        public PlayerDifficultyFlags DifficultyFlags { get; }

        public PlayerTorchAndCartFlags TorchAndCartFlags { get; }

        public PlayerConsumableUnlockFlags ConsumableUnlockFlags { get; }

        public string GetName()
        {
            return Encoding.UTF8.GetString(NameUtf8);
        }
    }
}



