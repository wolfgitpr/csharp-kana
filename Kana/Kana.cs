using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kana
{
    public struct RomajiRes
    {
        public string Kana;
        public string Romaji;
        public bool Error;       //  Whether the conversion failed.
    }

    public class RomajiResList : List<RomajiRes>
    {
        public RomajiResList() : base() { }

        public RomajiResList(int capacity) : base(capacity) { }

        //  Convert to utf-16 string list.
        public List<string> ToStrList()
        {
            List<string> res = new List<string>();
            for (int i = 0; i < this.Count; i++)
            {
                res.Add(this[i].Error ? this[i].Kana : this[i].Romaji);
            }
            return res;
        }

        //  Convert to utf-16 string with delimiter(default: " ").
        public string ToStr(string delimiter = " ")
        {
            StringBuilder result = new StringBuilder();
            bool first = true;

            foreach (var res in this)
            {
                if (!first)
                {
                    result.Append(delimiter);
                }
                result.Append(res.Error ? res.Kana : res.Romaji);
                first = false;
            }

            return result.ToString();
        }
    }

    public struct KanaRes
    {
        public string Romaji;
        public string Kana;
        public bool Error;      //  Whether the conversion failed.
    }

    public class KanaResList : List<KanaRes>
    {
        public KanaResList() : base() { }

        public KanaResList(int capacity) : base(capacity) { }

        //  Convert to utf-16 string list.
        public List<string> ToStrList()
        {
            List<string> res = new List<string>();
            for (int i = 0; i < this.Count; i++)
            {
                res.Add(this[i].Error ? this[i].Romaji : this[i].Kana);
            }
            return res;
        }

        //  Convert to utf-16 string with delimiter(default: " ").
        public string ToStr(string delimiter = " ")
        {
            StringBuilder result = new StringBuilder();
            bool first = true;

            foreach (var res in this)
            {
                if (!first)
                {
                    result.Append(delimiter);
                }
                result.Append(res.Error ? res.Romaji : res.Kana);
                first = false;
            }

            return result.ToString();
        }
    }

    public enum Error
    {
        // Keep original characters
        Default = 0,
        // Ignore this character (do not export)
        Ignore = 1
    }

    public enum KanaType
    {
        Hiragana, Katakana
    }

    public class Kana
    {
        private const char hiraganaStart = '\u3040';
        private const char katakanaStart = '\u30A0';
        private const int kanaRange = 0x5E;

        public static List<string> SplitString(string input)
        {
            string pattern = @"(?![ー゜])([a-zA-Z]+|[+-]|[0-9]|[\u4e00-\u9fa5]|[\u3040-\u309F\u30A0-\u30FF][ャュョゃゅょァィゥェォぁぃぅぇぉ]?)";
            return Regex.Matches(input, pattern).Cast<Match>().Select(m => m.Value).ToList();
        }

        public static bool IsKana(string input)
        {
            return DictUtil.KanaToRomajiMap.ContainsKey(input);
        }

        public static bool IsKana(char input)
        {
            return DictUtil.KanaToRomajiMap.ContainsKey(input.ToString());
        }

        public static string ConvertKana(string kana, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
        {
            var convertedKana = "";
            foreach (char kanaChar in kana)
            {
                if (!IsKana(kanaChar) && error == Error.Ignore)
                    continue;
                if (kanaType == KanaType.Hiragana)
                {
                    // Target is Hiragana
                    if (kanaChar >= katakanaStart && kanaChar < katakanaStart + kanaRange)
                    {
                        // Katakana to Hiragana
                        convertedKana += (char)(kanaChar - katakanaStart + hiraganaStart);
                    }
                    else
                    {
                        convertedKana += kanaChar;
                    }
                }
                else
                {
                    // Target is Katakana
                    if (kanaChar >= hiraganaStart && kanaChar < hiraganaStart + kanaRange)
                    {
                        // Hiragana to Katakana
                        convertedKana += (char)(kanaChar + katakanaStart - hiraganaStart);
                    }
                    else
                    {
                        convertedKana += kanaChar;
                    }
                }
            }
            return convertedKana;
        }

        public static List<string> ConvertKana(List<string> kanaList, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
        {
            var convertedList = new List<string>();
            var rx = new Regex(@"[\u3040-\u309F\u30A0-\u30FF]+");

            foreach (var kana in kanaList)
            {
                var convertedKana = "";
                if (rx.IsMatch(kana))
                {
                    foreach (char kanaChar in kana)
                    {
                        if (kanaType == KanaType.Hiragana)
                        {
                            if (kanaChar >= katakanaStart && kanaChar < katakanaStart + kanaRange)
                            {
                                convertedKana += (char)(kanaChar - katakanaStart + hiraganaStart);
                            }
                            else
                            {
                                convertedKana += kanaChar;
                            }
                        }
                        else
                        {
                            if (kanaChar >= hiraganaStart && kanaChar < hiraganaStart + kanaRange)
                            {
                                convertedKana += (char)(kanaChar + katakanaStart - hiraganaStart);
                            }
                            else
                            {
                                convertedKana += kanaChar;
                            }
                        }
                    }
                }
                else
                {
                    convertedKana = kana;
                    if (error == Error.Ignore)
                        continue;
                }
                convertedList.Add(convertedKana);
            }
            return convertedList;
        }

        public static RomajiResList KanaToRomaji(List<string> kanaList, Error error = Error.Default, bool doubleWrittenSokuon = false)
        {
            var inputList = ConvertKana(kanaList, error, KanaType.Hiragana);
            RomajiResList res = new RomajiResList();

            foreach (var kana in inputList)
            {
                if (kana != "゜" && kana != "ー")
                {
                    if (DictUtil.KanaToRomajiMap.TryGetValue(kana, out var romaji))
                    {
                        res.Add(new RomajiRes { Kana = kana, Romaji = romaji, Error = false });
                    }
                    else
                    {
                        if (error == Error.Ignore)
                            continue;
                        res.Add(new RomajiRes { Kana = kana, Romaji = kana, Error = true });
                    }
                }
            }

            if (doubleWrittenSokuon)
            {
                for (int i = 0; i < res.Count - 1; ++i)
                {
                    if (res[i].Romaji == "cl")
                    {
                        res[i] = new RomajiRes { Kana = res[i].Kana, Romaji = res[i].Romaji[0] + res[i].Romaji, Error = res[i].Error };
                        res.RemoveAt(i);
                    }
                }
            }

            return res;
        }

        public static RomajiResList KanaToRomaji(string kanaStr, Error error = Error.Default, bool doubleWrittenSokuon = false)
        {
            return KanaToRomaji(SplitString(kanaStr), error, doubleWrittenSokuon);
        }

        public static KanaResList RomajiToKana(List<string> romajiList, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
        {
            var res = new KanaResList();

            foreach (var romaji in romajiList)
            {
                if (DictUtil.RomajiToKanaMap.TryGetValue(romaji, out var kana))
                {
                    res.Add(new KanaRes
                    {
                        Romaji = romaji,
                        Kana = kanaType == KanaType.Hiragana ? kana : ConvertKana(kana, error, KanaType.Katakana),
                        Error = false
                    });
                }
                else
                {
                    if (error == Error.Ignore)
                        continue;
                    res.Add(new KanaRes { Romaji = romaji, Kana = romaji, Error = true });
                }
            }

            return res;
        }

        public static KanaResList RomajiToKana(string romajiStr, Error error = Error.Default, KanaType kanaType = KanaType.Hiragana)
        {
            return RomajiToKana(SplitString(romajiStr), error, kanaType);
        }
    }
}