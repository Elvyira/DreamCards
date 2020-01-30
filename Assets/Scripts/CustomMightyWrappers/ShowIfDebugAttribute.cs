using MightyAttributes;

[ShowIf("DebugName")]
public class ShowIfDebugAttribute : DebugAttribute
{
    [CallbackName] public string DebugName { get; }

    public ShowIfDebugAttribute(string debugName = "debug") => DebugName = debugName;
}
