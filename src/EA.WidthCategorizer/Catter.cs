using System.Globalization;
using System.Text.RegularExpressions;

namespace EA.WidthCategorizer;

public static class Catter
{
    private static readonly Regex s_re = new(@"([0-9A-Fa-f]+)(?:\.\.([0-9A-Fa-f]+))?;(\w+)");

    public static List<CRange> Categorize(Stream stream, out int initial, out int final, out int original, out int injected)
    {
        SortedList<int, XRange> ranges = new();
        using StreamReader sr = new(stream);
        while (sr.ReadLine() is { } line)
        {
            int i = line.IndexOf('#');
            string sub = i == -1 ? line : line[..i];
            if (string.IsNullOrWhiteSpace(sub)) continue;
            if (s_re.Match(sub) is not { Success: true } match) throw new InvalidDataException($"Failed on {sub}");
            EastAsianWidthKind kind = match.Groups[3].Value switch
            {
                "A" => EastAsianWidthKind.Ambiguous,
                "F" => EastAsianWidthKind.Fullwidth,
                "H" => EastAsianWidthKind.Halfwidth,
                "N" => EastAsianWidthKind.Neutral,
                "Na" => EastAsianWidthKind.Narrow,
                "W" => EastAsianWidthKind.Wide,
                _ => throw new InvalidDataException($"Failed on category for {sub}")
            };
            int begInc = int.Parse(match.Groups[1].ValueSpan, NumberStyles.HexNumber);
            int endInc = match.Groups[2] is { Success: true } g ? int.Parse(g.ValueSpan, NumberStyles.HexNumber) : begInc;
            ranges.Add(begInc, new XRange(begInc, endInc, kind, false));
        }
        for (int i = 0; i < ranges.Count; i++) TryInjectAtIndex(ranges, i);
        original = ranges.Values.Count(v => !v.Injected);
        injected = ranges.Values.Count(v => v.Injected);
        initial = ranges.Count;
        Validate(ranges, 0x10FFFF + 1);
        for (int i = 0; i < ranges.Count; i++) TryCombineAtIndex(ranges, i);
        Validate(ranges, 0x10FFFF + 1);
        final = ranges.Count;
        return ranges.Values.Select(v => new CRange(v.BegInc, v.EndInc, v.Kind)).ToList();
    }

    private static void TryInjectAtIndex(SortedList<int, XRange> list, int index)
    {
        int next = index + 1 == list.Count ? 0x10FFFF + 1 : list.Keys[index + 1];
        int nStart = list.Values[index].EndInc + 1;
        if (next != nStart)
        {
/*
#  - All code points, assigned or unassigned, that are not listed
#      explicitly are given the value "N".
#  - The unassigned code points in the following blocks default to "W":
#         CJK Unified Ideographs Extension A: U+3400..U+4DBF
#         CJK Unified Ideographs:             U+4E00..U+9FFF
#         CJK Compatibility Ideographs:       U+F900..U+FAFF
#  - All undesignated code points in Planes 2 and 3, whether inside or
#      outside of allocated blocks, default to "W":
#         Plane 2:                            U+20000..U+2FFFD
#         Plane 3:                            U+30000..U+3FFFD

Private use:
U+E000..U+F8FF
U+F0000..U+FFFFD
U+100000..U+10FFFD
*/
            (EastAsianWidthKind kind, int maxExc) = nStart switch
            {
                >= 0x3400 and <= 0x4DBF => (EastAsianWidthKind.Wide, 0x4DBF + 1),
                >= 0x4E00 and <= 0x9FFF => (EastAsianWidthKind.Wide, 0x9FFF + 1),
                >= 0xF900 and <= 0xFAFF => (EastAsianWidthKind.Wide, 0xFAFF + 1),
                >= 0x20000 and <= 0x2FFFD => (EastAsianWidthKind.Wide, 0x2FFFD + 1),
                >= 0x30000 and <= 0x3FFFD => (EastAsianWidthKind.Wide, 0x3FFFD + 1),
                >= 0xE000 and <= 0xF8FF => (EastAsianWidthKind.PrivateUse, 0xF8FF + 1),
                >= 0xF0000 and <= 0xFFFFD => (EastAsianWidthKind.PrivateUse, 0xFFFFD + 1),
                >= 0x100000 and <= 0x10FFFD => (EastAsianWidthKind.PrivateUse, 0x10FFFD + 1),
                _ => (EastAsianWidthKind.Neutral, int.MaxValue)
            };
            maxExc = Math.Min(next, maxExc);
            list.Add(nStart, new XRange(nStart, maxExc - 1, kind, true));
        }
    }

    private static void Validate(SortedList<int, XRange> list, int maxExc)
    {
        if (list.Values[0].BegInc != 0)
            throw new ApplicationException($"Fail beginning {list.Values[0]}");
        for (int i = 0; i < list.Count; i++)
            if (list.Values[i].EndInc + 1 != (i + 1 == list.Count ? maxExc : list.Keys[i + 1]))
                throw new ApplicationException($"Fail {list.Values[i]}");
    }

    private static void TryCombineAtIndex(SortedList<int, XRange> list, int index)
    {
        while (index + 1 != list.Count)
        {
            var a = list.Values[index];
            var b = list.Values[index + 1];
            if (a.Kind != b.Kind) break;
            list.Remove(a.BegInc);
            list.Remove(b.BegInc);
            list.Add(a.BegInc, b with { BegInc = a.BegInc });
        }
    }

    private readonly record struct XRange(int BegInc, int EndInc, EastAsianWidthKind Kind, bool Injected);
}
