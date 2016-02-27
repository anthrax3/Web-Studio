using System.Collections.Generic;

namespace W3cPlugin
{
    public class W3cResult
    {
        public List<MessageClass> Messages { get; set; }
    }

    public class MessageClass
    {
        public string Type { get; set; }
        public string Url { get; set; }
        public int LastLine { get; set; }
        public int LastColumn { get; set; }
        public int FirstColumn { get; set; }
        public string Message { get; set; }
        public string Extract { get; set; }
        public int HiliteStart { get; set; }
        public int HiliteLength { get; set; }
        public string SubType { get; set; }
    }
}