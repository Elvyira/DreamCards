namespace NaughtierAttributes
{
    public class GetComponentsAttribute : BaseSearchObjectAttribute
    {
        public GetComponentsAttribute(bool playUpdate = false) : base(false, playUpdate)
        {
        }
    }
}