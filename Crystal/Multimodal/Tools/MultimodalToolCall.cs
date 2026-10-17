using Crystal.Internal;
using Crystal.Multimodal.Chat;
using Crystal.Tools;

namespace Crystal.Multimodal.Tools;

/// <summary>Contains one complete model-generated multimodal tool call.</summary>
public sealed record MultimodalToolCall : MultimodalChatItem
{
    /// <summary>Initializes a multimodal tool call.</summary>
    /// <param name="callId">The model-generated correlation identifier.</param>
    /// <param name="name">The requested case-sensitive tool name.</param>
    /// <param name="arguments">The exact raw model-generated arguments.</param>
    /// <param name="inputKind">
    /// The portable input shape.
    /// </param>
    /// <param name="contents">Optional ordered model-generated typed content.</param>
    public MultimodalToolCall(
        string callId,
        string name,
        string arguments,
        ToolInputKind inputKind,
        IEnumerable<MultimodalContent>? contents = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(callId, nameof(callId));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));
        ArgumentNullException.ThrowIfNull(inputKind, nameof(inputKind));

        CallId = callId;
        Name = name;
        Arguments = arguments;
        InputKind = inputKind;
        Contents = CollectionSnapshot.Create(
            contents ?? Array.Empty<MultimodalContent>(),
            nameof(contents));
    }

    /// <summary>Gets the model-generated correlation identifier.</summary>
    public string CallId { get; }

    /// <summary>Gets the requested case-sensitive tool name.</summary>
    public string Name { get; }

    /// <summary>Gets the exact raw model-generated arguments.</summary>
    public string Arguments { get; }

    /// <summary>
    /// Gets the portable input shape of the call. Text input means
    /// <see cref="Arguments"/> is unstructured text; JSON input means it is
    /// caller-observed JSON text.
    /// </summary>
    public ToolInputKind InputKind { get; }

    /// <summary>Gets ordered model-generated typed content.</summary>
    public IReadOnlyList<MultimodalContent> Contents { get; }
}
