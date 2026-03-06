namespace Multiplicity.Packets
{
    public sealed class CraftingRequestItem
    {
        public int ItemIdOrRecipeGroup { get; set; }

        public int Stack { get; set; }
    }
}