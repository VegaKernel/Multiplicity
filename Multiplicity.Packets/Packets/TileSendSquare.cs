using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The TileSendSquare (20) packet. The tile rectangle body is retained as raw vanilla wire
    /// bytes so parsing does not depend on Terraria/XNA assemblies.
    /// </summary>
    public class TileSendSquare : TerrariaPacket
    {
        private const short FixedPayloadLength = 7;

        public short TileX { get; set; }
        public short TileY { get; set; }
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte TileChangeType { get; set; }

        private byte[] _tilePayload;
        private ReadOnlyMemory<byte> _tilePayloadMemory;

        public byte[] TilePayload
        {
            get => _tilePayload ??= _tilePayloadMemory.ToArray();
            set
            {
                _tilePayload = value;
                _tilePayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> TilePayloadMemory
        {
            get => _tilePayloadMemory;
            set
            {
                _tilePayloadMemory = value;
                _tilePayload = null;
            }
        }

        public TileSendSquare()
            : base((byte)PacketTypes.TileSendSquare)
        {
            _tilePayloadMemory = ReadOnlyMemory<byte>.Empty;
            _tilePayload = Array.Empty<byte>();
        }

        public TileSendSquare(BinaryReader br)
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
            return checked((short)(FixedPayloadLength + _tilePayloadMemory.Length));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(TileX);
            bw.Write(TileY);
            bw.Write(Width);
            bw.Write(Height);
            bw.Write(TileChangeType);
            if (_tilePayloadMemory.Length > 0)
            {
                stream.Write(_tilePayloadMemory.Span);
            }
        }

        public override string ToString()
        {
            double payloadKb = _tilePayloadMemory.Length / 1024.0;
            return $"[TileSendSquare: TileChangeType = {TileChangeType} Width = {Width} Height = {Height} TileX = {TileX} TileY = {TileY} TileData = {payloadKb:0.###} kB]";
        }
    }
}
