namespace MightyAttributes
{
    [DrawSerializable("SerializableOption"), Line("Position")]
    public class LineSerializableAttribute : CustomWrapperAttribute
    {
        [CallbackName] public string Position { get; }

        public SerializableOption SerializableOption { get; }

        public LineSerializableAttribute(string linePosition = null, SerializableOption option = SerializableOption.Nothing)
        {
            Position = linePosition;
            SerializableOption = option;
        }
    }
}