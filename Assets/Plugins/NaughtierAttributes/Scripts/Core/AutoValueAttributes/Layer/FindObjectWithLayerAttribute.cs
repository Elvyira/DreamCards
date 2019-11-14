namespace NaughtierAttributes
{
    public class FindObjectWithLayerAttribute : LayerObjectAttribute
    {
        public FindObjectWithLayerAttribute(string layer, bool includeInactive = false, bool playUpdate = false) :
            base(layer, includeInactive, playUpdate)
        {
        }
    }
}