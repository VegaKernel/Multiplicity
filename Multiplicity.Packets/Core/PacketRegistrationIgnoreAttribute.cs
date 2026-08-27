using System;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Excludes a legacy or extension packet model from the generated vanilla desktop packet registry.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class PacketRegistrationIgnoreAttribute : Attribute
    {
    }
}
