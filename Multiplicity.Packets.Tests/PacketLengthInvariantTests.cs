using System;
using System.Collections.Generic;
using Multiplicity.Packets;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class PacketLengthInvariantTests
    {
        [Fact]
        public void DefaultSerializablePackets_ReportTheirActualPayloadLength()
        {
            List<string> mismatches = new List<string>();

            foreach (PacketTypes packetType in Enum.GetValues<PacketTypes>())
            {
                if (!TerrariaPacket.TryGetPayloadMetadata(packetType, out PacketPayloadMetadata metadata))
                {
                    continue;
                }

                if (metadata.PacketClrType == typeof(NpcItemStrike))
                {
                    // Packet 24 is deliberately unsupported by current Terraria and throws on serialization.
                    continue;
                }

                TerrariaPacket packet;
                try
                {
                    packet = (TerrariaPacket)Activator.CreateInstance(metadata.PacketClrType, nonPublic: true)!;
                }
                catch (Exception ex)
                {
                    mismatches.Add($"{packetType}: could not construct {metadata.PacketClrType.Name}: {ex.GetType().Name}");
                    continue;
                }

                byte[] payload;
                try
                {
                    payload = packet.ToPayloadArray();
                }
                catch (Exception ex)
                {
                    mismatches.Add($"{packetType}: could not serialize {metadata.PacketClrType.Name}: {ex.GetType().Name}");
                    continue;
                }

                if (packet.GetLength() != payload.Length)
                {
                    mismatches.Add($"{packetType}: GetLength()={packet.GetLength()}, serialized={payload.Length}");
                }

                byte[] framed = packet.ToArray(includeHeader: true);
                int declaredLength = BitConverter.ToInt16(framed, 0);
                if (declaredLength != framed.Length)
                {
                    mismatches.Add($"{packetType}: header={declaredLength}, serialized={framed.Length}");
                }
            }

            Assert.True(mismatches.Count == 0, string.Join(Environment.NewLine, mismatches));
        }
    }
}
