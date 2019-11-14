namespace NaughtierAttributes
{
    public class LayerObjectAttribute : BaseSearchObjectAttribute
    {
        public readonly string Layer;
        
        public LayerObjectAttribute(string layer, bool includeInactive, bool playUpdate) : base(includeInactive, playUpdate)
        {
            Layer = layer;
        }
    }
}