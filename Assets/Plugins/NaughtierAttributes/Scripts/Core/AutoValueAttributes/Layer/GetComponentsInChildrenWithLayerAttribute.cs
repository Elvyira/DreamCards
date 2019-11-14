namespace NaughtierAttributes
{
    public class GetComponentsInChildrenWithLayerAttribute : LayerObjectAttribute
    {
        public GetComponentsInChildrenWithLayerAttribute(string layer, bool includeInactive = false, bool playUpdate = false) :
            base(layer, includeInactive, playUpdate)
        {
        }
    }
}