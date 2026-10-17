using Crystal.Chat;
using Crystal.Multimodal.Chat;
using Crystal.Multimodal.Tools;
using Crystal.Tools;

namespace Crystal.Tests;

public sealed class ToolInputKindTests
{
    [Fact]
    public void ValuesAreStable()
    {
        Assert.Equal("json", ToolInputKind.Json.Value);
        Assert.Equal("text", ToolInputKind.Text.Value);
        Assert.Equal("text", ToolInputKind.Text.ToString());
    }

    [Fact]
    public void CompleteCallsCarryTheirInputKind()
    {
        var json = new ToolCall(
            "call-json",
            "tool",
            "{}",
            ToolInputKind.Json);
        var text = new ToolCall(
            "call-text",
            "tool",
            "first line\nsecond line",
            ToolInputKind.Text);
        var multimodalJson = new MultimodalToolCall(
            "call-multimodal-json",
            "tool",
            "{}",
            ToolInputKind.Json);
        var multimodalText = new MultimodalToolCall(
            "call-multimodal-text",
            "tool",
            "first line\nsecond line",
            ToolInputKind.Text);

        Assert.Equal(ToolInputKind.Json, json.InputKind);
        Assert.Equal(ToolInputKind.Text, text.InputKind);
        Assert.Equal("first line\nsecond line", text.Arguments);
        Assert.Equal(ToolInputKind.Json, multimodalJson.InputKind);
        Assert.Equal(ToolInputKind.Text, multimodalText.InputKind);
        Assert.Equal("first line\nsecond line", multimodalText.Arguments);
    }

    [Fact]
    public void StreamDeltasCarryInputKind()
    {
        var chat = new ChatToolCallDelta(
            0,
            0,
            "call",
            "tool",
            "text",
            ToolInputKind.Text);
        var multimodal = new MultimodalToolCallDelta(
            0,
            0,
            "call",
            "tool",
            "text",
            ToolInputKind.Text);

        Assert.Equal(ToolInputKind.Text, chat.InputKind);
        Assert.Equal(ToolInputKind.Text, multimodal.InputKind);
    }
}
