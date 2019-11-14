namespace NaughtierAttributes
{
    public class GetComponentInChildrenWithTagAttribute : TagObjectAttribute
    {
        public GetComponentInChildrenWithTagAttribute(string tag, bool includeInactive = false, bool playUpdate = false) :
            base(tag, includeInactive, playUpdate)
        {
        }
    }
}