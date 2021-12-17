namespace EA;

/// <summary>
/// East Asian Width.
/// </summary>
public enum EastAsianWidthKind : byte
{
    /// <summary>
    /// East Asian Ambiguous (A).
    /// </summary>
    /// <remarks>
    /// All characters that can be sometimes wide and sometimes narrow. Ambiguous characters require additional information not contained in the character code to further resolve their width.
    /// </remarks>
    Ambiguous = 0,
    /// <summary>
    /// East Asian Fullwidth (F).
    /// </summary>
    /// <remarks>
    /// All characters that are defined as Fullwidth in the Unicode Standard by having a compatibility decomposition of type &lt;wide&gt; to characters elsewhere in the Unicode Standard that are implicitly narrow but unmarked.
    /// </remarks>
    Fullwidth = 1,
    /// <summary>
    /// East Asian Halfwidth (H).
    /// </summary>
    /// <remarks>
    /// All characters that are explicitly defined as Halfwidth in the Unicode Standard by having a compatibility decomposition of type &lt;narrow&gt; to characters elsewhere in the Unicode Standard that are implicitly wide but unmarked, plus U+20A9 ₩ WON SIGN.
    /// </remarks>
    Halfwidth = 2,
    /// <summary>
    /// East Asian Narrow (Na).
    /// </summary>
    /// <remarks>
    /// All other characters that are always narrow and have explicit fullwidth or wide counterparts. These characters are implicitly narrow in East Asian typography and legacy character sets because they have explicit fullwidth or wide counterparts. All of ASCII is an example of East Asian Narrow characters.
    /// </remarks>
    Narrow = 3,
    /// <summary>
    /// East Asian Wide (W).
    /// </summary>
    /// <remarks>
    /// All other characters that are always wide. These characters occur only in the context of East Asian typography where they are wide characters (such as the Unified Han Ideographs or Squared Katakana Symbols). This category includes characters that have explicit halfwidth counterparts, along with characters that have the UTS51 property Emoji_Presentation, with the exception of characters that have the UCD property Regional_Indicator
    /// </remarks>
    Wide = 4,
    /// <summary>
    /// Neutral (Not East Asian).
    /// </summary>
    /// <remarks>
    /// All other characters. Neutral characters do not occur in legacy East Asian character sets. By extension, they also do not occur in East Asian typography. For example, there is no traditional Japanese way of typesetting Devanagari. Canonical equivalents of narrow and neutral characters may not themselves be narrow or neutral respectively. For example, U+00C5 Å LATIN CAPITAL LETTER A WITH RING ABOVE is Neutral, but its decomposition starts with a Narrow character.
    /// </remarks>
    Neutral = 5,
    /// <summary>
    /// Private use.
    /// </summary>
    /// <remarks>
    /// Code points that are not (and never will be) assigned in Unicode.
    /// </remarks>
    PrivateUse = 6
}
