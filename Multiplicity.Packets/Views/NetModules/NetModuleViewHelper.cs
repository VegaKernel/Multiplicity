using System.IO;

namespace Multiplicity.Packets.Views
{
    internal static class NetModuleViewHelper
    {
        public static void EnsureType(LoadNetModuleView module, NetworkModuleTypes expectedType)
        {
            if (module.ModuleType != expectedType)
            {
                throw new InvalidDataException($"Expected net module {expectedType}, got {module.ModuleType}.");
            }
        }
    }
}