namespace Crystal.Tools;

/// <summary>
/// Identifies the portable input shape of one caller-authored tool.
/// </summary>
public sealed record ToolInputKind
{
    /// <summary>
    /// Identifies structured JSON text described by a caller-authored JSON schema.
    /// </summary>
    public static ToolInputKind Json { get; } = new("json");

    /// <summary>
    /// Identifies unstructured text with no caller-authored JSON schema.
    /// </summary>
    public static ToolInputKind Text { get; } = new("text");

    private ToolInputKind(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the portable input-shape value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}
