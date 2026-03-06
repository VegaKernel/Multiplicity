using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class PlayLegacySound : TerrariaPacket
        {
            public NetSoundInfoData SoundInfo { get; set; } = new NetSoundInfoData();
    
            public PlayLegacySound()
                : base((byte)PacketTypes.PlayLegacySound)
            {
            }
    
            public PlayLegacySound(BinaryReader br)
                : base(br)
            {
                SoundInfo = new NetSoundInfoData(br);
            }
    
            public override short GetLength()
            {
                return (SoundInfo ?? new NetSoundInfoData()).GetLength();
            }
    
            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }
    
                using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                {
                    (SoundInfo ?? new NetSoundInfoData()).ToStream(bw);
                }
            }
        }
}

