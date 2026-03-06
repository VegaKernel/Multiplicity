using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateItemDrop (0x5A) packet.
    /// </summary>
    public class UpdateItemDrop : WorldItemSyncPacketBase
    {
        public UpdateItemDrop()
            : base((byte)PacketTypes.UpdateItemDrop)
        {
        }

        public UpdateItemDrop(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return $"[UpdateItemDrop: ItemIndex = {ItemIndex} PositionX = {PositionX} PositionY = {PositionY} VelocityX = {VelocityX} VelocityY = {VelocityY} Stack = {Stack} Prefix = {Prefix} Flags = {Flags} ItemNetId = {ItemNetId}]";
        }
    }
}

