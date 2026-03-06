using System;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class NetBannersModule : TerrariaNetModule
    {
        private const short MessageTypeLength = sizeof(byte);

        public NetBannersMessageType MessageType { get; set; }

        public short BannerId { get; set; }

        public int KillCount { get; set; }

        public ushort ClaimableCount { get; set; }

        public ushort ClaimAmount { get; set; }

        public bool ClaimGranted { get; set; }

        public BannerSystemState FullStateData { get; set; }

        public byte[] FullStatePayload
        {
            get => FullStateData.ToPayloadArray();
            set => FullStateData = BannerSystemState.Parse(value ?? Array.Empty<byte>());
        }

        public ReadOnlyMemory<byte> FullStatePayloadMemory
        {
            get => FullStateData.ToPayloadArray();
            set => FullStateData = BannerSystemState.Parse(value.Span);
        }

        public NetBannersModule()
            : base(NetworkModuleTypes.NetBannersModule)
        {
            FullStateData = new BannerSystemState();
        }

        public NetBannersModule(BinaryReader br)
            : base(br)
        {
            MessageType = (NetBannersMessageType)br.ReadByte();
            FullStateData = new BannerSystemState();
            switch (MessageType)
            {
                case NetBannersMessageType.FullState:
                    FullStateData = new BannerSystemState(br);
                    break;
                case NetBannersMessageType.KillCountUpdate:
                    BannerId = br.ReadInt16();
                    KillCount = br.ReadInt32();
                    break;
                case NetBannersMessageType.ClaimCountUpdate:
                    BannerId = br.ReadInt16();
                    ClaimableCount = br.ReadUInt16();
                    break;
                case NetBannersMessageType.ClaimRequest:
                    BannerId = br.ReadInt16();
                    ClaimAmount = br.ReadUInt16();
                    break;
                case NetBannersMessageType.ClaimResponse:
                    BannerId = br.ReadInt16();
                    ClaimAmount = br.ReadUInt16();
                    ClaimGranted = br.ReadBoolean();
                    break;
            }
        }

        public override short GetLength()
        {
            return MessageType switch
            {
                NetBannersMessageType.FullState => checked((short)(MessageTypeLength + FullStateData.GetPayloadLength())),
                NetBannersMessageType.KillCountUpdate => (short)(MessageTypeLength + sizeof(short) + sizeof(int)),
                NetBannersMessageType.ClaimCountUpdate => (short)(MessageTypeLength + sizeof(short) + sizeof(ushort)),
                NetBannersMessageType.ClaimRequest => (short)(MessageTypeLength + sizeof(short) + sizeof(ushort)),
                NetBannersMessageType.ClaimResponse => (short)(MessageTypeLength + sizeof(short) + sizeof(ushort) + sizeof(bool)),
                _ => MessageTypeLength
            };
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write((byte)MessageType);

            switch (MessageType)
            {
                case NetBannersMessageType.FullState:
                    FullStateData.Write(bw);
                    break;
                case NetBannersMessageType.KillCountUpdate:
                    bw.Write(BannerId);
                    bw.Write(KillCount);
                    break;
                case NetBannersMessageType.ClaimCountUpdate:
                    bw.Write(BannerId);
                    bw.Write(ClaimableCount);
                    break;
                case NetBannersMessageType.ClaimRequest:
                    bw.Write(BannerId);
                    bw.Write(ClaimAmount);
                    break;
                case NetBannersMessageType.ClaimResponse:
                    bw.Write(BannerId);
                    bw.Write(ClaimAmount);
                    bw.Write(ClaimGranted);
                    break;
            }
        }
    }
}
