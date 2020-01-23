namespace MightyAttributes
{
    [Box("BoxName", false, false, true)]
    public class NoLabelBoxAttribute : CustomWrapperAttribute
    {
        public string BoxName { get; }

        public NoLabelBoxAttribute(string boxName) => BoxName = boxName;
    }
}