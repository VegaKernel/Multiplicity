using System;
using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public enum NetTextModulePayloadKind
    {
        Raw = 0,
        ClientChatMessage = 1,
        ServerChatMessage = 2
    }

    /// <summary>
    /// NetModule used for chat/message traffic. In modern Terraria this payload is directional:
    /// client->server sends command+text, server->client sends author+NetworkText+color.
    /// </summary>
    public class NetTextModule : TerrariaNetModule
    {
        public NetTextModulePayloadKind PayloadKind { get; set; }
        public string CommandName { get; set; }
        public string ChatMessage { get; set; }
        public byte AuthorId { get; set; } = byte.MaxValue;
        public NetworkText ServerText { get; set; }
        public ColorStruct MessageColor { get; set; }
        private byte[] _rawPayload;

        public byte[] RawPayload
        {
            get
            {
                return _rawPayload ??= RawPayloadMemory.ToArray();
            }
            set
            {
                _rawPayload = value;
                RawPayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> RawPayloadMemory { get; set; }

        public NetTextModule()
            : base(NetworkModuleTypes.NetTextModule)
        {
            PayloadKind = NetTextModulePayloadKind.Raw;
            CommandName = string.Empty;
            ChatMessage = string.Empty;
            ServerText = new NetworkText();
            RawPayloadMemory = ReadOnlyMemory<byte>.Empty;
            _rawPayload = Array.Empty<byte>();
        }

        public NetTextModule(BinaryReader br)
            : base(br)
        {
            CommandName = string.Empty;
            ChatMessage = string.Empty;
            ServerText = new NetworkText();
            RawPayloadMemory = br.ReadRemainingMemory();

            if (!TryReadServerPayload(RawPayloadMemory, out byte authorId, out NetworkText text, out ColorStruct color) &&
                !TryReadClientPayload(RawPayloadMemory, out string commandName, out string chatMessage))
            {
                PayloadKind = NetTextModulePayloadKind.Raw;
                return;
            }

            if (TryReadServerPayload(RawPayloadMemory, out authorId, out text, out color))
            {
                PayloadKind = NetTextModulePayloadKind.ServerChatMessage;
                AuthorId = authorId;
                ServerText = text;
                MessageColor = color;
                return;
            }

            if (TryReadClientPayload(RawPayloadMemory, out commandName, out chatMessage))
            {
                PayloadKind = NetTextModulePayloadKind.ClientChatMessage;
                CommandName = commandName;
                ChatMessage = chatMessage;
                return;
            }
        }

        public override short GetLength()
        {
            return PayloadKind switch
            {
                NetTextModulePayloadKind.ClientChatMessage => (short)(
                    PacketSerializationHelper.GetSerializedStringLength(CommandName) +
                    PacketSerializationHelper.GetSerializedStringLength(ChatMessage)),
                NetTextModulePayloadKind.ServerChatMessage => (short)(
                    sizeof(byte) +
                    (ServerText?.GetLength() ?? 0) +
                    3),
                _ => (short)RawPayloadMemory.Length
            };
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                switch (PayloadKind)
                {
                    case NetTextModulePayloadKind.ClientChatMessage:
                        bw.Write(CommandName ?? string.Empty);
                        bw.Write(ChatMessage ?? string.Empty);
                        break;
                    case NetTextModulePayloadKind.ServerChatMessage:
                        bw.Write(AuthorId);
                        bw.Write(ServerText ?? new NetworkText());
                        bw.Write(MessageColor);
                        break;
                    default:
                        if (RawPayloadMemory.Length > 0)
                        {
                            stream.Write(RawPayloadMemory.Span);
                        }
                        break;
                }
            }
        }

        private static bool TryReadServerPayload(ReadOnlyMemory<byte> payload, out byte authorId, out NetworkText text, out ColorStruct color)
        {
            authorId = byte.MaxValue;
            text = null;
            color = default;

            if (payload.Length < 5)
            {
                return false;
            }

            // After author id the modern payload stores NetworkText mode (0..2).
            if (payload.Span[1] > (byte)NetworkText.Mode.LocalizationKey)
            {
                return false;
            }

            try
            {
                using PacketBinaryReader br = new PacketBinaryReader(payload);
                authorId = br.ReadByte();
                text = br.ReadNetworkText();
                color = br.ReadColor();
                return br.BaseStream.Position == br.BaseStream.Length;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryReadClientPayload(ReadOnlyMemory<byte> payload, out string commandName, out string chatMessage)
        {
            commandName = string.Empty;
            chatMessage = string.Empty;

            if (payload.Length == 0)
            {
                return false;
            }

            try
            {
                using PacketBinaryReader br = new PacketBinaryReader(payload);
                commandName = br.ReadString();
                chatMessage = br.ReadString();
                return br.BaseStream.Position == br.BaseStream.Length;
            }
            catch
            {
                return false;
            }
        }
    }
}
