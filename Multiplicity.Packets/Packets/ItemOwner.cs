using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ItemOwner (0x16) packet for protocol 325.
    /// </summary>
    public class ItemOwner : TerrariaPacket
    {
        public short ItemId { get; set; }
        public byte PlayerId { get; set; }
        public int TimeToKeepReservation { get; set; }
        public byte GrabDelayPlayer { get; set; }
        public int GrabDelayTime { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public ItemOwner()
            : base((byte)PacketTypes.ItemOwner)
        {
        }

        public ItemOwner(BinaryReader br)
            : base(br)
        {
            ItemId = br.ReadInt16();
            PlayerId = br.ReadByte();
            TimeToKeepReservation = br.Read7BitEncodedInt();
            GrabDelayPlayer = br.ReadByte();
            GrabDelayTime = br.Read7BitEncodedInt();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
        }

        public override string ToString()
        {
            return $"[ItemOwner: ItemId = {ItemId} PlayerId = {PlayerId} TimeToKeepReservation = {TimeToKeepReservation} GrabDelayPlayer = {GrabDelayPlayer} GrabDelayTime = {GrabDelayTime} PositionX = {PositionX} PositionY = {PositionY}]";
        }

        public override short GetLength()
        {
            return checked((short)(
                sizeof(short) + sizeof(byte) +
                PacketSerializationHelper.Get7BitEncodedIntLength(TimeToKeepReservation) +
                sizeof(byte) +
                PacketSerializationHelper.Get7BitEncodedIntLength(GrabDelayTime) +
                sizeof(float) * 2));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(ItemId);
                bw.Write(PlayerId);
                bw.Write7BitEncodedInt(TimeToKeepReservation);
                bw.Write(GrabDelayPlayer);
                bw.Write7BitEncodedInt(GrabDelayTime);
                bw.Write(PositionX);
                bw.Write(PositionY);
            }
        }
    }
}
