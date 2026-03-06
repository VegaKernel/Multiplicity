using System;
using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetBannersModule : TerrariaNetModule
    {
        private const short MessageTypeLength = sizeof(byte);
        private byte[] _fullStatePayload;

        public NetBannersMessageType MessageType { get; set; }

        public short BannerId { get; set; }

        public int KillCount { get; set; }

        public ushort ClaimableCount { get; set; }

        public ushort ClaimAmount { get; set; }

        public bool ClaimGranted { get; set; }

        public byte[] FullStatePayload
        {
            get
            {
                return _fullStatePayload ??= FullStatePayloadMemory.ToArray();
            }
            set
            {
                _fullStatePayload = value;
                FullStatePayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> FullStatePayloadMemory { get; set; }

        public NetBannersModule()
            : base(NetworkModuleTypes.NetBannersModule)
        {
            FullStatePayloadMemory = ReadOnlyMemory<byte>.Empty;
            _fullStatePayload = Array.Empty<byte>();
        }

        public NetBannersModule(BinaryReader br)
            : base(br)
        {
            MessageType = (NetBannersMessageType)br.ReadByte();
            switch (MessageType)
            {
                case NetBannersMessageType.FullState:
                    FullStatePayloadMemory = br.ReadRemainingMemory();
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
                NetBannersMessageType.FullState => (short)(MessageTypeLength + FullStatePayloadMemory.Length),
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
                    if (FullStatePayloadMemory.Length > 0)
                    {
                        stream.Write(FullStatePayloadMemory.Span);
                    }
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