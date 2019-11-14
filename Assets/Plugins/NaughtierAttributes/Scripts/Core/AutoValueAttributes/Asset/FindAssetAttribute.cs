namespace NaughtierAttributes
{
    public class FindAssetAttribute : BaseSearchAssetAttribute
    {
        public FindAssetAttribute(bool playUpdate = false) : base(playUpdate)
        {
        }

        public FindAssetAttribute(string name, bool playUpdate = false) : base(name, playUpdate)
        {
        }
    }
}