namespace NaughtierAttributes
{
    public class FindObjectsWithLayerAttribute : LayerObjectAttribute
    {
        public FindObjectsWithLayerAttribute(string layer, bool includeInactive = false, bool playUpdate = false) :
            base(layer, includeInactive, playUpdate)
        {
        }
    }
}