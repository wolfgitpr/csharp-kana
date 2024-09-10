# csharp-kana

## Intro

csharp-kana is a lightweight library for converting Japanese kana to romaji and vice versa.

It is written in Csharp and uses a simple algorithm to perform the conversion.

## Usage

```csharp
var raw1 = "これでもやれるだけ飞ばしてきたんだよ";
var res1 = Kana.Kana.KanaToRomaji(raw1, Error.Default, false).ToStr();
Console.WriteLine("test1: Kana->romaji (Default)");
Console.WriteLine($"{raw1} -> {res1}");

var res2 = Kana.Kana.RomajiToKana(res1, Error.Default, KanaType.Hiragana).ToStrList();
Console.WriteLine("test2: romaji->Hiragana");
Console.WriteLine($"{res1} -> {string.Join(" ", res2)}");

var res3 = Kana.Kana.ConvertKana(string.Join(" ", res2), Error.Default, KanaType.Katakana);
Console.WriteLine("test3: Hiragana->Katakana");
Console.WriteLine($"{string.Join(" ", res2)} -> {string.Join(" ", res3)}");

var raw2 = "こん好にちは";
var res4 = Kana.Kana.KanaToRomaji(raw2, Error.Ignore, true).ToStr();
Console.WriteLine("test4: Kana->romaji (Ignore)");
Console.WriteLine($"{raw2} -> {res4}");

Console.WriteLine("test5: IsKana");
Console.WriteLine($"こ -> {Kana.Kana.IsKana("こ")}");
```

## Doc

```csharp
//  Kana.cs
public struct RomajiRes
{
    public string Kana;
    public string Romaji;
    public bool Error;       //  Whether the conversion failed.
}

public class RomajiResList : List<RomajiRes>
{
    //  Convert to utf-16 string list.
    public List<string> ToStrList();

    //  Convert to utf-16 string with delimiter(default: " ").
    public string ToStr(string delimiter = " ");
}

public struct KanaRes
{
    public string Romaji;
    public string Kana;
    public bool Error;      //  Whether the conversion failed.
}

public class KanaResResList : List<KanaRes> { ... }

//  Kana.cs
enum class Error {
    // Keep original characters
    Default = 0,
    // Ignore this character (do not export)
    Ignore = 1
};

//  Kana.cs
public enum KanaType
{
    Hiragana, Katakana
}

// Split Chinese/English/Kana string into a list of characters.
public static List<string> SplitString(string input)

// IsKana
public static bool IsKana(char input)
public static bool IsKana(string input)

// Convert between Hiragana and Katakana.
public static string ConvertKana(string kana, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
public static List<string> ConvertKana(List<string> kanaList, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)

// KanaToRomaji
public static RomajiResList KanaToRomaji(string kanaStr, Error error = Error.Default, bool doubleWrittenSokuon = false)
public static RomajiResList KanaToRomaji(List<string> kanaList, Error error = Error.Default, bool doubleWrittenSokuon = false)

// RomajiToKana
public static KanaResResList RomajiToKana(string romajiStr, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
public static KanaResResList RomajiToKana(List<string> romajiList, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
```


## Related Projects

+ [cpp-kana](https://github.com/wolfgitpr/cpp-kana)
  A lightweight library for converting Japanese kana to romaji and vice versa.

+ [pinyin-makedict](https://github.com/wolfgitpr/pinyin-makedict)
  A tool for creating Chinese/Cantonese dictionaries.

+ [cpp-pinyin](https://github.com/wolfgitpr/cpp-pinyin)
  A C++ implementation of Chinese/Cantonese to Pinyin library.

+ [csharp-pinyin](https://github.com/wolfgitpr/csharp-pinyin)
  A Csharp implementation of Chinese/Cantonese to Pinyin library.

+ [python-pinyin](https://github.com/mozillazg/python-pinyin)
  A Python implementation of Chinese to Pinyin library.
