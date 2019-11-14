namespace NaughtierAttributes
{
    public class FindObjectsWithTagAttribute : TagObjectAttribute
    {
        public FindObjectsWithTagAttribute(string tag, bool includeInactive = false, bool playUpdate = false) : 
            base(tag, includeInactive, playUpdate)
        {
        }
    }
}