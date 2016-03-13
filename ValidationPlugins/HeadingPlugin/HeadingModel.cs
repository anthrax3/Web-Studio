using HtmlAgilityPack;

namespace HeadingPlugin
{
    /// <summary>
    /// Class to save the heading tags
    /// </summary>
    public class HeadingModel
    {
        /// <summary>
        /// Path to file
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// Document 
        /// </summary>
        public HtmlDocument Document { get; set; }

        /// <summary>
        /// Number of h1 tags in the file
        /// </summary>
        public int H1 { get; set; } = 0;

        /// <summary>
        /// Number of h2 tags in the file
        /// </summary>
        public int H2 { get; set; } = 0;

        /// <summary>
        /// Number of h3 tags in the file
        /// </summary>
        public int H3 { get; set; } = 0;

        /// <summary>
        /// Default constructor, it loads the file
        /// </summary>
        /// <param name="filePath"></param>
        public HeadingModel(string filePath)
        {
            File = filePath;
            Document = new HtmlDocument();
            Document.Load(filePath);
        }

        /// <summary>
        /// Check method
        /// </summary>
        public void CheckHeadings()
        {
              CheckH1();
            CheckH2();
            CheckH3();
        }

        /// <summary>
        /// Check the h1 tag
        /// </summary>
        private void CheckH1()
        {
            var nodes = Document.DocumentNode.SelectNodes("//h1");
            if(nodes !=null ) H1 = nodes.Count;
        }

        /// <summary>
        /// Check the h2 tag
        /// </summary>
        private void CheckH2()
        {
            var nodes = Document.DocumentNode.SelectNodes("//h2");
            if (nodes != null) H2 = nodes.Count;
        }

        /// <summary>
        /// Check the h3 tag
        /// </summary>
        private void CheckH3()
        {
            var nodes = Document.DocumentNode.SelectNodes("//h3");
            if (nodes != null) H3 = nodes.Count;
        }
    }
}