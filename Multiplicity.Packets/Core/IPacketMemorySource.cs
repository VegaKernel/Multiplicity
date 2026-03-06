using System;

namespace Multiplicity.Packets
{
    internal interface IPacketMemorySource
    {
        long Length { get; }

        long Position { get; }

        ReadOnlyMemory<byte> ReadMemory(int count);
    }
}
