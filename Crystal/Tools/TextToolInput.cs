namespace Crystal.Tools;

/// <summary>
/// Describes a tool whose arguments are unstructured text.
/// </summary>
public sealed record TextToolInput : ToolInput
{
    /// <inheritdoc />
    public override ToolInputKind Kind => ToolInputKind.Text;

    /// <inheritdoc />
    public override string ToString() => nameof(TextToolInput);
}
