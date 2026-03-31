namespace EA;

/// <summary>
/// Provides East Asian Width evaluation utilities.
/// </summary>
public static partial class EastAsianWidth
{
    #region Constants

    private const int VBits = 3; // 6+1 values => 8 = 2^3
    private const int VShift = 8 - VBits;
    private const int MaskUpper = (1 << VShift) - 1;

    #endregion

    #region Fields

    private static readonly Entry[] s_e;

    #endregion

    #region Initialization

    static EastAsianWidth()
    {
        ReadOnlySpan<byte> span = EastAsianWidthData.s_data;
        System.Diagnostics.Debug.Assert(span.Length % 3 == 0);
        int cc = span.Length / 3;
        s_e = new Entry[cc];
        for (int i = 0; i < cc; i++)
        {
            byte a = span[i * 3], b = span[i * 3 + 1], c = span[i * 3 + 2];
            s_e[i] = new Entry(a | (b << 8) | ((c & MaskUpper) << 16), (EastAsianWidthKind)(c >> VShift));
        }
    }

    #endregion

    #region Public API

    #region GetWidth

    /// <summary>
    /// Gets the width of a width kind.
    /// </summary>
    /// <param name="kind">Width kind.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidth(EastAsianWidthKind kind, int ambiguousWidth = 1, int neutralWidth = 1, int privateUseWidth = 1)
    {
        return kind switch
        {
            EastAsianWidthKind.Ambiguous => ambiguousWidth,
            EastAsianWidthKind.Fullwidth => 2,
            EastAsianWidthKind.Halfwidth => 1,
            EastAsianWidthKind.Narrow => 1,
            EastAsianWidthKind.Wide => 2,
            EastAsianWidthKind.Neutral => neutralWidth,
            EastAsianWidthKind.PrivateUse => privateUseWidth,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }

    /// <summary>
    /// Gets the width of a code point.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if kind is invalid.</exception>
    public static int GetWidth(int codePoint, int ambiguousWidth = 1, int neutralWidth = 1, int privateUseWidth = 1)
    {
        return GetWidth(GetWidthKind(codePoint), ambiguousWidth, neutralWidth, privateUseWidth);
    }

    #endregion

    #region GetWidthKind

    /// <summary>
    /// Gets the width kind for the specified Unicode code point.
    /// </summary>
    /// <param name="codePoint">Unicode code point.</param>
    /// <returns>Width category.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if character is outside the range of valid Unicode code points.</exception>
    public static EastAsianWidthKind GetWidthKind(int codePoint)
    {
        if (codePoint is > 0x10FFFF or < 0) throw new ArgumentOutOfRangeException(nameof(codePoint), "Character out of Unicode range");
        return GetWidthKindInternal(codePoint);
    }

    #endregion

    #region HasX

    /// <summary>
    /// Checks if this code point has definite East Asian Width.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point has definite width.</returns>
    public static bool HasDefiniteWidth(int codePoint)
    {
        return GetWidthKind(codePoint) switch
        {
            EastAsianWidthKind.Ambiguous => false,
            EastAsianWidthKind.Fullwidth => true,
            EastAsianWidthKind.Halfwidth => true,
            EastAsianWidthKind.Narrow => true,
            EastAsianWidthKind.Wide => true,
            EastAsianWidthKind.Neutral => false,
            EastAsianWidthKind.PrivateUse => false,
            _ => throw new ArgumentException()
        };
    }

    #endregion

    #region IsX

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Ambiguous"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsAmbiguous(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.Ambiguous;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Fullwidth"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsFullwidth(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.Fullwidth;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Halfwidth"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsHalfwidth(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.Halfwidth;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Narrow"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsNarrow(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.Narrow;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Wide"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsWide(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.Wide;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Neutral"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsNeutral(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.Neutral;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.PrivateUse"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsPrivateUse(int codePoint) => GetWidthKind(codePoint) == EastAsianWidthKind.PrivateUse;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Fullwidth"/> or <see cref="EastAsianWidthKind.Wide"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsFullwidthOrWide(int codePoint) => GetWidthKind(codePoint) is EastAsianWidthKind.Fullwidth or EastAsianWidthKind.Wide;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Halfwidth"/> or <see cref="EastAsianWidthKind.Narrow"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsHalfwidthOrNarrow(int codePoint) => GetWidthKind(codePoint) is EastAsianWidthKind.Halfwidth or EastAsianWidthKind.Narrow;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Fullwidth"/>, <see cref="EastAsianWidthKind.Wide"/>, or <see cref="EastAsianWidthKind.Ambiguous"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsFullwidthOrWideOrAmbiguous(int codePoint) => GetWidthKind(codePoint) is EastAsianWidthKind.Fullwidth or EastAsianWidthKind.Wide or EastAsianWidthKind.Ambiguous;

    /// <summary>
    /// Checks if this code point is <see cref="EastAsianWidthKind.Halfwidth"/>,  <see cref="EastAsianWidthKind.Narrow"/>, or <see cref="EastAsianWidthKind.Ambiguous"/>.
    /// </summary>
    /// <param name="codePoint">Code point.</param>
    /// <returns>True if code point matches this classification.</returns>
    public static bool IsHalfwidthOrNarrowOrAmbiguous(int codePoint) => GetWidthKind(codePoint) is EastAsianWidthKind.Halfwidth or EastAsianWidthKind.Narrow or EastAsianWidthKind.Ambiguous;

    #endregion

    #endregion

    #region Internals

    private static int ResolveWidth(int codePoint, EastAsianWidthKind kind, Func<int, int> defaultWidth, int? ambiguousWidth, int? neutralWidth, int? privateUseWidth)
    {
        return kind switch
        {
            EastAsianWidthKind.Ambiguous => ambiguousWidth ?? defaultWidth(codePoint),
            EastAsianWidthKind.Fullwidth => 2,
            EastAsianWidthKind.Halfwidth => 1,
            EastAsianWidthKind.Narrow => 1,
            EastAsianWidthKind.Wide => 2,
            EastAsianWidthKind.Neutral => neutralWidth ?? defaultWidth(codePoint),
            EastAsianWidthKind.PrivateUse => privateUseWidth ?? defaultWidth(codePoint),
            _ => throw new ArgumentException()
        };
    }

    private static int ResolveWidth(int codePoint, EastAsianWidthKind kind, Func<int, EastAsianWidthKind, int> defaultWidth, int? ambiguousWidth, int? neutralWidth, int? privateUseWidth)
    {
        return kind switch
        {
            EastAsianWidthKind.Ambiguous => ambiguousWidth ?? defaultWidth(codePoint, kind),
            EastAsianWidthKind.Fullwidth => 2,
            EastAsianWidthKind.Halfwidth => 1,
            EastAsianWidthKind.Narrow => 1,
            EastAsianWidthKind.Wide => 2,
            EastAsianWidthKind.Neutral => neutralWidth ?? defaultWidth(codePoint, kind),
            EastAsianWidthKind.PrivateUse => privateUseWidth ?? defaultWidth(codePoint, kind),
            _ => throw new ArgumentException()
        };
    }

    private static EastAsianWidthKind GetWidthKindInternal(int ch)
    {
        ReadOnlySpan<Entry> span = s_e;
        int l = 0, u = span.Length - 1;
        while (l <= u)
        {
            int m = l + (u - l) / 2;
            Entry e = span[m];
            switch (ch - e.Value)
            {
                case 0:
                    return e.Kind;
                case > 0:
                    l = m + 1;
                    break;
                default:
                    u = m - 1;
                    break;
            }
        }
        return span[l - 1].Kind;
    }

    #endregion

    #region Types

    private readonly struct Entry
    {
        public readonly int Value;
        public readonly EastAsianWidthKind Kind;

        public Entry(int value, EastAsianWidthKind kind)
        {
            Value = value;
            Kind = kind;
        }
    }

    #endregion
}
