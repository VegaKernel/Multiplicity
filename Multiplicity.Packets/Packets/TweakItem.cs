using System.IO;

namespace Multiplicity.Packets
{
    public class TweakItem : TerrariaPacket
    {
        private const short FixedPayloadLength = 3;
        private const short SecondaryFlagsPayloadLength = 1;
        private const short PackedColorPayloadLength = 4;
        private const short UShortPayloadLength = 2;
        private const short FloatPayloadLength = 4;
        private const short BooleanPayloadLength = 1;

        public short ItemIndex { get; set; }
        public TweakItemFlags1 Flags1 { get; set; }
        public uint PackedColorValue { get; set; }
        public ushort Damage { get; set; }
        public float Knockback { get; set; }
        public ushort UseAnimation { get; set; }
        public ushort UseTime { get; set; }
        public short Shoot { get; set; }
        public float ShootSpeed { get; set; }
        public TweakItemFlags2 Flags2 { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public float Scale { get; set; }
        public short Ammo { get; set; }
        public short UseAmmo { get; set; }
        public bool NotAmmo { get; set; }

        public TweakItem()
            : base((byte)PacketTypes.TweakItem)
        {
        }

        public TweakItem(BinaryReader br)
            : base(br)
        {
            ItemIndex = br.ReadInt16();
            Flags1 = (TweakItemFlags1)br.ReadByte();
            if (HasFlags1(TweakItemFlags1.PackedColorValue))
                PackedColorValue = br.ReadUInt32();
            if (HasFlags1(TweakItemFlags1.Damage))
                Damage = br.ReadUInt16();
            if (HasFlags1(TweakItemFlags1.Knockback))
                Knockback = br.ReadSingle();
            if (HasFlags1(TweakItemFlags1.UseAnimation))
                UseAnimation = br.ReadUInt16();
            if (HasFlags1(TweakItemFlags1.UseTime))
                UseTime = br.ReadUInt16();
            if (HasFlags1(TweakItemFlags1.Shoot))
                Shoot = br.ReadInt16();
            if (HasFlags1(TweakItemFlags1.ShootSpeed))
                ShootSpeed = br.ReadSingle();

            if (HasFlags1(TweakItemFlags1.SecondaryFlags))
            {
                Flags2 = (TweakItemFlags2)br.ReadByte();
                if (HasFlags2(TweakItemFlags2.Width))
                    Width = br.ReadInt16();
                if (HasFlags2(TweakItemFlags2.Height))
                    Height = br.ReadInt16();
                if (HasFlags2(TweakItemFlags2.Scale))
                    Scale = br.ReadSingle();
                if (HasFlags2(TweakItemFlags2.Ammo))
                    Ammo = br.ReadInt16();
                if (HasFlags2(TweakItemFlags2.UseAmmo))
                    UseAmmo = br.ReadInt16();
                if (HasFlags2(TweakItemFlags2.NotAmmo))
                    NotAmmo = br.ReadBoolean();
            }
        }

        public override short GetLength()
        {
            short length = FixedPayloadLength;

            if (HasFlags1(TweakItemFlags1.PackedColorValue))
                length += PackedColorPayloadLength;
            if (HasFlags1(TweakItemFlags1.Damage))
                length += UShortPayloadLength;
            if (HasFlags1(TweakItemFlags1.Knockback))
                length += FloatPayloadLength;
            if (HasFlags1(TweakItemFlags1.UseAnimation))
                length += UShortPayloadLength;
            if (HasFlags1(TweakItemFlags1.UseTime))
                length += UShortPayloadLength;
            if (HasFlags1(TweakItemFlags1.Shoot))
                length += UShortPayloadLength;
            if (HasFlags1(TweakItemFlags1.ShootSpeed))
                length += FloatPayloadLength;

            if (HasFlags1(TweakItemFlags1.SecondaryFlags))
            {
                length += SecondaryFlagsPayloadLength;
                if (HasFlags2(TweakItemFlags2.Width))
                    length += UShortPayloadLength;
                if (HasFlags2(TweakItemFlags2.Height))
                    length += UShortPayloadLength;
                if (HasFlags2(TweakItemFlags2.Scale))
                    length += FloatPayloadLength;
                if (HasFlags2(TweakItemFlags2.Ammo))
                    length += UShortPayloadLength;
                if (HasFlags2(TweakItemFlags2.UseAmmo))
                    length += UShortPayloadLength;
                if (HasFlags2(TweakItemFlags2.NotAmmo))
                    length += BooleanPayloadLength;
            }

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(ItemIndex);
                bw.Write((byte)Flags1);
                if (HasFlags1(TweakItemFlags1.PackedColorValue))
                    bw.Write(PackedColorValue);
                if (HasFlags1(TweakItemFlags1.Damage))
                    bw.Write(Damage);
                if (HasFlags1(TweakItemFlags1.Knockback))
                    bw.Write(Knockback);
                if (HasFlags1(TweakItemFlags1.UseAnimation))
                    bw.Write(UseAnimation);
                if (HasFlags1(TweakItemFlags1.UseTime))
                    bw.Write(UseTime);
                if (HasFlags1(TweakItemFlags1.Shoot))
                    bw.Write(Shoot);
                if (HasFlags1(TweakItemFlags1.ShootSpeed))
                    bw.Write(ShootSpeed);

                if (HasFlags1(TweakItemFlags1.SecondaryFlags))
                {
                    bw.Write((byte)Flags2);
                    if (HasFlags2(TweakItemFlags2.Width))
                        bw.Write(Width);
                    if (HasFlags2(TweakItemFlags2.Height))
                        bw.Write(Height);
                    if (HasFlags2(TweakItemFlags2.Scale))
                        bw.Write(Scale);
                    if (HasFlags2(TweakItemFlags2.Ammo))
                        bw.Write(Ammo);
                    if (HasFlags2(TweakItemFlags2.UseAmmo))
                        bw.Write(UseAmmo);
                    if (HasFlags2(TweakItemFlags2.NotAmmo))
                        bw.Write(NotAmmo);
                }
            }
        }

        public override string ToString()
        {
            return $"[TweakItem ItemIndex: {ItemIndex}, Width: {Width}, Height: {Height}]";
        }

        private bool HasFlags1(TweakItemFlags1 flag)
        {
            return (Flags1 & flag) != 0;
        }

        private bool HasFlags2(TweakItemFlags2 flag)
        {
            return (Flags2 & flag) != 0;
        }
    }
}
