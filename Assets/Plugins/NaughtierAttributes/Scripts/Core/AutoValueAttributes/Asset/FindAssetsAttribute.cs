namespace NaughtierAttributes
{
    public class FindAssetsAttribute : BaseSearchAssetAttribute
    {
        public FindAssetsAttribute(bool playUpdate = false) : base(playUpdate)
        {
        }
        
        public FindAssetsAttribute(string name, bool playUpdate = false) : base(name, playUpdate)
        {
        }
    }
}