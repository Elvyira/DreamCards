namespace NaughtierAttributes
{
    public class FindObjectWithTagAttribute : TagObjectAttribute
    {
        public FindObjectWithTagAttribute(string tag, bool includeInactive = false, bool playUpdate = false) : 
            base(tag, includeInactive, playUpdate)
        {
        }
    }
}