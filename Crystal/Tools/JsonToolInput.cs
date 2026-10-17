using System.Text.Json;

namespace Crystal.Tools;

/// <summary>
/// Describes a tool whose arguments are JSON text described by a caller-authored
/// JSON Schema.
/// </summary>
public sealed record JsonToolInput : ToolInput
{
    /// <summary>
    /// Initializes a JSON tool input contract.
    /// </summary>
    /// <param name="schema">The caller-authored JSON Schema.</param>
    public JsonToolInput(JsonElement schema)
    {
        if (schema.ValueKind is not (JsonValueKind.Object
            or JsonValueKind.True or JsonValueKind.False))
        {
            throw new ArgumentException(
                "JSON input schema must be a JSON object or boolean.",
                nameof(schema));
        }

        Schema = schema.Clone();
    }

    /// <summary>
    /// Gets a clone of the caller-authored JSON Schema.
    /// </summary>
    public JsonElement Schema => field.Clone();

    /// <inheritdoc />
    public override ToolInputKind Kind => ToolInputKind.Json;

    /// <inheritdoc />
    public override string ToString() => nameof(JsonToolInput);
}
