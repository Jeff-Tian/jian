using System.Collections.Generic;

namespace Jian
{
    public class JianEnt
    {
        public List<Page> Pages { get; set; }
    }

    public class Page
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public Dictionary<string, PatternValuePair> Interests { get; set; }
    }

    public class PatternValuePair
    {
        public string Pattern { get; set; }
        public string Value { get; set; }
    }
}