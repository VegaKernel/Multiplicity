using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Terraria DevCommands packet (94).
    /// Wire layout: string command, int32 value, float value2, float value3.
    /// </summary>
    public sealed class DebugCommands : TerrariaPacket
    {
        private const short NumericPayloadLength = sizeof(int) + sizeof(float) + sizeof(float);

        public string Command { get; set; } = string.Empty;
        public int Value { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }

        public DebugCommands()
            : base((byte)PacketTypes.DebugCommands)
        {
        }

        public DebugCommands(BinaryReader br)
            : base(br)
        {
            Command = br.ReadString();
            Value = br.ReadInt32();
            Value2 = br.ReadSingle();
            Value3 = br.ReadSingle();
            CheckLength();
        }

        public override short GetLength()
        {
            return checked((short)(PacketSerializationHelper.GetSerializedStringLength(Command) + NumericPayloadLength));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(Command ?? string.Empty);
            bw.Write(Value);
            bw.Write(Value2);
            bw.Write(Value3);
        }

        public override string ToString()
        {
            return $"[DebugCommands: Command = {Command}, Value = {Value}, Value2 = {Value2}, Value3 = {Value3}]";
        }
    }
}
