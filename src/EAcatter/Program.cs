// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text;
using EA;
using EA.WidthCategorizer;

const int vBits = 3; // 6+1 values => 8 = 2^3
const int vShift = 8 - vBits;
const int maxBits = 24 - vBits;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
#pragma warning disable CS0162
if (vBits > 8) throw new ApplicationException($"Too many value bits ({vBits}). Redesign data containers.");
if (0x10FFFF + 1 > 1 << maxBits) throw new ApplicationException($"Can't contain unicode in {maxBits} available bits. Redesign data containers.");
#pragma warning restore CS0162
// ReSharper restor ConditionIsAlwaysTrueOrFalse

if (args.Length is < 0 or > 2)
{
    Console.WriteLine("usage: <EastAsianWidth.txt> [Categories.cs]");
    return;
}
List<CRange> ranges = Catter.Categorize(File.OpenRead(args[0]), out int initial, out int final, out int original, out int injected);
Console.WriteLine($"{initial} initial, {final} final, {original} original, {injected} injected");
if (args.Length == 1) return;
byte[] buf = new byte[ranges.Count * 3];
for (int i = 0; i < ranges.Count; i++)
{
    (int begInc, _, EastAsianWidthKind eawKind) = ranges[i];
    int cv = (int)eawKind;
    if (cv >= 1 << vBits) throw new ApplicationException($"invalid value {cv}");
    buf[3 * i] = (byte)begInc;
    buf[3 * i + 1] = (byte)(begInc >> 8);
    buf[3 * i + 2] = (byte)((begInc >> 16) | (cv << vShift));
}

StringBuilder sb = new();
ArrayToContentString(buf, sb, 2);
File.WriteAllText(args[1], @$"namespace EA;

internal static class EastAsianWidthData
{{
    internal static ReadOnlySpan<byte> s_data => new byte[]
    {{
        {sb}
    }};


}}");

void ArrayToContentString(byte[] array, StringBuilder b, int indent)
{
    var ci = CultureInfo.InvariantCulture;
    for (int i = 0; i < array.Length; i++)
    {
        b.Append(ci, $"0x{array[i]:X2}");
        if (i + 1 != array.Length) b.Append(',');
        if ((i + 1) % 16 == 0) b.AppendLine().Append(' ', indent * 4);
        else b.Append(' ');
    }
}
