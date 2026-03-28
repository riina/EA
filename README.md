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
// Width kind of code point starting at char 0 in "💜"
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
UNICODE LICENSE V3

COPYRIGHT AND PERMISSION NOTICE

Copyright © 1991-2025 Unicode, Inc.

NOTICE TO USER: Carefully read the following legal agreement. BY
DOWNLOADING, INSTALLING, COPYING OR OTHERWISE USING DATA FILES, AND/OR
SOFTWARE, YOU UNEQUIVOCALLY ACCEPT, AND AGREE TO BE BOUND BY, ALL OF THE
TERMS AND CONDITIONS OF THIS AGREEMENT. IF YOU DO NOT AGREE, DO NOT
DOWNLOAD, INSTALL, COPY, DISTRIBUTE OR USE THE DATA FILES OR SOFTWARE.

Permission is hereby granted, free of charge, to any person obtaining a
copy of data files and any associated documentation (the "Data Files") or
software and any associated documentation (the "Software") to deal in the
Data Files or Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, and/or sell
copies of the Data Files or Software, and to permit persons to whom the
Data Files or Software are furnished to do so, provided that either (a)
this copyright and permission notice appear with all copies of the Data
Files or Software, or (b) this copyright and permission notice appear in
associated Documentation.

THE DATA FILES AND SOFTWARE ARE PROVIDED "AS IS", WITHOUT WARRANTY OF ANY
KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT OF
THIRD PARTY RIGHTS.

IN NO EVENT SHALL THE COPYRIGHT HOLDER OR HOLDERS INCLUDED IN THIS NOTICE
BE LIABLE FOR ANY CLAIM, OR ANY SPECIAL INDIRECT OR CONSEQUENTIAL DAMAGES,
OR ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS,
WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION,
ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THE DATA
FILES OR SOFTWARE.

Except as contained in this notice, the name of a copyright holder shall
not be used in advertising or otherwise to promote the sale, use or other
dealings in these Data Files or Software without prior written
authorization of the copyright holder.
```
