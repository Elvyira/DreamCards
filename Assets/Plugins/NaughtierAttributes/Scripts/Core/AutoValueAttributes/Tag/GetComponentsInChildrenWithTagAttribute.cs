namespace NaughtierAttributes
{
    public class GetComponentsInChildrenWithTagAttribute : TagObjectAttribute
    {
        public GetComponentsInChildrenWithTagAttribute(string tag, bool includeInactive = false, bool playUpdate = false) : 
            base(tag, includeInactive, playUpdate)
        {
        }
    }
}