namespace NaughtierAttributes
{
    public class GetComponentAttribute : BaseSearchObjectAttribute
    {
        public GetComponentAttribute(bool playUpdate = false) : base(false, playUpdate)
        {
        }
    }
}