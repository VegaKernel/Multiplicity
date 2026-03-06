using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayLegacySoundView
        {
            private const byte HasStyleFlag = 1 << 0;
            private const byte HasVolumeFlag = 1 << 1;
            private const byte HasPitchOffsetFlag = 1 << 2;
    
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public PlayLegacySoundView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.PlayLegacySound);
                PacketReader reader = packet.CreatePayloadReader();
                float positionX = reader.ReadSingle();
                float positionY = reader.ReadSingle();
                ushort soundIndex = reader.ReadUInt16();
                byte flags = reader.ReadByte();
                int style = 0;
                float volume = 0f;
                float pitchOffset = 0f;
    
                if ((flags & HasStyleFlag) != 0)
                {
                    style = reader.ReadInt32();
                }
    
                if ((flags & HasVolumeFlag) != 0)
                {
                    volume = reader.ReadSingle();
                }
    
                if ((flags & HasPitchOffsetFlag) != 0)
                {
                    pitchOffset = reader.ReadSingle();
                }
    
                reader.EnsureEnd();
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PositionX = positionX;
                PositionY = positionY;
                SoundIndex = soundIndex;
                HasStyle = (flags & HasStyleFlag) != 0;
                HasVolume = (flags & HasVolumeFlag) != 0;
                HasPitchOffset = (flags & HasPitchOffsetFlag) != 0;
                Style = style;
                Volume = volume;
                PitchOffset = pitchOffset;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public float PositionX { get; }
            public float PositionY { get; }
            public ushort SoundIndex { get; }
            public bool HasStyle { get; }
            public int Style { get; }
            public bool HasVolume { get; }
            public float Volume { get; }
            public bool HasPitchOffset { get; }
            public float PitchOffset { get; }
        }
}

