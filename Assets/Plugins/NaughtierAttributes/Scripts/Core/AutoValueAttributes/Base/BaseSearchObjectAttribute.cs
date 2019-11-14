namespace NaughtierAttributes
{
    public abstract class BaseSearchObjectAttribute : BaseAutoValueAttribute
    {
        public readonly bool IncludeInactive;

        protected BaseSearchObjectAttribute(bool includeInactive, bool playUpdate) : base(playUpdate)
        {
            IncludeInactive = includeInactive;
        }
    }
}