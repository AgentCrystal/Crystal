using System.Text.Json;

using Crystal.Tools;

namespace Crystal.Tests;

public sealed class ToolDefinitionTests
{
    [Fact]
    public void JsonInputSnapshotsObjectAfterDocumentDisposal()
    {
        ToolDefinition definition;
        using (var document = JsonDocument.Parse(
            "{\"type\":\"object\",\"description\":\"private\"}"))
        {
            definition = ToolDefinition.Json(
                "caller-tool",
                document.RootElement);
        }

        var input = Assert.IsType<JsonToolInput>(definition.Input);
        Assert.Equal(ToolInputKind.Json, input.Kind);
        Assert.Equal("object", input.Schema
            .GetProperty("type").GetString());
        Assert.Equal(nameof(ToolDefinition), definition.ToString());
    }

    [Fact]
    public void JsonInputAcceptsBooleanAndRejectsOtherRootValues()
    {
        using var booleanSchema = JsonDocument.Parse("true");
        using var invalidSchema = JsonDocument.Parse("[1]");

        var definition = ToolDefinition.Json(
            "caller-tool",
            booleanSchema.RootElement);
        Assert.Equal(
            JsonValueKind.True,
            Assert.IsType<JsonToolInput>(definition.Input).Schema.ValueKind);
        Assert.Equal(
            "schema",
            Assert.Throws<ArgumentException>(() =>
                ToolDefinition.Json("caller-tool", invalidSchema.RootElement))
                .ParamName);
        Assert.Equal(
            "schema",
            Assert.Throws<ArgumentException>(() =>
                new JsonToolInput(default)).ParamName);
    }

    [Fact]
    public void TextInputCarriesNoSchema()
    {
        var definition = ToolDefinition.Text(
            "capture",
            "Captures text input.");
        var input = Assert.IsType<TextToolInput>(definition.Input);

        Assert.Equal("capture", definition.Name);
        Assert.Equal("Captures text input.", definition.Description);
        Assert.Equal(ToolInputKind.Text, input.Kind);
        Assert.Equal(nameof(ToolDefinition), definition.ToString());
    }

    [Fact]
    public void TextDefinitionRejectsBlankName()
    {
        Assert.Equal(
            "name",
            Assert.Throws<ArgumentException>(() => ToolDefinition.Text(" "))
                .ParamName);
    }

    [Fact]
    public void DefinitionRejectsNullInput()
    {
        Assert.Equal(
            "input",
            Assert.Throws<ArgumentNullException>(() =>
                new ToolDefinition("caller-tool", null!)).ParamName);
    }
}
