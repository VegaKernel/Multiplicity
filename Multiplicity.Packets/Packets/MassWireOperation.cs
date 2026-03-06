using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The MassWireOperation (0x6D) packet.
    /// </summary>
    public class MassWireOperation : TerrariaPacket
    {
        private const short PayloadLength = 9;

        public short StartX { get; set; }
        public short StartY { get; set; }
        public short EndX { get; set; }
        public short EndY { get; set; }
        public MassWireOperationToolMode ToolMode { get; set; }

        public MassWireOperation()
            : base((byte)PacketTypes.MassWireOperation)
        {
        }

        public MassWireOperation(BinaryReader br)
            : base(br)
        {
            StartX = br.ReadInt16();
            StartY = br.ReadInt16();
            EndX = br.ReadInt16();
            EndY = br.ReadInt16();
            ToolMode = (MassWireOperationToolMode)br.ReadByte();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(StartX);
                bw.Write(StartY);
                bw.Write(EndX);
                bw.Write(EndY);
                bw.Write((byte)ToolMode);
            }
        }

        public override string ToString()
        {
            return $"[MassWireOperation: StartX = {StartX}, StartY = {StartY}, EndX = {EndX}, EndY = {EndY}, ToolMode = {ToolMode}]";
        }
    }
}
