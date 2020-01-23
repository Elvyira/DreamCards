namespace MightyAttributes
{
    [DarkBox("BoxName", false, false, true)]
    public class NoLabelDarkBoxAttribute : CustomWrapperAttribute
    {
        public string BoxName { get; }

        public NoLabelDarkBoxAttribute(string boxName) => BoxName = boxName;
    }
}