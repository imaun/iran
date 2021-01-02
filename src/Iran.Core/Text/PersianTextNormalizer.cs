//Mostly from : https://github.com/VahidN/DNTPersianUtils.Core/tree/master/src/DNTPersianUtils.Core
//with some minor modifications.
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Iran.Core.Text.Internal;

namespace Iran.Core.Text
{
    public static class PersianTextNormalizer {

        #region RegEx constants
        private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(3);

        private static readonly Regex _matchRemoveAllKashida = 
            new Regex("ـ+", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchFixDashes1 =
            new Regex(@"-{3}", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchFixDashes2 =
            new Regex(@"-{2}", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchConvertDotsToEllipsis =
            new Regex(@"\s*\.{3,}", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchApplyHalfSpaceRule1 =
           new Regex(@"\s+(ن?می)\s+", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchApplyHalfSpaceRule2 =
            new Regex(@"\s+(تر(ی(ن)?)?|ها(ی)?)\s+", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchCleanupZwnj =
            new Regex(@"\s+‌|‌\s+", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchYeHeHalfSpace =
            new Regex(@"(\S)(ه[\s‌]+[یی])(\s)", options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _matchAllTags =
           new Regex(@"<(.|\n)*?>", options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
                , matchTimeout: MatchTimeout
#endif
                );

        private static readonly Regex _matchArabicHebrew =
            new Regex(@"[\u0600-\u06FF,\u0590-\u05FF,«,»]", options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
                , matchTimeout: MatchTimeout
#endif
                );

        private static readonly Regex _matchOnlyPersianNumbersRange =
            new Regex(@"^[\u06F0-\u06F9]+$", options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
                , matchTimeout: MatchTimeout
#endif
            );

        private static readonly Regex _matchOnlyPersianLetters =
            new Regex(@"^[\\s,\u06A9\u06AF\u06C0\u06CC\u060C,\u062A\u062B\u062C\u062D\u062E\u062F,\u063A\u064A\u064B\u064C\u064D\u064E,\u064F\u067E\u0670\u0686\u0698\u200C,\u0621-\u0629\u0630-\u0639\u0641-\u0654]+$",
                options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
                , matchTimeout: MatchTimeout
#endif
            );

        internal static readonly Regex _hasHalfSpaces =
                    new Regex(@"\u200B|\u200C|\u200E|\u200F",
                        options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
                        , matchTimeout: MatchTimeout
#endif
                    );
        #endregion

        /// <summary>
        /// Fixes Arabic Ye&Ke with persian Ye&Ke.
        /// </summary>
        /// <param name="text">Text to apply correction.</param>
        /// <returns>Fixed text</returns>
        public static string FixYeKe(this string text) {
            if (text == null)
                return null;

            if (text.IsNullOrEmpty())
                return string.Empty;

            var dataChars = text.ToCharArray();
            for (var i = 0; i < dataChars.Length; i++) {
                switch (dataChars[i]) {
                    case PersianTextConst.ArabicYeChar1:
                    case PersianTextConst.ArabicYeChar2:
                    case PersianTextConst.ArabicYeWithOneDotBelow:
                    case PersianTextConst.ArabicYeWithInvertedV:
                    case PersianTextConst.ArabicYeWithTwoDotsAbove:
                    case PersianTextConst.ArabicYeWithThreeDotsAbove:
                    case PersianTextConst.ArabicYeWithHighHamzeYeh:
                    case PersianTextConst.ArabicYeWithFinalForm:
                    case PersianTextConst.ArabicYeWithThreeDotsBelow:
                    case PersianTextConst.ArabicYeWithTail:
                    case PersianTextConst.ArabicYeSmallV:
                        dataChars[i] = PersianTextConst.PersianYeChar;
                        break;

                    case PersianTextConst.ArabicKeChar:
                        dataChars[i] = PersianTextConst.PersianKeChar;
                        break;

                    default:
                        dataChars[i] = dataChars[i];
                        break;
                }
            }

            return new string(dataChars);
        }

        /// <summary>
        /// Convert Arabic text to UTF-8
        /// </summary>
        public static string ConvertArabic1256ToUtf8(this string text) {
            var latin = Encoding.GetEncoding("ISO-8859-1");
            var bytes = latin.GetBytes(text); // get the bytes for your ANSI string
            var arabic = Encoding.GetEncoding("Windows-1256"); // decode it using the correct encoding
            return arabic.GetString(bytes);
        }

        /// <summary>
        /// Clean UnderLines
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string NormalizeUnderLines(this string text) {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            const char chr1600 = (char)1600; //ـ=1600
            const char chr8204 = (char)8204; //‌=8204

            return text.Replace(chr1600.ToString(), "")
                       .Replace(chr8204.ToString(), "");
        }

        /// <summary>
        /// Removes all kashida
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string NormalizeAllKashida(this string text) {
            return _matchRemoveAllKashida.Replace(text, "").NormalizeUnderLines();
        }

        /// <summary>
        /// Replaces double dash to ndash and triple dash to mdash.
        /// It converts آزمون--- to آزمون—
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string NormalizeDashes(this string text) {
            var phase1 = _matchFixDashes1.Replace(text, @"—");
            var phase2 = _matchFixDashes2.Replace(phase1, @"–");
            return phase2;
        }


        /// <summary>
        /// حذف اعراب از حروف و کلمات
        /// </summary>
        public static string RemoveDiacritics(this string text) {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalizedString = text.Normalize(NormalizationForm.FormKC);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString) {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Replaces three dots with ellipsis.
        /// It converts آزمون.... to آزمون…
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string NormalizeDotsToEllipsis(this string text) {
            return _matchConvertDotsToEllipsis.Replace(text, @"…");
        }

        /// <summary>
        ///  Replaces thin/half spaces with the replacement provided.
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <param name="replacement">The replacement of the thin space. Default value is an empty string.</param>
        /// <returns></returns>
        public static string ReplaceHalfSpaces(this string text, string replacement = "") {
            var result = text;

            if (ContainsHalfSpace(text)) 
                result = _hasHalfSpaces.Replace(text, replacement);
            
            return result;
        }

        /// <summary>
        /// Adds zwnj char between word and prefix/suffix
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string ApplyHalfSpaceRule(this string text) {
            text = text.NormalizeZwnj();

            //put zwnj between word and prefix (mi* nemi*)
            var phase1 = _matchApplyHalfSpaceRule1.Replace(text, @" $1‌");

            //put zwnj between word and suffix (*tar *tarin *ha *haye)
            var phase2 = _matchApplyHalfSpaceRule2.Replace(phase1, @"‌$1 ");

            var phase3 = phase2.NormalizeYeHeHalfSpace();
            return phase3;
        }

        /// <summary>
        /// Removes unnecessary zwnj char that are succeeded/preceded by a space
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string NormalizeZwnj(this string text) 
            => _matchCleanupZwnj.Replace(text, " ");
        
        /// <summary>
        /// Converts ه ی to ه‌ی
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string NormalizeYeHeHalfSpace(this string text) 
            => _matchYeHeHalfSpace.Replace(text, "$1ه‌ی‌$3"); // fix zwnj

        /// <summary>
        /// Check if text contains persian characters.
        /// </summary>
        public static bool ContainsPersian(this string txt) 
            => !txt.IsNullOrEmpty() &&
                _matchArabicHebrew.IsMatch(txt.StripHtmlTags()
                    .Replace(",", ""));
        
        /// <summary>
        /// Check if text cosist only of persian characters.
        /// </summary>
        public static bool ContainsOnlyPersianChars(this string txt) 
            => !txt.IsNullOrEmpty() &&
                   _matchOnlyPersianLetters.IsMatch(txt.StripHtmlTags()
                        .Replace(",", ""));

        /// <summary>
        /// Strip text from Html tags.
        /// </summary>
        public static string StripHtmlTags(this string text) 
            => string.IsNullOrEmpty(text) ?
                        string.Empty :
                        _matchAllTags.Replace(text, " ")
                            .Replace("&nbsp;", " ");

        /// <summary>
        /// Check if text contains only persian numbers. 
        /// </summary>
        public static bool ContainsOnlyPersianNumbers(this string text) 
            => !text.IsNullOrEmpty() &&
                   _matchOnlyPersianNumbersRange.IsMatch(text.StripHtmlTags());

        /// <summary>
        /// Check if text contains half-space.
        /// </summary>
        /// <param name="text">Text to process.</param>
        /// <returns>True if text contains half-space.</returns>
        public static bool ContainsHalfSpace(this string text)
            => _hasHalfSpaces.IsMatch(text);
    }
}
