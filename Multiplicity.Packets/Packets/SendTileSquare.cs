using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SendTileSquare (0x14) packet.
    /// </summary>
    public class SendTileSquare : TerrariaPacket
    {
        private const short FixedPayloadLength = 7;

        public short TileX { get; set; }
        public short TileY { get; set; }
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte TileChangeType { get; set; }
        private byte[] _tilePayload;

        public byte[] TilePayload
        {
            get
            {
                return _tilePayload ??= TilePayloadMemory.ToArray();
            }
            set
            {
                _tilePayload = value;
                TilePayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> TilePayloadMemory { get; set; }

        public SendTileSquare()
            : base((byte)PacketTypes.SendTileSquare)
        {
            TilePayloadMemory = ReadOnlyMemory<byte>.Empty;
            _tilePayload = Array.Empty<byte>();
        }

        public SendTileSquare(BinaryReader br)
            : base(br)
        {
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            Width = br.ReadByte();
            Height = br.ReadByte();
            TileChangeType = br.ReadByte();
            TilePayloadMemory = br.ReadRemainingMemory();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + TilePayloadMemory.Length);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(TileX);
                bw.Write(TileY);
                bw.Write(Width);
                bw.Write(Height);
                bw.Write(TileChangeType);
                if (TilePayloadMemory.Length > 0)
                {
                    stream.Write(TilePayloadMemory.Span);
                }
            }
        }

        public override string ToString()
        {
            double payloadKb = TilePayloadMemory.Length / 1024.0;
            return $"[SendTileSquare: TileChangeType = {TileChangeType} Width = {Width} Height = {Height} TileX = {TileX} TileY = {TileY} TileData = {payloadKb:0.###} kB]";
        }
    }
}
