using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtension
    {
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
        public static string ToTitleCase(this String str)
        {
            if (str != null)
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            return null;
        }
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string ReplaceWithDic(this string str, Dictionary<string, object> replace)
        {
            if (replace != null)
            {
                var matches = Regex.Matches(str, @"\{(.+?)\}");
                List<string> mots = (from Match matche in matches select matche.Groups[1].Value).ToList();

                str = mots.Aggregate(
                        str,
                        (current, key) =>
                        {
                            int colonIndex = key.IndexOf(':');
                            var keyDic = colonIndex > 0 ? key.Substring(0, colonIndex) : key;
                            return replace.ContainsKey(keyDic) ? current.Replace("{" + key + "}",
                                colonIndex > 0
                                ? string.Format("{0:" + key.Substring(colonIndex + 1) + "}", replace[key.Substring(0, colonIndex)])
                                : replace[key].ToString()) : current;
                        });
            }
            return str;
        }
        public static string Replace<T>(this string str, T replace)
        {
            if (replace != null)
            {
                var dic = replace.PropertyToDictionary();
                return str.ReplaceWithDic(dic);
            }
            return str;
        }
        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out int result))
                return result;
            return 0;
        }
        public static int[] ToInt(this string[] str, int? total = null)
        {
            var tab = new List<int>();

            foreach (var item in str)
            {
                if (int.TryParse(item, out int result))
                    tab.Add(result);
            }

            if (total.HasValue && tab.Count() < total.Value)
            {
                for (var i = tab.Count(); i < total.Value; i++)
                    tab.Add(0);
            }

            return tab.ToArray();
        }
        public static double ToDouble(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (double.TryParse(str.Replace('.', ','), NumberStyles.Float, CultureInfo.CreateSpecificCulture("fr-FR"), out double result))
                    return result;
            }

            return 0;
        }
        public static int? ToNullableInt(this string str)
        {
            if (int.TryParse(str, out int result))
                return (int?)result;
            return (int?)null;
        }
        public static string StringToHex(this string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.GetEncoding(1252);
            byte[] ba = encoding.GetBytes(str);
            var hexstring = BitConverter.ToString(ba);
            hexstring = hexstring.Replace("-", String.Empty);

            return hexstring;
        }
        public static string HexToString(this string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.GetEncoding(1252);
            byte[] rawData = FromHexToByteArray(str);
            string result = encoding.GetString(rawData);

            return result;
        }
        public static byte[] FromHexToByteArray(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        public static string GetFileName(this string str)
        {
            var normalizedString = str.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
            }
            return stringBuilder.ToString().Replace(" ", "_").Normalize(NormalizationForm.FormC);
        }
        public static DateTime? ToDateTime(this string str)
        {
            var result = DateTime.MinValue;
            if (DateTime.TryParse(str, System.Globalization.CultureInfo.GetCultureInfo("fr-fr"), System.Globalization.DateTimeStyles.None, out result))
                return result;
            else return null;
        }
        public static string ToBase64Encode(this string plainText)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            return string.Empty;
        }
        public static string FromBase64Encode(this string base64EncodedData)
        {
            if (!string.IsNullOrEmpty(base64EncodedData))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            return string.Empty;
        }

        private class StackEntry
        {
            public int NumberOfCharactersToSkip { get; set; }
            public bool Ignorable { get; set; }

            public StackEntry(int numberOfCharactersToSkip, bool ignorable)
            {
                NumberOfCharactersToSkip = numberOfCharactersToSkip;
                Ignorable = ignorable;
            }
        }
        private static readonly Regex _rtfRegex = new Regex(@"\\([a-z]{1,32})(-?\d{1,10})?[ ]?|\\'([0-9a-f]{2})|\\([^a-z])|([{}])|[\r\n]+|(.)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static readonly List<string> destinations = new List<string>
    {
        "aftncn","aftnsep","aftnsepc","annotation","atnauthor","atndate","atnicn","atnid",
        "atnparent","atnref","atntime","atrfend","atrfstart","author","background",
        "bkmkend","bkmkstart","blipuid","buptim","category","colorschememapping",
        "colortbl","comment","company","creatim","datafield","datastore","defchp","defpap",
        "do","doccomm","docvar","dptxbxtext","ebcend","ebcstart","factoidname","falt",
        "fchars","ffdeftext","ffentrymcr","ffexitmcr","ffformat","ffhelptext","ffl",
        "ffname","ffstattext","field","file","filetbl","fldinst","fldrslt","fldtype",
        "fname","fontemb","fontfile","fonttbl","footer","footerf","footerl","footerr",
        "footnote","formfield","ftncn","ftnsep","ftnsepc","g","generator","gridtbl",
        "header","headerf","headerl","headerr","hl","hlfr","hlinkbase","hlloc","hlsrc",
        "hsv","htmltag","info","keycode","keywords","latentstyles","lchars","levelnumbers",
        "leveltext","lfolevel","linkval","list","listlevel","listname","listoverride",
        "listoverridetable","listpicture","liststylename","listtable","listtext",
        "lsdlockedexcept","macc","maccPr","mailmerge","maln","malnScr","manager","margPr",
        "mbar","mbarPr","mbaseJc","mbegChr","mborderBox","mborderBoxPr","mbox","mboxPr",
        "mchr","mcount","mctrlPr","md","mdeg","mdegHide","mden","mdiff","mdPr","me",
        "mendChr","meqArr","meqArrPr","mf","mfName","mfPr","mfunc","mfuncPr","mgroupChr",
        "mgroupChrPr","mgrow","mhideBot","mhideLeft","mhideRight","mhideTop","mhtmltag",
        "mlim","mlimloc","mlimlow","mlimlowPr","mlimupp","mlimuppPr","mm","mmaddfieldname",
        "mmath","mmathPict","mmathPr","mmaxdist","mmc","mmcJc","mmconnectstr",
        "mmconnectstrdata","mmcPr","mmcs","mmdatasource","mmheadersource","mmmailsubject",
        "mmodso","mmodsofilter","mmodsofldmpdata","mmodsomappedname","mmodsoname",
        "mmodsorecipdata","mmodsosort","mmodsosrc","mmodsotable","mmodsoudl",
        "mmodsoudldata","mmodsouniquetag","mmPr","mmquery","mmr","mnary","mnaryPr",
        "mnoBreak","mnum","mobjDist","moMath","moMathPara","moMathParaPr","mopEmu",
        "mphant","mphantPr","mplcHide","mpos","mr","mrad","mradPr","mrPr","msepChr",
        "mshow","mshp","msPre","msPrePr","msSub","msSubPr","msSubSup","msSubSupPr","msSup",
        "msSupPr","mstrikeBLTR","mstrikeH","mstrikeTLBR","mstrikeV","msub","msubHide",
        "msup","msupHide","mtransp","mtype","mvertJc","mvfmf","mvfml","mvtof","mvtol",
        "mzeroAsc","mzeroDesc","mzeroWid","nesttableprops","nextfile","nonesttables",
        "objalias","objclass","objdata","object","objname","objsect","objtime","oldcprops",
        "oldpprops","oldsprops","oldtprops","oleclsid","operator","panose","password",
        "passwordhash","pgp","pgptbl","picprop","pict","pn","pnseclvl","pntext","pntxta",
        "pntxtb","printim","private","propname","protend","protstart","protusertbl","pxe",
        "result","revtbl","revtim","rsidtbl","rxe","shp","shpgrp","shpinst",
        "shppict","shprslt","shptxt","sn","sp","staticval","stylesheet","subject","sv",
        "svb","tc","template","themedata","title","txe","ud","upr","userprops",
        "wgrffmtfilter","windowcaption","writereservation","writereservhash","xe","xform",
        "xmlattrname","xmlattrvalue","xmlclose","xmlname","xmlnstbl",
        "xmlopen"
    };
        private static readonly Dictionary<string, string> specialCharacters = new Dictionary<string, string>
    {
        { "par", "\n" },
        { "sect", "\n\n" },
        { "page", "\n\n" },
        { "line", "\n" },
        { "tab", "\t" },
        { "emdash", "\u2014" },
        { "endash", "\u2013" },
        { "emspace", "\u2003" },
        { "enspace", "\u2002" },
        { "qmspace", "\u2005" },
        { "bullet", "\u2022" },
        { "lquote", "\u2018" },
        { "rquote", "\u2019" },
        { "ldblquote", "\u201C" },
        { "rdblquote", "\u201D" },
    };
        public static string StripRichTextFormat(this string inputRtf)
        {
            if (inputRtf == null)
            {
                return null;
            }

            string returnString;

            var stack = new Stack<StackEntry>();
            bool ignorable = false;              // Whether this group (and all inside it) are "ignorable".
            int ucskip = 1;                      // Number of ASCII characters to skip after a unicode character.
            int curskip = 0;                     // Number of ASCII characters left to skip
            var outList = new List<string>();    // Output buffer.

            MatchCollection matches = _rtfRegex.Matches(inputRtf);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string word = match.Groups[1].Value;
                    string arg = match.Groups[2].Value;
                    string hex = match.Groups[3].Value;
                    string character = match.Groups[4].Value;
                    string brace = match.Groups[5].Value;
                    string tchar = match.Groups[6].Value;

                    if (!String.IsNullOrEmpty(brace))
                    {
                        curskip = 0;
                        if (brace == "{")
                        {
                            // Push state
                            stack.Push(new StackEntry(ucskip, ignorable));
                        }
                        else if (brace == "}")
                        {
                            // Pop state
                            StackEntry entry = stack.Pop();
                            ucskip = entry.NumberOfCharactersToSkip;
                            ignorable = entry.Ignorable;
                        }
                    }
                    else if (!String.IsNullOrEmpty(character)) // \x (not a letter)
                    {
                        curskip = 0;
                        if (character == "~")
                        {
                            if (!ignorable)
                            {
                                outList.Add("\xA0");
                            }
                        }
                        else if ("{}\\".Contains(character))
                        {
                            if (!ignorable)
                            {
                                outList.Add(character);
                            }
                        }
                        else if (character == "*")
                        {
                            ignorable = true;
                        }
                    }
                    else if (!String.IsNullOrEmpty(word)) // \foo
                    {
                        curskip = 0;
                        if (destinations.Contains(word))
                        {
                            ignorable = true;
                        }
                        else if (ignorable)
                        {
                        }
                        else if (specialCharacters.ContainsKey(word))
                        {
                            outList.Add(specialCharacters[word]);
                        }
                        else if (word == "uc")
                        {
                            ucskip = Int32.Parse(arg);
                        }
                        else if (word == "u")
                        {
                            int c = Int32.Parse(arg);
                            if (c < 0)
                            {
                                c += 0x10000;
                            }
                            outList.Add(Char.ConvertFromUtf32(c));
                            curskip = ucskip;
                        }
                    }
                    else if (!String.IsNullOrEmpty(hex)) // \'xx
                    {
                        if (curskip > 0)
                        {
                            curskip -= 1;
                        }
                        else if (!ignorable)
                        {
                            int c = Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                            outList.Add(Char.ConvertFromUtf32(c));
                        }
                    }
                    else if (!String.IsNullOrEmpty(tchar))
                    {
                        if (curskip > 0)
                        {
                            curskip -= 1;
                        }
                        else if (!ignorable)
                        {
                            outList.Add(tchar);
                        }
                    }
                }
            }
            else
            {
                // Didn't match the regex
                returnString = inputRtf;
            }

            returnString = String.Join(String.Empty, outList.ToArray());

            return returnString;
        }
        public static int[] GetCheckboxListIds(this string str)
        {
            var statuts = new List<int>();
            var tab = !string.IsNullOrEmpty(str) ? str.Split(';') : new string[0]; ;

            if (tab.Any())
            {
                var result = 0;
                foreach (var item in tab)
                {
                    if (int.TryParse(item, out result))
                        statuts.Add(result);
                }
            }

            return statuts.ToArray();
        }
        public static int[] GetTagsListIds(this string str)
        {
            var lst = new List<int>();
            try
            {
                var objs = JsonConvert.DeserializeObject<TagObject[]>(str);
                if (objs.Any())
                    lst.AddRange(objs.Select(m => m.Id).ToArray());
                return lst.ToArray();
            }
            catch (Exception e)
            {
                return new int[0];
            }
        }
        public static IEnumerable<String> EnumByNearestSpace(this String value, int length)
        {
            if (String.IsNullOrEmpty(value))
                yield break;

            int bestDelta = int.MaxValue;
            int bestSplit = -1;

            int from = 0;

            for (int i = 0; i < value.Length; ++i)
            {
                var Ch = value[i];

                if (Ch != ' ')
                    continue;

                int size = (i - from);
                int delta = (size - length > 0) ? size - length : length - size;

                if ((bestSplit < 0) || (delta < bestDelta))
                {
                    bestSplit = i;
                    bestDelta = delta;
                }
                else
                {
                    yield return value.Substring(from, bestSplit - from);

                    i = bestSplit;

                    from = i + 1;
                    bestSplit = -1;
                    bestDelta = int.MaxValue;
                }
            }

            // String's tail
            if (from < value.Length)
            {
                if (bestSplit >= 0)
                {
                    if (bestDelta < value.Length - from)
                        yield return value.Substring(from, bestSplit - from);

                    from = bestSplit + 1;
                }

                if (from < value.Length)
                    yield return value.Substring(from);
            }
        }
    }

    public class TagObject
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
}
