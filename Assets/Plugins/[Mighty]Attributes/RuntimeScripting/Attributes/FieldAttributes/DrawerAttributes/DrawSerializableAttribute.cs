namespace MightyAttributes
{
    public class DrawSerializableAttribute : BaseDrawerAttribute
    {
        public SerializableOption SerializableOption { get; } = SerializableOption.Nothing;

        public string OptionCallback { get; }

        public DrawSerializableAttribute(SerializableOption option = SerializableOption.Nothing) : base((FieldOption) option) =>
            SerializableOption = option;

        public DrawSerializableAttribute(string optionCallback) : base(FieldOption.Nothing) => OptionCallback = optionCallback;
    }
}