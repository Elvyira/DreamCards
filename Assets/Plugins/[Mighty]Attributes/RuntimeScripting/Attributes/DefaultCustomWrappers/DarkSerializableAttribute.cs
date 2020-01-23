namespace MightyAttributes
{
    [DarkBox("BoxName", false, false, true)]
    public class DarkSerializableAttribute : LineSerializableAttribute
    {
        private string BoxName { get; }

        public DarkSerializableAttribute(string linePosition = null, string boxName = null,
            SerializableOption option = SerializableOption.Nothing) : base(linePosition, option) =>
        BoxName = boxName;
    }
}