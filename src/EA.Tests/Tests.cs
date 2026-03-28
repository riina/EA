using System.Buffers.Binary;
using System.IO;
using System.Text;
using EA.WidthCategorizer;

namespace EA.Tests;

public class Tests
{
    [Fact]
    public void ParsedData_AllValuesResolveCorrectly()
    {
        var categories = Catter.Categorize(File.OpenRead("EastAsianWidth.txt"), out _, out _, out _, out _);
        foreach (CRange range in categories)
        {
            EastAsianWidthKind kind = range.Kind;
            foreach (int i in range) Assert.Equal(kind, EastAsianWidth.GetWidthKind(i));
        }
    }

    [Theory]
    [MemberData(nameof(TdGetWidthKindTestValues))]
    public void GetWidthKind_String_HasCorrectResolution(KnownEastAsianWidthKindInfo testValue)
    {
        Assert.Equal(testValue.Kind, EastAsianWidth.GetWidthKind(testValue.CodePoint, 0));
    }

    [Theory]
    [MemberData(nameof(TdGetWidthKindTestValues))]
    public void GetWidthKind_ReadOnlySpan_Char_HasCorrectResolution(KnownEastAsianWidthKindInfo testValue)
    {
        Assert.Equal(testValue.Kind, EastAsianWidth.GetWidthKind((System.ReadOnlySpan<char>)testValue.CodePoint, 0));
    }

    [Theory]
    [MemberData(nameof(TdGetWidthKindTestValues))]
    public void GetWidthKind_ReadOnlySpan_Byte_HasCorrectResolution(KnownEastAsianWidthKindInfo testValue)
    {
        Assert.Equal(testValue.Kind, EastAsianWidth.GetWidthKind(s_utf8Encoding.GetBytes(testValue.CodePoint), 0));
    }

    [Theory]
    [MemberData(nameof(TdGetWidthTestValues))]
    public void GetWidth_String_HasCorrectResolution(KnownEastAsianWidthLengthInfo testValue)
    {
        Assert.Equal(testValue.Width, EastAsianWidth.GetWidth(testValue.Value));
    }

    [Theory]
    [MemberData(nameof(TdGetWidthTestValues))]
    public void GetWidth_ReadOnlySpan_Char_HasCorrectResolution(KnownEastAsianWidthLengthInfo testValue)
    {
        Assert.Equal(testValue.Width, EastAsianWidth.GetWidth((System.ReadOnlySpan<char>)testValue.Value));
    }

    [Theory]
    [MemberData(nameof(TdGetWidthTestValues))]
    public void GetWidth_ReadOnlySpan_Byte_HasCorrectResolution(KnownEastAsianWidthLengthInfo testValue)
    {
        Assert.Equal(testValue.Width, EastAsianWidth.GetWidth(s_utf8Encoding.GetBytes(testValue.Value)));
    }

    [Theory]
    [MemberData(nameof(TdUtfStringTestValues))]
    public void Utf8Util_GetUtf32CodePointEnumerable_MatchesExpectedArray(string testValue)
    {
        byte[] expectedArray = s_utf32Encoding.GetBytes(testValue);
        MemoryStream ms = new();
        System.Span<byte> buffer = stackalloc byte[4];
        foreach (int value in Utf8Util.GetUtf32CodePointEnumerable(s_utf8Encoding.GetBytes(testValue)))
        {
            BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
            ms.Write(buffer);
        }
        Assert.Equal(expectedArray, ms.ToArray());
    }

    private static readonly UTF8Encoding s_utf8Encoding = new(encoderShouldEmitUTF8Identifier: false);
    private static readonly UTF32Encoding s_utf32Encoding = new(bigEndian: false, byteOrderMark: false);

    private static readonly KnownEastAsianWidthKindInfo[] s_getWidthKindTestValues =
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

    private static readonly KnownEastAsianWidthLengthInfo[] s_getWidthTestValues =
    [
        new("a", 1), //
        new("力", 2), //
        new("\u23F0", 2), //
        new("\uFF0B", 2), //
        new("\u20A9", 1), //
        new("\u00A9", 1), //
        new("\u2200", 1), //
        new("\U000F0000", 1), // "All private-use characters are by default classified as Ambiguous, because their definition depends on context."
        new("音楽の聴き方", 12),
    ];

    private static readonly string[] s_utfStringTestValues =
    [
        "a",
        "力",
        "\u23F0",
        "\uFF0B",
        "\u20A9",
        "\u00A9",
        "\u2200",
        "\U000F0000",
        "音楽の聴き方",
    ];
    public static readonly TheoryData<KnownEastAsianWidthLengthInfo> TdGetWidthTestValues = new(s_getWidthTestValues);

    public static readonly TheoryData<KnownEastAsianWidthKindInfo> TdGetWidthKindTestValues = new(s_getWidthKindTestValues);

    public static readonly TheoryData<string> TdUtfStringTestValues = new(s_utfStringTestValues);
}

public record KnownEastAsianWidthKindInfo(string CodePoint, EastAsianWidthKind Kind);

public record KnownEastAsianWidthLengthInfo(string Value, int Width);
