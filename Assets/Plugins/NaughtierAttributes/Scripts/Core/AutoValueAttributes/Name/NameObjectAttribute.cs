namespace NaughtierAttributes
{
    public class NameObjectAttribute : BaseSearchObjectAttribute
    {
        public readonly string Name;
        
        public NameObjectAttribute(bool includeInactive, bool playUpdate) : base(includeInactive, playUpdate)
        {
        }
        
        public NameObjectAttribute(string name, bool includeInactive, bool playUpdate) : base(includeInactive, playUpdate)
        {
            Name = name;
        }
    }
}