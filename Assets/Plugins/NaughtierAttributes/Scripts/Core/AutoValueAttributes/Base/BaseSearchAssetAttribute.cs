namespace NaughtierAttributes
{
    public abstract class BaseSearchAssetAttribute : BaseAutoValueAttribute
    {
        public readonly string Name;

        protected BaseSearchAssetAttribute(bool playUpdate) : base(playUpdate)
        {
        }

        protected BaseSearchAssetAttribute(string name, bool playUpdate) : base(playUpdate)
        {
            Name = name;
        }
    }
}