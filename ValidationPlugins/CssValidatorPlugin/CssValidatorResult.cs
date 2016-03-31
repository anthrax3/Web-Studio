using System.Collections.Generic;

namespace CssValidatorPlugin
{
#pragma warning disable 1591
    public class CssValidatorResult

    {
        public Cssvalidation cssvalidation { get; set; }
    }

    public class Result
    {
        public int errorcount { get; set; }
        public int warningcount { get; set; }
    }

    public class Error
    {
        public string source { get; set; }
        public int line { get; set; }
        public string context { get; set; }
        public string type { get; set; }
        public string message { get; set; }
    }

    public class Warning
    {
        public string source { get; set; }
        public int line { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public int level { get; set; }
    }

    public class Cssvalidation
    {
        public string uri { get; set; }
        public string checkedby { get; set; }
        public string csslevel { get; set; }
        public string date { get; set; }
        public string timestamp { get; set; }
        public bool validity { get; set; }
        public Result result { get; set; }
        public List<Error> errors { get; set; }
        public List<Warning> warnings { get; set; }
    }
#pragma warning restore 1591

}