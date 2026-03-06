using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateTileEntity (0x56) packet. Modern Terraria sends tile entity payload through TileEntity.Write/Read.
    /// This model keeps the entity key, existence flag, and raw serialized tile-entity payload.
    /// </summary>
    public class UpdateTileEntity : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public int Key { get; set; }
        public bool Exists { get; set; }
        private byte[] _tileEntityPayload;

        public byte[] TileEntityPayload
        {
            get
            {
                return _tileEntityPayload ??= TileEntityPayloadMemory.ToArray();
            }
            set
            {
                _tileEntityPayload = value;
                TileEntityPayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> TileEntityPayloadMemory { get; set; }

        public UpdateTileEntity()
            : base((byte)PacketTypes.UpdateTileEntity)
        {
            TileEntityPayloadMemory = ReadOnlyMemory<byte>.Empty;
            _tileEntityPayload = Array.Empty<byte>();
        }

        public UpdateTileEntity(BinaryReader br)
            : base(br)
        {
            Key = br.ReadInt32();
            Exists = br.ReadBoolean();
            TileEntityPayloadMemory = Exists
                ? br.ReadRemainingMemory()
                : ReadOnlyMemory<byte>.Empty;
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + TileEntityPayloadMemory.Length);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Key);
                bw.Write(Exists);
                if (Exists && TileEntityPayloadMemory.Length > 0)
                {
                    stream.Write(TileEntityPayloadMemory.Span);
                }
            }
        }

        public override string ToString()
        {
            int payloadLength = TileEntityPayloadMemory.Length;
            return $"[UpdateTileEntity: Key = {Key} Exists = {Exists} TileEntityPayload = {payloadLength} bytes]";
        }
    }
}
