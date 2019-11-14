namespace NaughtierAttributes
{
    public class GetComponentInChildrenAttribute : NameObjectAttribute
    {
        public GetComponentInChildrenAttribute(bool includeInactive = false, bool playUpdate = false) : base(includeInactive, playUpdate)
        {
        }

        public GetComponentInChildrenAttribute(string name, bool includeInactive = false, bool playUpdate = false) :
            base(name, includeInactive, playUpdate)
        {
        }
    }
}