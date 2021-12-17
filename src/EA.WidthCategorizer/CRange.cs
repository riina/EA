using System.Collections;

namespace EA.WidthCategorizer;

public readonly record struct CRange(int BegInc, int EndInc, EastAsianWidthKind Kind) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = BegInc; i <= EndInc; i++)
            yield return i;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
