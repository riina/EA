using System.IO;
using EA.WidthCategorizer;
using NUnit.Framework;

namespace EA.Tests;

public class Tests
{
    [Test]
    public void TestCategoriesFromData()
    {
        var categories = Catter.Categorize(File.OpenRead("EastAsianWidth.txt"), out _, out _, out _, out _);
        foreach (CRange range in categories)
        {
            EastAsianWidthKind kind = range.Kind;
            foreach (int i in range) Assert.That(EastAsianWidth.GetWidthKind(i), Is.EqualTo(kind), () => $"Failed check on character index {i} in range {range}");
        }
        Assert.Pass();
    }
}
