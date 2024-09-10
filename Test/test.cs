using Kana;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
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
            Console.WriteLine($"的 -> {Kana.Kana.IsKana("的")}");
            Console.WriteLine($"りゃ -> {Kana.Kana.IsKana("りゃ")}");
            Console.WriteLine($"ニャ -> {Kana.Kana.IsKana("ニャ")}");
        }
    }
}