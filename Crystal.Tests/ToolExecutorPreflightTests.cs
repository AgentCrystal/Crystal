using System.Text.Json;

using Crystal.Multimodal;
using Crystal.Multimodal.Tools;
using Crystal.Tools;

namespace Crystal.Tests;

public sealed class ToolExecutorPreflightTests
{
    [Theory]
    [InlineData(ToolExecutionMode.Serial, 1)]
    [InlineData(ToolExecutionMode.Concurrent, 2)]
    public async Task UnknownTextToolStopsBatchBeforeAnyInvocation(
        ToolExecutionMode mode,
        int maximumConcurrency)
    {
        var tool = new RecordingTextTool("known");
        var executor = new ToolExecutor(
            new ToolCatalog([tool]),
            new ToolExecutionOptions(mode, maximumConcurrency));
        ToolCall[] calls =
        [
            new("first", "known", "{}", ToolInputKind.Json),
            new(
                "second",
                "missing-secret-name",
                "{\"private\":true}",
                ToolInputKind.Json)
        ];

        var failure = await Assert.ThrowsAsync<ToolNotFoundException>(
            () => executor.ExecuteAsync(calls));

        Assert.Equal(0, tool.InvocationCount);
        Assert.DoesNotContain("missing-secret-name", failure.Message);
        Assert.DoesNotContain("private", failure.Message);
    }

    [Theory]
    [InlineData(MultimodalToolExecutionMode.Serial, 1)]
    [InlineData(MultimodalToolExecutionMode.Concurrent, 2)]
    public async Task UnknownMultimodalToolStopsBatchBeforeAnyInvocation(
        MultimodalToolExecutionMode mode,
        int maximumConcurrency)
    {
        var tool = new RecordingMultimodalTool("known");
        var executor = new MultimodalToolExecutor(
            new MultimodalToolCatalog([tool]),
            new MultimodalToolExecutionOptions(mode, maximumConcurrency));
        MultimodalToolCall[] calls =
        [
            new(
                "first",
                "known",
                "{}",
                ToolInputKind.Json,
                [new TextContent("input")]),
            new(
                "second",
                "missing-secret-name",
                "{\"private\":true}",
                ToolInputKind.Json)
        ];

        var failure = await Assert.ThrowsAsync<MultimodalToolNotFoundException>(
            () => executor.ExecuteAsync(calls));

        Assert.Equal(0, tool.InvocationCount);
        Assert.DoesNotContain("missing-secret-name", failure.Message);
        Assert.DoesNotContain("private", failure.Message);
    }

    [Theory]
    [InlineData(ToolExecutionMode.Serial, 1)]
    [InlineData(ToolExecutionMode.Concurrent, 2)]
    public async Task MismatchedTextToolInputKindStopsBatchBeforeAnyInvocation(
        ToolExecutionMode mode,
        int maximumConcurrency)
    {
        var tool = new RecordingTextTool("known");
        var executor = new ToolExecutor(
            new ToolCatalog([tool]),
            new ToolExecutionOptions(mode, maximumConcurrency));
        ToolCall[] calls =
        [
            new("first", "known", "{}", ToolInputKind.Json),
            new("second", "known", "unstructured", ToolInputKind.Text)
        ];

        var failure = await Assert.ThrowsAsync<ToolInputKindMismatchException>(
            () => executor.ExecuteAsync(calls));

        Assert.Equal(0, tool.InvocationCount);
        Assert.DoesNotContain("unstructured", failure.Message);
    }

    [Theory]
    [InlineData(MultimodalToolExecutionMode.Serial, 1)]
    [InlineData(MultimodalToolExecutionMode.Concurrent, 2)]
    public async Task MismatchedMultimodalToolInputKindStopsBatchBeforeAnyInvocation(
        MultimodalToolExecutionMode mode,
        int maximumConcurrency)
    {
        var tool = new RecordingMultimodalTool("known");
        var executor = new MultimodalToolExecutor(
            new MultimodalToolCatalog([tool]),
            new MultimodalToolExecutionOptions(mode, maximumConcurrency));
        MultimodalToolCall[] calls =
        [
            new("first", "known", "{}", ToolInputKind.Json),
            new("second", "known", "unstructured", ToolInputKind.Text)
        ];

        var failure = await Assert.ThrowsAsync<ToolInputKindMismatchException>(
            () => executor.ExecuteAsync(calls));

        Assert.Equal(0, tool.InvocationCount);
        Assert.DoesNotContain("unstructured", failure.Message);
    }

    [Theory]
    [InlineData(ToolExecutionMode.Serial, 1)]
    [InlineData(ToolExecutionMode.Concurrent, 2)]
    public async Task MatchingTextToolInputKindExecutes(
        ToolExecutionMode mode,
        int maximumConcurrency)
    {
        var tool = new RecordingTextTool("known", textInput: true);
        var executor = new ToolExecutor(
            new ToolCatalog([tool]),
            new ToolExecutionOptions(mode, maximumConcurrency));

        var results = await executor.ExecuteAsync(
            [new ToolCall(
                "first", "known", "unstructured", ToolInputKind.Text)]);

        Assert.Equal(1, tool.InvocationCount);
        var result = Assert.Single(results);
        Assert.Equal("first", result.CallId);
        Assert.Equal("caller output", result.Text);
    }

    [Theory]
    [InlineData(MultimodalToolExecutionMode.Serial, 1)]
    [InlineData(MultimodalToolExecutionMode.Concurrent, 2)]
    public async Task MatchingMultimodalToolInputKindExecutes(
        MultimodalToolExecutionMode mode,
        int maximumConcurrency)
    {
        var tool = new RecordingMultimodalTool("known", textInput: true);
        var executor = new MultimodalToolExecutor(
            new MultimodalToolCatalog([tool]),
            new MultimodalToolExecutionOptions(mode, maximumConcurrency));

        var results = await executor.ExecuteAsync(
            [new MultimodalToolCall(
                "first", "known", "unstructured", ToolInputKind.Text)]);

        Assert.Equal(1, tool.InvocationCount);
        var result = Assert.Single(results);
        Assert.Equal("first", result.CallId);
        Assert.Equal(
            "caller output",
            Assert.IsType<TextContent>(Assert.Single(result.Contents)).Text);
    }

    private sealed class RecordingTextTool : ITool
    {
        public RecordingTextTool(string name, bool textInput = false)
        {
            if (textInput)
            {
                Definition = ToolDefinition.Text(name);
            }
            else
            {
                using var schema = JsonDocument.Parse("{}");
                Definition = ToolDefinition.Json(name, schema.RootElement);
            }
        }

        public ToolDefinition Definition { get; }

        public int InvocationCount { get; private set; }

        public ValueTask<ToolOutput> InvokeAsync(
            ToolCall call,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;
            return ValueTask.FromResult(new ToolOutput("caller output"));
        }
    }

    private sealed class RecordingMultimodalTool : IMultimodalTool
    {
        public RecordingMultimodalTool(string name, bool textInput = false)
        {
            if (textInput)
            {
                Definition = ToolDefinition.Text(name);
            }
            else
            {
                using var schema = JsonDocument.Parse("{}");
                Definition = ToolDefinition.Json(name, schema.RootElement);
            }
        }

        public ToolDefinition Definition { get; }

        public int InvocationCount { get; private set; }

        public ValueTask<MultimodalToolOutput> InvokeAsync(
            MultimodalToolCall call,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;
            return ValueTask.FromResult(new MultimodalToolOutput([new TextContent("caller output")]));
        }
    }
}
