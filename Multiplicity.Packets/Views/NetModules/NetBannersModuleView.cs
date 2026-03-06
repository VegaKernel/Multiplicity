using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetBannersModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetBannersModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            MessageType = (NetBannersMessageType)reader.ReadByte();
            BannerId = 0;
            KillCount = 0;
            ClaimableCount = 0;
            ClaimAmount = 0;
            ClaimGranted = false;
            FullStatePayloadSpan = ReadOnlySpan<byte>.Empty;

            switch (MessageType)
            {
                case NetBannersMessageType.FullState:
                    FullStatePayloadSpan = reader.ReadRemainingSpan();
                    break;
                case NetBannersMessageType.KillCountUpdate:
                    BannerId = reader.ReadInt16();
                    KillCount = reader.ReadInt32();
                    reader.EnsureEnd();
                    break;
                case NetBannersMessageType.ClaimCountUpdate:
                    BannerId = reader.ReadInt16();
                    ClaimableCount = reader.ReadUInt16();
                    reader.EnsureEnd();
                    break;
                case NetBannersMessageType.ClaimRequest:
                    BannerId = reader.ReadInt16();
                    ClaimAmount = reader.ReadUInt16();
                    reader.EnsureEnd();
                    break;
                case NetBannersMessageType.ClaimResponse:
                    BannerId = reader.ReadInt16();
                    ClaimAmount = reader.ReadUInt16();
                    ClaimGranted = reader.ReadBoolean();
                    reader.EnsureEnd();
                    break;
                default:
                    reader.EnsureEnd();
                    break;
            }

            _sourceSpan = payload;
        }

        public NetBannersModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetBannersModule);
        }

        public static NetBannersModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetBannersModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public NetBannersMessageType MessageType { get; }
        public short BannerId { get; }
        public int KillCount { get; }
        public ushort ClaimableCount { get; }
        public ushort ClaimAmount { get; }
        public bool ClaimGranted { get; }
        public ReadOnlySpan<byte> FullStatePayloadSpan { get; }
        public bool HasFullState => MessageType == NetBannersMessageType.FullState;

        public BannerSystemState GetFullState() => HasFullState
            ? BannerSystemState.Parse(FullStatePayloadSpan)
            : throw new InvalidOperationException("NetBannersModuleView does not contain a FullState payload.");
    }
}
