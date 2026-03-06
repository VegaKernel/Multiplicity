using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct LoadNetModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public LoadNetModuleView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.LoadNetModule, payload))
        {
        }

        public static LoadNetModuleView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new LoadNetModuleView(payload);
        }

        public LoadNetModuleView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.LoadNetModule);

            PacketReader reader = packet.CreatePayloadReader();
            NetworkModuleTypes moduleType = (NetworkModuleTypes)reader.ReadUInt16();
            ReadOnlySpan<byte> modulePayloadSpan = reader.ReadRemainingSpan();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            ModuleType = moduleType;
            ModulePayloadSpan = modulePayloadSpan;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public NetworkModuleTypes ModuleType { get; }

        public ReadOnlySpan<byte> ModulePayloadSpan { get; }

        public PacketReader CreateModulePayloadReader()
        {
            return new PacketReader(ModulePayloadSpan);
        }
    }
}



