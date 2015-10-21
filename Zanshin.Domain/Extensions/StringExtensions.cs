namespace Zanshin.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using Zanshin.Domain.Exceptions;

    public static class StringExtensions
    {

        private const string ScriptIncludeFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
        private const string LinkIncludeFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        private static readonly ASCIIEncoding encoding = new ASCIIEncoding();

        private static readonly Regex validEmailRegexOne = new Regex(@"([a-zA-Z0-9_\.]+)/PDCO/PDCO@PDCO",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex validEmailRegexTwo =
            new Regex(
                @"^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly List<Rule> plurals = new List<Rule>();
        private static readonly List<Rule> singulars = new List<Rule>();
        private static readonly List<string> uncountables = new List<string>();
        private static readonly Regex usOne = new Regex("([A-Z]+)([A-Z][a-z])", RegexOptions.Compiled);
        private static readonly Regex usTwo = new Regex(@"([a-z\d])([A-Z])", RegexOptions.Compiled);
        private static readonly Regex usThree = new Regex(@"[-\s]", RegexOptions.Compiled);

        static StringExtensions()
        {
            AddPlural("$", "s");
            AddPlural("s$", "s");
            AddPlural("(ax|test)is$", "$1es");
            AddPlural("(octop|vir)us$", "$1i");
            AddPlural("(alias|status)$", "$1es");
            AddPlural("(bu)s$", "$1ses");
            AddPlural("(buffal|tomat)o$", "$1oes");
            AddPlural("([ti])um$", "$1a");
            AddPlural("sis$", "ses");
            AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
            AddPlural("(hive)$", "$1s");
            AddPlural("([^aeiouy]|qu)y$", "$1ies");
            AddPlural("(x|ch|ss|sh)$", "$1es");
            AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
            AddPlural("([m|l])ouse$", "$1ice");
            AddPlural("^(ox)$", "$1en");
            AddPlural("(quiz)$", "$1zes");
            AddSingular("s$", "");
            AddSingular("(n)ews$", "$1ews");
            AddSingular("([ti])a$", "$1um");
            AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
            AddSingular("(^analy)ses$", "$1sis");
            AddSingular("([^f])ves$", "$1fe");
            AddSingular("(hive)s$", "$1");
            AddSingular("(tive)s$", "$1");
            AddSingular("([lr])ves$", "$1f");
            AddSingular("([^aeiouy]|qu)ies$", "$1y");
            AddSingular("(s)eries$", "$1eries");
            AddSingular("(m)ovies$", "$1ovie");
            AddSingular("(x|ch|ss|sh)es$", "$1");
            AddSingular("([m|l])ice$", "$1ouse");
            AddSingular("(bus)es$", "$1");
            AddSingular("(o)es$", "$1");
            AddSingular("(shoe)s$", "$1");
            AddSingular("(cris|ax|test)es$", "$1is");
            AddSingular("(octop|vir)i$", "$1us");
            AddSingular("(alias|status)es$", "$1");
            AddSingular("^(ox)en", "$1");
            AddSingular("(vert|ind)ices$", "$1ex");
            AddSingular("(matr)ices$", "$1ix");
            AddSingular("(quiz)zes$", "$1");
            AddIrregular("person", "people");
            AddIrregular("man", "men");
            AddIrregular("child", "children");
            AddIrregular("sex", "sexes");
            AddIrregular("move", "moves");
            AddUncountable("equipment");
            AddUncountable("information");
            AddUncountable("rice");
            AddUncountable("money");
            AddUncountable("species");
            AddUncountable("series");
            AddUncountable("fish");
            AddUncountable("sheep");
        }

        private static void AddIrregular(string singular, string plural)
        {
            AddPlural(string.Concat(new object[]
            {
                "(", singular[0], ")", singular.Substring(1), "$"
            }), "$1" + plural.Substring(1));
            AddSingular(string.Concat(new object[]
            {
                "(", plural[0], ")", plural.Substring(1), "$"
            }), "$1" + singular.Substring(1));
        }

        private static void AddPlural(string rule, string replacement)
        {
            plurals.Add(new Rule(rule, replacement));
        }

        private static void AddSingular(string rule, string replacement)
        {
            singulars.Add(new Rule(rule, replacement));
        }

        private static void AddUncountable(string word)
        {
            uncountables.Add(word.ToLower());
        }

        private static string ApplyRules(List<Rule> rules, string word)
        {
            string str = word;
            if (!uncountables.Contains(word.ToLower()))
            {
                for (int i = rules.Count - 1; i >= 0; i--)
                {
                    str = rules[i].Apply(word);
                    if (str != null)
                    {
                        return str;
                    }
                }
            }
            return str;
        }

        public static string ToCamel(this string lowercaseAndUnderscoredWord)
        {
            return Lowercase(MakePascal(lowercaseAndUnderscoredWord));
        }

        public static string Capitalize(this string word)
        {
            return (word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower());
        }

        public static string AddDashes(this string underscoredWord)
        {
            return underscoredWord.Replace('_', '-');
        }

        public static string Humanize(this string lowercaseAndUnderscoredWord)
        {
            return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, "_", " "));
        }

        public static string ToOrdinal(this string number)
        {
            int num = int.Parse(number);
            int num2 = num % 100;
            if ((num2 < 11) || (num2 > 13))
            {
                switch ((num % 10))
                {
                    case 1:
                        return (number + "st");

                    case 2:
                        return (number + "nd");

                    case 3:
                        return (number + "rd");
                }
            }
            return (number + "th");
        }

        public static string MakePascal(this string lowercaseAndUnderscoredWord)
        {
            return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
        }

        public static string Pluralize(this string word)
        {
            return ApplyRules(plurals, word);
        }

        public static string ToSingular(this string word)
        {
            return ApplyRules(singulars, word);
        }

        public static string ToTitle(this string word)
        {
            return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])", match => match.Captures[0].Value.ToUpper());
        }

        public static string Lowercase(this string word)
        {
            return (word.Substring(0, 1).ToLower() + word.Substring(1));
        }

        public static string Underscore(this string pascalCasedWord)
        {
            return usThree.Replace(usTwo.Replace(usOne.Replace(pascalCasedWord, "$1_$2"), "$1_$2"), "_").ToLower();
        }

        /// <summary>
        ///   Converts a "Y" or "N" string to its boolean value.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <returns> </returns>
        public static bool ConvertYorNToBool(this string s)
        {
            if ((!string.IsNullOrEmpty(s)) && (s.Length == 1))
            {
                return s.ToUpperInvariant() == "Y";
            }

            return false;
        }

        /// <summary>
        ///   Capitalizes the first letter of each word in a string that are separated by spaces or a string that is a single word.
        /// </summary>
        /// <param name="fixedString"> The string value </param>
        /// <returns> </returns>
        public static string CapitalizeFirstLetterOfEachWord(this string fixedString)
        {
            if (string.IsNullOrEmpty(fixedString))
            {
                return fixedString;
            }

            // make sure all the letters are lower case to start with
            string lowerString = fixedString.ToLowerInvariant();

            // now remove any spaces in the string by putting each 
            // group of concurrent chars into a container in a string array
            string[] stringArray = lowerString.Split(' ');
            int arrayLength = stringArray.Length;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < arrayLength; i++)
            {
                char[] charArray = stringArray[i].ToCharArray();

                if (charArray.Length == 0) // sent whitespace or garbage we can't read
                {
                    continue;
                }

                sb.Append(Char.IsLetter(charArray[0]) ? char.ToUpperInvariant(charArray[0]) : charArray[0]);

                if (stringArray[i].Length > 1)
                {
                    sb.Append(stringArray[i].Substring(1));
                }

                // only add back in the spaces between the words, not after the last word.
                if (arrayLength > i)
                {
                    sb.Append(' ');
                }
            }

            // trim any whitespace off
            return sb.ToString().Trim();
        }

        private static readonly char[] removeFromStart =
        {
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '[', ']', '{', '}', ':', '?', '<', '>', '\\'
            , '/', '_', '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Uppercases the first.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.TrimStart(removeFromStart).ToCharArray();

            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }

        /// <summary>
        ///   Determines whether a string ends with any of the variables in a given string array.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <param name="stringsToCheck"> The strings to check. </param>
        /// <param name="caseSensitive"> if set to <c>true</c> [case sensitive]. </param>
        /// <param name="cultureInfo"> The culture info. </param>
        /// <returns> </returns>
        public static bool EndsWith(this string s, string[] stringsToCheck, bool caseSensitive, CultureInfo cultureInfo)
        {
            if (stringsToCheck == null)
            {
                throw new ParameterNullException("stringsToCheck");
            }

            if ((string.IsNullOrEmpty(s)) || (stringsToCheck.Length == 0))
            {
                return false;
            }

            bool result = false;

            foreach (var item in stringsToCheck)
            {
                // did they pass us an empty or null filter?
                // if so, protect the calling code.
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                if (s.EndsWith(item, caseSensitive, cultureInfo))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        ///   Ensures the dot after.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <returns> </returns>
        public static string EnsureDotAfter(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.EndsWith("."))
            {
                return s;
            }
            return s + ".";
        }

        /// <summary>
        ///   Ensures the dot before.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <returns> </returns>
        public static string EnsureDotBefore(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.StartsWith("."))
            {
                return s;
            }
            return "." + s;
        }

        /// <summary>
        ///   Ensures the slashes after.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <returns> </returns>
        public static string EnsureSlashesAfter(this string s)
        {
            if (s == null)
            {
                return null;
            }

            if (s.EndsWith("\\"))
            {
                return s;
            }
            return s + "\\";
        }

        /// <summary>
        ///   Ensures the slashes before.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <returns> </returns>
        public static string EnsureSlashesBefore(this string s)
        {
            if (s == null)
            {
                return null;
            }

            if (s.StartsWith("\\"))
            {
                return s;
            }
            return "\\" + s;
        }

        /// <summary>
        ///   Finds the config path.
        /// </summary>
        /// <param name="fileName"> The name of the file.(i.e. notes.txt, index.html etc.) </param>
        /// <param name="prependPath"> if set to <c>true</c> [prepend path]. </param>
        /// <returns> </returns>
        public static string FindPath(this string fileName, bool prependPath)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ParameterNullException("fileName");
            }

            string configName = fileName.EnsureSlashesBefore();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string path = string.Empty;

            // first see if a file exists in the given path
            // this 
            if (File.Exists(fileName))
            {
                // yes we really do want to do an assignment here, and not just
                // return the string.
                if (prependPath)
                {
                    FileInfo fi = new FileInfo(fileName);
                    path = fi.FullName;
                }
                else
                {
                    path = fileName;
                }
            }
            // try all the usual suspects.
            else if (File.Exists(basePath + configName))
            {
                path = basePath + configName;
            }
            // try adding \\bin
            else if (File.Exists(basePath + "\\bin" + configName))
            {
                path = basePath + "\\bin" + configName;
            }
            // try adding \\bin\\Debug
            else if (File.Exists(basePath + "\\bin\\Debug" + configName))
            {
                path = basePath + "\\bin\\Debug" + configName;
            }
            // try release
            else if (File.Exists(basePath + "\\bin\\Release" + configName))
            {
                path = basePath + "\\bin\\Release" + configName;
            }
            // try App_Data
            else if (File.Exists(basePath + "\\App_Data" + configName))
            {
                path = basePath + "\\App_Data" + configName;
            }
            else if (File.Exists(basePath + "\\bin\\App_Data" + configName))
            {
                path = basePath + "\\bin\\App_Data" + configName;
            }
            else if (File.Exists(basePath + "\\bin\\Debug\\App_Data" + configName))
            {
                path = basePath + "\\bin\\Debug\\App_Data" + configName;
            }
            else if (File.Exists(basePath + "\\bin\\Release\\App_Data" + configName))
            {
                path = basePath + "\\bin\\Release\\App_Data" + configName;
            }
            else if (File.Exists(configName))
            {
                path = configName;
            }
            return path;
        }

        /// <summary>
        ///   Finds the full file path.
        /// </summary>
        /// <param name="fileName"> The name of the file.(i.e. notes.txt, index.html etc.) </param>
        /// <returns> </returns>
        public static string FindPath(this string fileName)
        {
            return fileName.FindPath(false);
        }

        /// <summary>
        ///   Converts a base 64 string to its 8bit ASCII equivalent
        /// </summary>
        /// <param name="messageToConvert"> The Base 64 string to convert. </param>
        /// <returns> </returns>
        public static string FromBase64(this string messageToConvert)
        {
            if (messageToConvert == null)
            {
                return null;
            }

            messageToConvert = messageToConvert.Replace("-", "+").Replace("_", "/");
            var bytes = Convert.FromBase64String(messageToConvert);
            return encoding.GetString(bytes);
        }

        /// <summary>
        ///   Determines whether [is valid email address] [the specified string to check].
        /// </summary>
        /// <param name="stringToCheck"> The s. </param>
        /// <returns> <c>true</c> if the specified string to check is a valid email address, otherwise <c>false</c> . </returns>
        public static bool IsValidEmailAddress(this string stringToCheck)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                return false;
            }

            bool result = validEmailRegexTwo.IsMatch(stringToCheck);

            // make sure this isn't a silly lotus notes address
            if (!result)
            {
                result = validEmailRegexOne.IsMatch(stringToCheck);
            }
            return result;
        }

        /// <summary>
        ///   Returns an MD5 hash of the given string.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <returns> </returns>
        /// <remarks>
        ///   New code should NOT use this method, it is for backwards compatibility only! MD5 is no longer considered a secure hashing algorithm
        /// </remarks>
        [Obsolete("If possible, use the Sha256Hash method instead.")]
        public static string Md5Hash(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            MD5 md5 = MD5.Create();
            byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(s));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataMd5.Length; i++)
            {
                sb.AppendFormat("{0:x2}", dataMd5[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   Converts the contents of a StringBuilder to an MD5 Hash and returns the string value.
        /// </summary>
        /// <param name="sb"> The StringBuilder. </param>
        /// <returns> </returns>
        [Obsolete("If possible, use the Sha256Hash method instead.")]
        public static string Md5Hash(this StringBuilder sb)
        {
            return sb == null ? null : sb.ToString().Md5Hash();
        }

        /// <summary>
        ///   Replaces the specified s.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <param name="remove"> The remove. </param>
        /// <param name="replacement"> The replacement. </param>
        /// <returns> </returns>
        public static string Replace(this string s, string[] remove, string replacement)
        {
            if ((string.IsNullOrEmpty(s)) || ((remove == null) || (remove.Length == 0)))
            {
                return s;
            }

            List<string> removes = new List<string>();
            for (int i = 0; i < remove.Length; i++)
            {
                if (!string.IsNullOrEmpty(remove[i]))
                {
                    removes.Add(remove[i]);
                }
            }
            // make sure we have something to remove.
            if (removes.Count == 0)
            {
                return s;
            }

            if (replacement == null)
            {
                replacement = string.Empty;
            }

            Func<string, string> f = x => x;

            // now we iterate through the cleaned up collection
            // of items we want to remove and execute our func
            removes.Each(strings =>
            {
                var s0 = strings;

                // make sure s0 contains a value
                if (!string.IsNullOrEmpty(s0))
                {
                    f = f.On(sr => sr.Replace(s0, replacement));
                }
            });

            return f(s);
        }

        /// <summary>
        ///   Creates a one way hash for string passed using the SHA256 hashing algorithm.
        /// </summary>
        /// <param name="s"> The string to hash. </param>
        /// <returns> </returns>
        public static string Sha256Hash(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            SHA256 sha256 = SHA256.Create();

            byte[] dataSha256 = sha256.ComputeHash(Encoding.Default.GetBytes(s));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataSha256.Length; i++)
            {
                sb.AppendFormat("{0:x2}", dataSha256[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   Determines whether a string starts with any of the variables in a given string array.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <param name="stringsToCheck"> The strings to check. </param>
        /// <param name="caseSensitive"> if set to <c>true</c> [case sensitive]. </param>
        /// <param name="cultureInfo"> The culture info. </param>
        /// <returns> </returns>
        public static bool StartsWith(this string s, string[] stringsToCheck, bool caseSensitive, CultureInfo cultureInfo)
        {
            if (stringsToCheck == null)
            {
                throw new ParameterNullException("stringsToCheck");
            }

            if ((string.IsNullOrEmpty(s)) || (stringsToCheck.Length == 0))
            {
                return false;
            }

            bool result = false;

            for (int i = 0; i < stringsToCheck.Length; i++)
            {
                if (string.IsNullOrEmpty(stringsToCheck[i]))
                {
                    continue;
                }

                if (s.StartsWith(stringsToCheck[i], caseSensitive, cultureInfo))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        ///   Converts a string to base 64
        /// </summary>
        /// <param name="messageToEncode"> The encode. </param>
        /// <returns> </returns>
        public static string ToBase64(this string messageToEncode)
        {
            if (messageToEncode == null)
            {
                return null;
            }
            byte[] bytes = encoding.GetBytes(messageToEncode);
            string result = Convert.ToBase64String(bytes, 0, bytes.Length);
            result = result.Replace("+", "-").Replace("/", "_");
            return result;
        }

        /// <summary>
        ///   URLs the decode.
        /// </summary>
        /// <param name="decode"> The decode. </param>
        /// <returns> </returns>
        public static string UrlDecode(this string decode)
        {
            if (string.IsNullOrEmpty(decode))
            {
                return decode;
            }
            return decode.StartsWith("=") ? FromBase64(decode.TrimStart('=')) : decode.Replace("+", " ");
        }

        /// <summary>
        ///   Encodes the given string.
        /// </summary>
        /// <param name="encode"> The encode. </param>
        /// <returns> </returns>
        public static string UrlEncode(this string encode)
        {
            if (string.IsNullOrEmpty(encode))
            {
                return encode;
            }
            string encoded = HttpServerUtility.UrlTokenEncode(encoding.GetBytes(encode));
            if (encoded.Replace("+", string.Empty) == encode.Replace(" ", string.Empty))
            {
                return encoded;
            }
            return "=" + ToBase64(encode);
        }

        /// <summary>
        /// Creates a html script tag setting the src to the absolute path given the virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">virtualPath</exception>
        public static string IncludeScript(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new ParameterNullException("virtualPath");
            }
            string clientPath = VirtualPathUtility.ToAbsolute(virtualPath);
            return string.Format(ScriptIncludeFormat, clientPath);
        }

        /// <summary>
        /// Creates a html link style tag setting the href to the absolute path given the virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">virtualPath</exception>
        public static string IncludeLinkStyle(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new ParameterNullException("virtualPath");
            }
            string clientPath = VirtualPathUtility.ToAbsolute(virtualPath);
            return string.Format(LinkIncludeFormat, clientPath);
        }

        /// <summary>
        /// Sets the absolute path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">virtualPath</exception>
        public static string SetAbsolutePath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new ParameterNullException("virtualPath");
            }
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        /// <summary>
        /// To the virtual path.
        /// </summary>
        /// <param name="physicalPath">The physical path.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Cannot convert string to relative without a valid HttpContext.</exception>
        public static string ToVirtualPath(this string physicalPath)
        {
            if ((ReferenceEquals(null, HttpContext.Current)) || (ReferenceEquals(null, HttpContext.Current.Request))
                || (ReferenceEquals(null, HttpContext.Current.Request.PhysicalApplicationPath)))
            {
                throw new ApplicationException("Cannot convert string to relative without a valid HttpContext.");
            }
            return "~/" + physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length).Replace("\\", "/");
        }

        private sealed class Rule
        {
            private readonly Regex regex;
            private readonly string replacement;

            /// <summary>
            ///   Initializes a new instance of the <see cref="Rule" /> class.
            /// </summary>
            /// <param name="pattern"> The pattern. </param>
            /// <param name="replacement"> The replacement. </param>
            public Rule(string pattern, string replacement)
            {
                this.regex = new Regex(pattern, RegexOptions.IgnoreCase);
                this.replacement = replacement;
            }

            /// <summary>
            ///   Applies the specified word.
            /// </summary>
            /// <param name="word"> The word. </param>
            /// <returns> </returns>
            public string Apply(string word)
            {
                if (!this.regex.IsMatch(word))
                {
                    return null;
                }
                return this.regex.Replace(word, this.replacement);
            }
        }
    }
}