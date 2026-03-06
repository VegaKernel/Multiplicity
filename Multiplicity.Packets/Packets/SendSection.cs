using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SendSection (0x0A) packet. Payload is the raw compressed tile block emitted by Terraria.
    /// </summary>
    public class SendSection : TerrariaPacket
    {
        private byte[] _compressedTileBlockData;

        public byte[] CompressedTileBlockData
        {
            get
            {
                return _compressedTileBlockData ??= CompressedTileBlockMemory.ToArray();
            }
            set
            {
                _compressedTileBlockData = value;
                CompressedTileBlockMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> CompressedTileBlockMemory { get; set; }

        public SendSection()
            : base((byte)PacketTypes.SendSection)
        {
            CompressedTileBlockMemory = ReadOnlyMemory<byte>.Empty;
            _compressedTileBlockData = Array.Empty<byte>();
        }

        public SendSection(BinaryReader br)
            : base(br)
        {
            CompressedTileBlockMemory = br.ReadRemainingMemory();
        }

        public override short GetLength()
        {
            return (short)CompressedTileBlockMemory.Length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            if (CompressedTileBlockMemory.Length == 0)
            {
                return;
            }

            stream.Write(CompressedTileBlockMemory.Span);
        }

        public override string ToString()
        {
            double compressedKb = CompressedTileBlockMemory.Length / 1024.0;
            return $"[SendSection: CompressedTileBlockData = {compressedKb:0.###} kB]";
        }
    }
}
