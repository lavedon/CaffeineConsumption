using FluentAssertions;
using Caffeine.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Caffeine.Tests;

public class YamlParserTests
{
    private readonly IDeserializer _deserializer;

    public YamlParserTests()
    {
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
    }

    [Fact]
    public void Deserialize_ValidYaml_ReturnsDictionary()
    {
        // Arrange
        string yaml = @"
""3-13-2025"":
Total: 640
""3-14-2025"":
Total: 657
";

        // Act
        var result = _deserializer.Deserialize<Dictionary<string, DailyTotal>>(yaml);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result["3-13-2025"].Total.Should().Be(640);
        result["3-14-2025"].Total.Should().Be(657);
    }

    [Fact]
    public void Deserialize_EmptyYaml_ReturnsEmptyDictionary()
    {
        // Arrange
        string yaml = "{}";

        // Act
        var result = _deserializer.Deserialize<Dictionary<string, DailyTotal>>(yaml);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
