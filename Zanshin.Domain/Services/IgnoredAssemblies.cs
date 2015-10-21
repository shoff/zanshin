namespace Zanshin.Domain.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public sealed class IgnoredAssemblies : IEnumerable<Regex>
    {
        private static readonly IgnoredAssemblies ignored = new IgnoredAssemblies();

        private IgnoredAssemblies()
        {
        }

        private static readonly HashSet<Regex> ignoreAssemblies = new HashSet<Regex>
        {
            // TODO read the package list from nuget to get a more complete list.

            new Regex(".*SYSTEM.*",RegexOptions.IgnoreCase), 
            new Regex(".*MICROSOFT.*",RegexOptions.IgnoreCase),
            new Regex(".*MSCORLIB.*",RegexOptions.IgnoreCase),
            new Regex(".*MVCSITEMAP.*",RegexOptions.IgnoreCase),
            new Regex(".*CASTLE.*", RegexOptions.IgnoreCase),
            new Regex(".*NHUNSPELL.*", RegexOptions.IgnoreCase),
            new Regex(".*ICSHARPCODE.*",RegexOptions.IgnoreCase),
            new Regex(".*LUCENE.*", RegexOptions.IgnoreCase),
            new Regex(".*NLOG.*", RegexOptions.IgnoreCase),
            new Regex(".*LOG4NET.*",RegexOptions.IgnoreCase),
            new Regex(".*DYNAMICPROXYGENASSEMBLY.*",RegexOptions.IgnoreCase),
            new Regex(".*CPPCODEPROVIDER.*",RegexOptions.IgnoreCase),
            new Regex(".*ANTLR3.*",RegexOptions.IgnoreCase),
            new Regex(".*CKFINDER.*",RegexOptions.IgnoreCase),
            new Regex(".*DOTNETOPENAUTH.*",RegexOptions.IgnoreCase),
            new Regex(".*ENTITYFRAMEWORK.*",RegexOptions.IgnoreCase),
            new Regex(".*NEWTONSOFT.*", RegexOptions.IgnoreCase),
            new Regex(".*NUNIT.*",RegexOptions.IgnoreCase),
            new Regex(".*WEBGREASE.*",RegexOptions.IgnoreCase),
            new Regex(".*OWIN.*",RegexOptions.IgnoreCase)
        };

        /// <summary>
        /// Determines whether the specified filter is match.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public bool IsMatch(string filter)
        {
            bool result = false;

            foreach (var regex in ignoreAssemblies)
            {
                if (regex.IsMatch(filter))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        public void Add(Regex s)
        {
            ignoreAssemblies.Add(s);
        }

        /// <summary>
        ///   Gets the instance.
        /// </summary>
        /// <value> The instance. </value>
        public static IgnoredAssemblies Instance
        {
            get { return ignored; }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection. </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Regex> GetEnumerator()
        {
            return ignoreAssemblies.GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection. </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}