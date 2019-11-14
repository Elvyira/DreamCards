namespace NaughtierAttributes
{
    public class GetComponentsInChildrenAttribute : NameObjectAttribute
    {
        public GetComponentsInChildrenAttribute(bool includeInactive = false, bool playUpdate = false) : base(includeInactive, playUpdate)
        {
        }

        public GetComponentsInChildrenAttribute(string name, bool includeInactive = false, bool playUpdate = false) :
            base(name, includeInactive, playUpdate)
        {
        }
    }
}