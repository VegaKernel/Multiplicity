using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateItemDrop2 (0x5A) packet.
    /// </summary>
    public class UpdateItemDrop2 : WorldItemSyncPacketBase
    {
        public UpdateItemDrop2()
            : base((byte)PacketTypes.UpdateItemDrop2)
        {
        }

        public UpdateItemDrop2(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return $"[UpdateItemDrop2: ItemIndex = {ItemIndex} PositionX = {PositionX} PositionY = {PositionY} VelocityX = {VelocityX} VelocityY = {VelocityY} Stack = {Stack} Prefix = {Prefix} Flags = {Flags} ItemNetID = {ItemNetID}]";
        }
    }
}
