namespace NaughtierAttributes
{
    public class FindObjectsAttribute : NameObjectAttribute
    {
        public FindObjectsAttribute(bool includeInactive = false, bool playUpdate = false) : base(includeInactive, playUpdate)
        {
        }

        public FindObjectsAttribute(string name, bool includeInactive = false, bool playUpdate = false) :
            base(name, includeInactive, playUpdate)
        {
        }
    }
}