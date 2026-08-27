using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The MoonlordHorror / MoonLordCountdown (103) packet.
    /// </summary>
    public class MoonLordCountdown : TerrariaPacket
    {
        private const short PayloadLength = 8;

        public int MaxCountdownValue { get; set; }
        public int CountdownValue { get; set; }

        public MoonLordCountdown()
            : base((byte)PacketTypes.MoonLordCountdown)
        {
        }

        public MoonLordCountdown(BinaryReader br)
            : base(br)
        {
            MaxCountdownValue = br.ReadInt32();
            CountdownValue = br.ReadInt32();
        }

        public override short GetLength() => PayloadLength;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(MaxCountdownValue);
            bw.Write(CountdownValue);
        }

        public override string ToString()
        {
            return $"[MoonLordCountdown: MaxCountdownValue = {MaxCountdownValue} CountdownValue = {CountdownValue}]";
        }
    }
}
