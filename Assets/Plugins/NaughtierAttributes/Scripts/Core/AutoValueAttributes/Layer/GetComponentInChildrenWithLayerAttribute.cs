namespace NaughtierAttributes
{
    public class GetComponentInChildrenWithLayerAttribute : LayerObjectAttribute
    {
        public GetComponentInChildrenWithLayerAttribute(string layer, bool includeInactive = false, bool playUpdate = false) :
            base(layer, includeInactive, playUpdate)
        {
        }
    }
}