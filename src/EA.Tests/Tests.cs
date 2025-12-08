using System.IO;
using EA.WidthCategorizer;
using NUnit.Framework;

namespace EA.Tests;

public class Tests
{
    [Test]
    public void ParsedData_AllValuesResolveCorrectly()
    {
        var categories = Catter.Categorize(File.OpenRead("EastAsianWidth.txt"), out _, out _, out _, out _);
        foreach (CRange range in categories)
        {
            EastAsianWidthKind kind = range.Kind;
            foreach (int i in range) Assert.That(EastAsianWidth.GetWidthKind(i), Is.EqualTo(kind), () => $"Failed check on character index {i} in range {range}");
        }

        Assert.Pass();
    }

    [Test]
    public void TestValue_HasCorrectResolution([ValueSource(nameof(s_testValues))] KnownEastAsianWidthInfo testValue)
    {
        Assert.That(EastAsianWidth.GetWidthKind(testValue.CodePoint, 0), Is.EqualTo(testValue.Kind));
    }

    private static readonly KnownEastAsianWidthInfo[] s_testValues =
    [
        new("a", EastAsianWidthKind.Narrow), //
        new("力", EastAsianWidthKind.Wide), //
        new("\u23F0", EastAsianWidthKind.Wide), //
        new("\uFF0B", EastAsianWidthKind.Fullwidth), //
        new("\u20A9", EastAsianWidthKind.Halfwidth), //
        new("\u00A9", EastAsianWidthKind.Neutral), //
        new("\u2200", EastAsianWidthKind.Ambiguous), //
        new("\U000F0000", EastAsianWidthKind.Ambiguous), // "All private-use characters are by default classified as Ambiguous, because their definition depends on context."
    ];
}

public record KnownEastAsianWidthInfo(string CodePoint, EastAsianWidthKind Kind);
