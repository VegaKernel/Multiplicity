using System;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    public abstract class TerrariaNetModule : TerrariaNetworkObject
    {
        public const short ModuleHeaderLength = sizeof(ushort);

        public static readonly Dictionary<NetworkModuleTypes, Func<BinaryReader, TerrariaNetModule>> DeserializerMap =
            new Dictionary<NetworkModuleTypes, Func<BinaryReader, TerrariaNetModule>>
            {
                { NetworkModuleTypes.NetPingModule, br => new NetPingModule(br) },
                { NetworkModuleTypes.NetAmbienceModule, br => new NetAmbienceModule(br) },
                { NetworkModuleTypes.NetBestiaryModule, br => new NetBestiaryModule(br) },
                { NetworkModuleTypes.NetCreativeUnlocksModule, br => new NetCreativeUnlocksModule(br) },
                { NetworkModuleTypes.NetCreativePowersModule, br => new NetCreativePowersModule(br) },
                { NetworkModuleTypes.NetCreativeUnlocksPlayerReportModule, br => new NetCreativeUnlocksPlayerReportModule(br) },
                { NetworkModuleTypes.NetTeleportPylonModule, br => new NetTeleportPylonModule(br) },
                { NetworkModuleTypes.NetParticlesModule, br => new NetParticlesModule(br) },
                { NetworkModuleTypes.NetCreativePowerPermissionsModule, br => new NetCreativePowerPermissionsModule(br) },
                { NetworkModuleTypes.NetBannersModule, br => new NetBannersModule(br) },
                { NetworkModuleTypes.NetCraftingRequestsModule, br => new NetCraftingRequestsModule(br) },
                { NetworkModuleTypes.LeashedEntityModule, br => new LeashedEntityModule(br) },
                { NetworkModuleTypes.UnbreakableWallScanModule, br => new UnbreakableWallScanModule(br) },
                { NetworkModuleTypes.NetLiquidModule, br => new NetLiquidModule(br) },
                { NetworkModuleTypes.NetTextModule, br => new NetTextModule(br) }
            };

        public NetworkModuleTypes Id { get; protected set; }

        protected TerrariaNetModule(BinaryReader br)
        {
            Id = (NetworkModuleTypes)br.ReadUInt16();
        }

        protected TerrariaNetModule(NetworkModuleTypes id)
        {
            Id = id;
        }

        public static TerrariaNetModule Deserialize(BinaryReader br)
        {
            long startPosition = br.BaseStream.Position;
            ushort rawId = br.ReadUInt16();
            br.BaseStream.Seek(startPosition, SeekOrigin.Begin);

            if (!DeserializerMap.TryGetValue((NetworkModuleTypes)rawId, out Func<BinaryReader, TerrariaNetModule> deserializer))
                return new UnknownNetModule(br);

            return deserializer(br);
        }

        public static TerrariaNetModule Deserialize(ReadOnlyMemory<byte> moduleBuffer)
        {
            using PacketBinaryReader br = new PacketBinaryReader(moduleBuffer);
            return Deserialize(br);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (!includeHeader)
                return;

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                bw.Write((ushort)Id);
        }
    }
}
