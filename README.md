# EA
 East Asian text utility library

## EastAsianWidth

The EastAsianWidth class provides utilities for working with the East Asian Width property of every Unicode code point.

### Using EastAsianWidth

Getting width category:

```csharp
// Width kind of char '音' (calls int codePoint overload)
Console.WriteLine($"{EastAsianWidth.GetWidthKind('音')}");
// Width kind of code point U+1F49C
Console.WriteLine($"{EastAsianWidth.GetWidthKind(0x1F49C)}");
// Width of code point starting at char 0 in "💜"
Console.WriteLine($"{EastAsianWidth.GetWidthKind("💜", 0)}");
```

Boolean checks:

```csharp
// Checks if char 'x' is narrow (calls int codePoint overload)
Console.WriteLine($"{EastAsianWidth.IsNarrow('x')}");
// Checks if "音楽の聴き方" contains any wide code points
Console.WriteLine($"{EastAsianWidth.ContainsWide("音楽の聴き方")}");
```

Getting width:

```csharp
// Gets width of code point 'x' (calls int codePoint overload)
// default values 1 for ambiguous, neutral, and private use chars
Console.WriteLine($"{EastAsianWidth.GetWidth('x')}");
// Gets width of string "💜"
// 2 for ambiguous, 1 for neutral, 0 for private use
Console.WriteLine($"{EastAsianWidth.GetWidth("💜", ambiguousWidth: 2, neutralWidth: 1, privateUseWidth: 0)}");
// Gets width of string "音楽の聴き方"
// 1 for neutral, 0 for private use, lambda evaluated for ambiguous because no explicit value was specified
Console.WriteLine($"{EastAsianWidth.GetWidth("音楽の聴き方", v => v == 0x212B ? 2 : 1, neutralWidth: 1, privateUseWidth: 0)}");
```

## Notices

EastAsianWidth.txt is used in tests and original category generation, licensed as follows

```
Copyright © 1991-2021 Unicode, Inc. All rights reserved.
Distributed under the Terms of Use in https://www.unicode.org/copyright.html.
```