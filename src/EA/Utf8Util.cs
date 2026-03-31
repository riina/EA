namespace EA;

internal static class Utf8Util
{
    public static Utf8ToUtf32CodePointEnumerable GetUtf32CodePointEnumerable(ReadOnlySpan<byte> utf8Span)
    {
        return new Utf8ToUtf32CodePointEnumerable(utf8Span);
    }

#if !NETCOREAPP
    private static readonly System.Text.UTF8Encoding s_utf8Encoding = new(encoderShouldEmitUTF8Identifier: false);
    private static System.Text.Decoder Utf8Decoder => s_utf8Decoder ??= s_utf8Encoding.GetDecoder();
    [ThreadStatic] private static System.Text.Decoder? s_utf8Decoder;
#endif

    public static void ConvertUtf8ToUtf16(ReadOnlySpan<byte> span, Span<char> resultBuffer, out int bytesRead, out int charsWritten)
    {
#if NETCOREAPP
        var status = System.Text.Unicode.Utf8.ToUtf16(span, resultBuffer, out bytesRead, out charsWritten, replaceInvalidSequences: false, isFinalBlock: true);
        switch (status)
        {
            case System.Buffers.OperationStatus.Done:
            case System.Buffers.OperationStatus.DestinationTooSmall:
                return;
            case System.Buffers.OperationStatus.NeedMoreData:
                throw new ArgumentException("impossible");
            case System.Buffers.OperationStatus.InvalidData:
                throw new ArgumentException("Encountered invalid data while processing input");
            default:
                throw new ArgumentOutOfRangeException();
        }
#else
        var decoder = Utf8Decoder;
        decoder.Reset();
        decoder.Convert(span, resultBuffer, true, out bytesRead, out charsWritten, out _);
#endif
    }

    public static int ConvertToUtf32(ReadOnlySpan<byte> span, int i)
    {
        if ((uint)i >= span.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(i));
        }
        span = span[i..];
        Span<char> tmp = stackalloc char[2];
        ConvertUtf8ToUtf16(span[i..], tmp, out int _, out int charsWritten);
        switch (charsWritten)
        {
            case 0:
                throw new ArgumentException($"No values available when reading from input at index {i}");
            case 1:
                return tmp[0];
            case 2:
                {
                    char c0 = tmp[0];
                    if (char.IsSurrogate(c0))
                    {
                        return char.ConvertToUtf32(c0, tmp[1]);
                    }
                    return tmp[0];
                }
            default:
                throw new ArgumentException($"Unexpected write of {charsWritten} chars");
        }
    }
}

internal readonly ref struct Utf8ToUtf32CodePointEnumerable
{
    private readonly ReadOnlySpan<byte> _span;

    public Utf8ToUtf32CodePointEnumerable(ReadOnlySpan<byte> span)
    {
        _span = span;
    }

    public Utf8ToUtf32CodePointEnumerator GetEnumerator()
    {
        return new Utf8ToUtf32CodePointEnumerator(_span);
    }
}

internal ref struct Utf8ToUtf32CodePointEnumerator
{
    private readonly ReadOnlySpan<byte> _span;
    private int _index;
    private int _current;

    public Utf8ToUtf32CodePointEnumerator(ReadOnlySpan<byte> span)
    {
        _span = span;
        _index = 0;
        _current = 0;
    }

    public int Current => _current;

    public bool MoveNext()
    {
        Span<char> tmp = stackalloc char[2];
        if (TryGetSingle(_span, tmp, _index, out int endIndex, out int value))
        {
            _index = endIndex;
            _current = value;
            return true;
        }
        _index = _span.Length;
        _current = 0;
        return false;
    }

    private static bool TryGetSingle(ReadOnlySpan<byte> span, Span<char> tmp, int startIndex, out int endIndex, out int value)
    {
        while (true)
        {
            if ((uint)startIndex >= span.Length)
            {
                endIndex = startIndex;
                value = 0;
                return false;
            }
            Utf8Util.ConvertUtf8ToUtf16(span[startIndex..], tmp, out int bytesRead, out int charsWritten);
            switch (charsWritten)
            {
                case 0:
                    throw new ArgumentException($"No values available when reading from input at index {startIndex}");
                case 1:
                    value = tmp[0];
                    endIndex = startIndex + bytesRead;
                    return true;
                case 2:
                    {
                        char c0 = tmp[0];
                        if (char.IsSurrogate(c0))
                        {
                            value = char.ConvertToUtf32(c0, tmp[1]);
                            endIndex = startIndex + bytesRead;
                            return true;
                        }
                        tmp = tmp[..1];
                        continue;
                    }
                default:
                    throw new ArgumentException($"Unexpected write of {charsWritten} chars");
            }
        }
    }

    public void Reset()
    {
        _index = 0;
        _current = 0;
    }
}
