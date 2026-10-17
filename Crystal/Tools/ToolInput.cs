namespace Crystal.Tools;

/// <summary>
/// Describes the portable input contract of one caller-authored tool.
/// </summary>
public abstract record ToolInput
{
    private protected ToolInput()
    {
    }

    /// <summary>
    /// Gets the portable input shape.
    /// </summary>
    public abstract ToolInputKind Kind { get; }
}
