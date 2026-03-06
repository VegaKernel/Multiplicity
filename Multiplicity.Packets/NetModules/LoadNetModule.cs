using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The LoadNetModule (0x52) packet.
    /// </summary>
    public class LoadNetModule : TerrariaPacket
    {
        public TerrariaNetModule LoadedModule { get; set; }

        public NetworkModuleTypes? LoadedModuleType => LoadedModule?.Id;

        public LoadNetModule()
            : base((byte)PacketTypes.LoadNetModule)
        {
        }

        public LoadNetModule(BinaryReader br)
            : base(br)
        {
            LoadedModule = TerrariaNetModule.Deserialize(br);
        }

        public override short GetLength()
        {
            if (LoadedModule == null)
            {
                return 0;
            }

            return (short)(TerrariaNetModule.ModuleHeaderLength + LoadedModule.GetLength());
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            LoadedModule?.ToStream(stream, includeHeader: true);
        }

        public override string ToString()
        {
            return $"[LoadNetModule: LoadedModuleType = {LoadedModuleType}]";
        }
    }
}
