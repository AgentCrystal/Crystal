using System.Text.Json;

namespace Crystal.Tools;

/// <summary>
/// Describes one caller-authored tool exposed to a model.
/// </summary>
public sealed record ToolDefinition
{
    /// <summary>
    /// Initializes a tool definition.
    /// </summary>
    /// <param name="name">The stable case-sensitive tool name.</param>
    /// <param name="input">The caller-authored input contract.</param>
    /// <param name="description">
    /// Optional caller-authored descriptive text.
    /// </param>
    public ToolDefinition(
        string name,
        ToolInput input,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        Name = name;
        Description = description;
        Input = input;
    }

    /// <summary>
    /// Initializes a JSON-input tool definition.
    /// </summary>
    /// <param name="name">The stable case-sensitive tool name.</param>
    /// <param name="inputSchema">The caller-authored JSON input schema.</param>
    /// <param name="description">
    /// Optional caller-authored descriptive text.
    /// </param>
    public static ToolDefinition Json(
        string name,
        JsonElement inputSchema,
        string? description = null) =>
        new(name, new JsonToolInput(inputSchema), description);

    /// <summary>
    /// Initializes a text-input tool definition.
    /// </summary>
    /// <param name="name">The stable case-sensitive tool name.</param>
    /// <param name="description">
    /// Optional caller-authored descriptive text.
    /// </param>
    public static ToolDefinition Text(
        string name,
        string? description = null) =>
        new(name, new TextToolInput(), description);

    /// <summary>
    /// Gets the stable case-sensitive name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the optional caller-authored description.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Gets the caller-authored input contract.
    /// </summary>
    public ToolInput Input { get; }

    /// <inheritdoc />
    public override string ToString() => nameof(ToolDefinition);
}
