namespace NaughtierAttributes
{
    public class FindObjectAttribute : NameObjectAttribute
    {
        public FindObjectAttribute(bool includeInactive = false, bool playUpdate = false) : base(includeInactive, playUpdate)
        {
        }
        
        public FindObjectAttribute(string name, bool includeInactive = false, bool playUpdate = false) : 
            base(name, includeInactive, playUpdate)
        {
        }
    }
}