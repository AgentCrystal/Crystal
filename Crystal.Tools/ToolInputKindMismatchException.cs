namespace Crystal.Tools;

/// <summary>
/// Indicates that a model-requested tool input shape does not match its
/// registered definition.
/// </summary>
public sealed class ToolInputKindMismatchException : Exception
{
    internal ToolInputKindMismatchException()
        : base("The requested tool input shape does not match its definition.")
    {
    }
}
