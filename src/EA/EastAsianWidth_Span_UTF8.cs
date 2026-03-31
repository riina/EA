namespace EA;

partial class EastAsianWidth
{
    #region Public API

    #region GetWidth

    /// <summary>
    /// Gets the width of a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidth(ReadOnlySpan<byte> s, int ambiguousWidth = 1, int neutralWidth = 1, int privateUseWidth = 1)
    {
        int sum = 0;
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            sum += GetWidth(codePoint, ambiguousWidth, neutralWidth, privateUseWidth);
        }
        return sum;
    }

    /// <summary>
    /// Gets the width of a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="defaultWidth">Width override for characters where no default was provided.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidth(ReadOnlySpan<byte> s, Func<int, int> defaultWidth, int? ambiguousWidth = null, int? neutralWidth = null, int? privateUseWidth = null)
    {
        int sum = 0;
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            sum += ResolveWidth(codePoint, GetWidthKind(codePoint), defaultWidth, ambiguousWidth, neutralWidth, privateUseWidth);
        }
        return sum;
    }

    /// <summary>
    /// Gets the width of a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="defaultWidth">Width override for characters where no default was provided.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidth(ReadOnlySpan<byte> s, Func<int, EastAsianWidthKind, int> defaultWidth, int? ambiguousWidth = null, int? neutralWidth = null, int? privateUseWidth = null)
    {
        int sum = 0;
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            sum += ResolveWidth(codePoint, GetWidthKind(codePoint), defaultWidth, ambiguousWidth, neutralWidth, privateUseWidth);
        }
        return sum;
    }

    #endregion

    #region GetWidthOfCodePoint

    /// <summary>
    /// Gets the width of the Unicode code point at the specified offset in a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="index">Index.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidthOfCodePoint(ReadOnlySpan<byte> s, int index, int ambiguousWidth = 1, int neutralWidth = 1, int privateUseWidth = 1)
    {
        return GetWidth(GetWidthKind(s, index), ambiguousWidth, neutralWidth, privateUseWidth);
    }

    /// <summary>
    /// Gets the width of the Unicode code point at the specified offset in a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="index">Index.</param>
    /// <param name="defaultWidth">Width override for characters where no default was provided.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidthOfCodePoint(ReadOnlySpan<byte> s, int index, Func<int, int> defaultWidth, int? ambiguousWidth = null, int? neutralWidth = null, int? privateUseWidth = null)
    {
        int codePoint = Utf8Util.ConvertToUtf32(s, index);
        return ResolveWidth(codePoint, GetWidthKind(codePoint), defaultWidth, ambiguousWidth, neutralWidth, privateUseWidth);
    }

    /// <summary>
    /// Gets the width of the Unicode code point at the specified offset in a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="index">Index.</param>
    /// <param name="defaultWidth">Width override for characters where no default was provided.</param>
    /// <param name="ambiguousWidth">Width to use for <see cref="EastAsianWidthKind.Ambiguous"/> characters.</param>
    /// <param name="neutralWidth">Width to use for <see cref="EastAsianWidthKind.Neutral"/> characters.</param>
    /// <param name="privateUseWidth">Width to use for <see cref="EastAsianWidthKind.PrivateUse"/> characters.</param>
    /// <returns>Width.</returns>
    public static int GetWidthOfCodePoint(ReadOnlySpan<byte> s, int index, Func<int, EastAsianWidthKind, int> defaultWidth, int? ambiguousWidth = null, int? neutralWidth = null, int? privateUseWidth = null)
    {
        int codePoint = Utf8Util.ConvertToUtf32(s, index);
        return ResolveWidth(codePoint, GetWidthKind(codePoint), defaultWidth, ambiguousWidth, neutralWidth, privateUseWidth);
    }

    #endregion

    #region GetWidthKind

    /// <summary>
    /// Gets the width kind for the Unicode code point at the specified offset in a UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <param name="index">Index.</param>
    /// <returns>Width kind for code point.</returns>
    public static EastAsianWidthKind GetWidthKind(ReadOnlySpan<byte> s, int index)
    {
        return GetWidthKind(Utf8Util.ConvertToUtf32(s, index));
    }

    #endregion

    #region HasX

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> is only composed of code points with a definite East Asian Width.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if all code points have definite width.</returns>
    public static bool HasDefiniteWidth(ReadOnlySpan<byte> s)
    {
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            EastAsianWidthKind kind = GetWidthKind(codePoint);
            switch (kind)
            {
                case EastAsianWidthKind.Ambiguous:
                    return false;
                case EastAsianWidthKind.Fullwidth:
                    break;
                case EastAsianWidthKind.Halfwidth:
                    break;
                case EastAsianWidthKind.Narrow:
                    break;
                case EastAsianWidthKind.Wide:
                    break;
                case EastAsianWidthKind.Neutral:
                    return false;
                case EastAsianWidthKind.PrivateUse:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return true;
    }

    #endregion

    #region ContainsX

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Ambiguous"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsAmbiguous(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Ambiguous);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Fullwidth"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsFullwidth(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Fullwidth);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Halfwidth"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsHalfwidth(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Halfwidth);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Narrow"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsNarrow(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Narrow);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Wide"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsWide(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Wide);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Neutral"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsNeutral(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Neutral);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.PrivateUse"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsPrivateUse(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.PrivateUse);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any code points that have definite East Asian Width.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsDefiniteWidth(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Fullwidth, EastAsianWidthKind.Halfwidth, EastAsianWidthKind.Narrow, EastAsianWidthKind.Wide);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Fullwidth"/> or <see cref="EastAsianWidthKind.Wide"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsFullwidthOrWide(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Fullwidth, EastAsianWidthKind.Wide);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Halfwidth"/> or <see cref="EastAsianWidthKind.Narrow"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsHalfwidthOrNarrow(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Halfwidth, EastAsianWidthKind.Narrow);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Fullwidth"/>, <see cref="EastAsianWidthKind.Wide"/>, or <see cref="EastAsianWidthKind.Ambiguous"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsFullwidthOrWideOrAmbiguous(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Fullwidth, EastAsianWidthKind.Wide, EastAsianWidthKind.Ambiguous);

    /// <summary>
    /// Checks if this UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> contains any <see cref="EastAsianWidthKind.Halfwidth"/>,  <see cref="EastAsianWidthKind.Narrow"/>, or <see cref="EastAsianWidthKind.Ambiguous"/> code points.
    /// </summary>
    /// <param name="s">UTF-8 encoded <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.</param>
    /// <returns>True if any such code points are contained.</returns>
    public static bool ContainsHalfwidthOrNarrowOrAmbiguous(ReadOnlySpan<byte> s) => ContainsFilter(s, EastAsianWidthKind.Halfwidth, EastAsianWidthKind.Narrow, EastAsianWidthKind.Ambiguous);

    #endregion

    #endregion

    #region Internals

    private static bool ContainsFilter(ReadOnlySpan<byte> s, EastAsianWidthKind kind1)
    {
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            EastAsianWidthKind kind = GetWidthKind(codePoint);
            if (kind1 == kind) return true;
        }
        return false;
    }

    private static bool ContainsFilter(ReadOnlySpan<byte> s, EastAsianWidthKind kind1, EastAsianWidthKind kind2)
    {
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            EastAsianWidthKind kind = GetWidthKind(codePoint);
            if (kind1 == kind || kind2 == kind) return true;
        }
        return false;
    }

    private static bool ContainsFilter(ReadOnlySpan<byte> s, EastAsianWidthKind kind1, EastAsianWidthKind kind2, EastAsianWidthKind kind3)
    {
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            EastAsianWidthKind kind = GetWidthKind(codePoint);
            if (kind1 == kind || kind2 == kind || kind3 == kind) return true;
        }
        return false;
    }

    private static bool ContainsFilter(ReadOnlySpan<byte> s, EastAsianWidthKind kind1, EastAsianWidthKind kind2, EastAsianWidthKind kind3, EastAsianWidthKind kind4)
    {
        foreach (int codePoint in Utf8Util.GetUtf32CodePointEnumerable(s))
        {
            EastAsianWidthKind kind = GetWidthKind(codePoint);
            if (kind1 == kind || kind2 == kind || kind3 == kind || kind4 == kind) return true;
        }
        return false;
    }

    #endregion
}
