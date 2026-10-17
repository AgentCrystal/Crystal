using Crystal.Tools;

namespace Crystal.Chat;

/// <summary>
/// Carries exact deltas for one streamed tool call.
/// </summary>
public sealed record ChatToolCallDelta : ChatItemStreamEvent
{
    /// <summary>
    /// Initializes a streamed tool-call delta.
    /// </summary>
    /// <param name="candidateIndex">The zero-based candidate index.</param>
    /// <param name="itemIndex">The zero-based item index.</param>
    /// <param name="callIdDelta">The exact call-identifier delta.</param>
    /// <param name="nameDelta">The exact tool-name delta.</param>
    /// <param name="argumentsDelta">The exact raw-arguments delta.</param>
    /// <param name="inputKind">
    /// The portable input shape. Every delta for one call must report the same
    /// value.
    /// </param>
    public ChatToolCallDelta(
        int candidateIndex,
        int itemIndex,
        string callIdDelta,
        string nameDelta,
        string argumentsDelta,
        ToolInputKind inputKind)
        : base(candidateIndex, itemIndex)
    {
        ArgumentNullException.ThrowIfNull(callIdDelta, nameof(callIdDelta));
        ArgumentNullException.ThrowIfNull(nameDelta, nameof(nameDelta));
        ArgumentNullException.ThrowIfNull(argumentsDelta, nameof(argumentsDelta));
        ArgumentNullException.ThrowIfNull(inputKind, nameof(inputKind));

        CallIdDelta = callIdDelta;
        NameDelta = nameDelta;
        ArgumentsDelta = argumentsDelta;
        InputKind = inputKind;
    }

    /// <summary>
    /// Gets the exact call-identifier delta.
    /// </summary>
    public string CallIdDelta { get; }

    /// <summary>
    /// Gets the exact tool-name delta.
    /// </summary>
    public string NameDelta { get; }

    /// <summary>
    /// Gets the exact raw-arguments delta.
    /// </summary>
    public string ArgumentsDelta { get; }

    /// <summary>
    /// Gets the portable input shape of the call.
    /// </summary>
    public ToolInputKind InputKind { get; }
}
