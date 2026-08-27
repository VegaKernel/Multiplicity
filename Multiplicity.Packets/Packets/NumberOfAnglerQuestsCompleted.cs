using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The QuestsCountSync (76) packet.
    /// </summary>
    public class NumberOfAnglerQuestsCompleted : TerrariaPacket
    {
        private const short PayloadLength = 9;

        public byte PlayerId { get; set; }
        public int AnglerQuestsCompleted { get; set; }
        public int GolferScoreAccumulated { get; set; }

        public NumberOfAnglerQuestsCompleted()
            : base((byte)PacketTypes.NumberOfAnglerQuestsCompleted)
        {
        }

        public NumberOfAnglerQuestsCompleted(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            AnglerQuestsCompleted = br.ReadInt32();
            GolferScoreAccumulated = br.ReadInt32();
        }

        public override short GetLength() => PayloadLength;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(PlayerId);
            bw.Write(AnglerQuestsCompleted);
            bw.Write(GolferScoreAccumulated);
        }

        public override string ToString()
        {
            return $"[NumberOfAnglerQuestsCompleted: PlayerId = {PlayerId} AnglerQuestsCompleted = {AnglerQuestsCompleted} GolferScoreAccumulated = {GolferScoreAccumulated}]";
        }
    }
}
