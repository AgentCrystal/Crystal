using Crystal.Chat;

namespace Crystal.Tools;

/// <summary>
/// Contains one complete model-generated tool call.
/// </summary>
public sealed record ToolCall : ChatItem
{
    /// <summary>
    /// Initializes a tool call.
    /// </summary>
    /// <param name="callId">The model-generated correlation identifier.</param>
    /// <param name="name">The requested case-sensitive tool name.</param>
    /// <param name="arguments">
    /// The exact raw model-generated argument text.
    /// </param>
    /// <param name="inputKind">
    /// The portable input shape.
    /// </param>
    public ToolCall(
        string callId,
        string name,
        string arguments,
        ToolInputKind inputKind)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(callId, nameof(callId));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));
        ArgumentNullException.ThrowIfNull(inputKind, nameof(inputKind));

        CallId = callId;
        Name = name;
        Arguments = arguments;
        InputKind = inputKind;
    }

    /// <summary>
    /// Gets the correlation identifier.
    /// </summary>
    public string CallId { get; }

    /// <summary>
    /// Gets the requested tool name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the exact raw argument text.
    /// </summary>
    public string Arguments { get; }

    /// <summary>
    /// Gets the portable input shape of the call. Text input means
    /// <see cref="Arguments"/> is unstructured text; JSON input means it is
    /// caller-observed JSON text.
    /// </summary>
    public ToolInputKind InputKind { get; }
}
