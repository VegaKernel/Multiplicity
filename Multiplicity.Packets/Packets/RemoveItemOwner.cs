using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// ReleaseItemOwnership (0x27) for protocol 325.
    /// </summary>
    public class RemoveItemOwner : TerrariaPacket
    {
        public short ItemIndex { get; set; }
        public bool ForceAssignToServer { get; set; }

        public RemoveItemOwner()
            : base((byte)PacketTypes.RemoveItemOwner)
        {
        }

        public RemoveItemOwner(BinaryReader br)
            : base(br)
        {
            ItemIndex = br.ReadInt16();
            ForceAssignToServer = br.ReadBoolean();
        }

        public override string ToString() =>
            $"[RemoveItemOwner: ItemIndex = {ItemIndex} ForceAssignToServer = {ForceAssignToServer}]";

        public override short GetLength() => 3;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(ItemIndex);
                bw.Write(ForceAssignToServer);
            }
        }
    }
}
