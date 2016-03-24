using System.Collections.Generic;
using HtmlAgilityPack;
using TwitterPlugin.Properties;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace TwitterPlugin
{
    /// <summary>
    /// Class to manage the twitter metadata     https://dev.twitter.com/cards/types/summary
    /// </summary>
    public class TwitterMetadata
    {
        private readonly HtmlDocument _document;
        private HtmlNode _headNode;
        private string _file;
        private string _site;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="file"></param>
        /// <param name="site"></param>
        public TwitterMetadata(string file,string site)
        {
            _file = file;
            _site = site;
            _document = new HtmlDocument();
            _document.OptionWriteEmptyNodes = true; //Close tags
            _document.Load(file);
        }

        /// <summary>
        ///     Add all Open Graph tags
        /// </summary>
        /// <returns></returns>
        public List<AnalysisResult> AddTags()
        {
            _headNode = _document.DocumentNode.SelectSingleNode("//head");
            var list = new List<AnalysisResult>();
            if (_headNode == null)
            {
                list.Add(new AnalysisResult(_file, 0, Strings.Name, Strings.HeadNotFound, ErrorType.Instance));
                return list;
            }
            var result = TitleNode();
            if (result != null) list.Add(result);
            result = TypeNode();
            if (result != null) list.Add(result);
            result = DescriptionNode();
            if (result != null) list.Add(result);
            result = ImageNode();
            if (result != null) list.Add(result);
            result = SiteNode();
            if (result != null) list.Add(result);
            _document.Save(_file);
            return list;
        }

        private AnalysisResult SiteNode()
        {  
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@name='twitter:site']");
            if (tempTag == null) //Add tag
            {
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("name", "twitter:site");
                metaTag.Attributes.Add("content",_site );
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult TypeNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@name='twitter:card']");
            if (tempTag == null) //Add tag
            {
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("name", "twitter:card");
                metaTag.Attributes.Add("content", "summary");
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult TitleNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@name='twitter:title']");
            if (tempTag == null) //Add tag
            {
                var title = _document.DocumentNode.SelectSingleNode("//title");
                if (title == null)
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.TitleNotFound, ErrorType.Instance);
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("name", "twitter:title");
                metaTag.Attributes.Add("content", title.InnerText);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult DescriptionNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@name='twitter:description']");
            if (tempTag == null) //Add tag
            {
                var description = _document.DocumentNode.SelectSingleNode("//meta[@name='description']");
                var value = description?.GetAttributeValue("content", null);
                if (value == null)
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.DescriptionNotFound, ErrorType.Instance);
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("name", "twitter:description");
                metaTag.Attributes.Add("content", value);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult ImageNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@name='twitter:image']");
            if (tempTag == null) //Add tag
            {
                var image = _document.DocumentNode.SelectSingleNode("//img");
                var value = image?.GetAttributeValue("src", null);
                if (value == null)
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.ImgNotFound, WarningType.Instance);
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("name", "twitter:image");
                metaTag.Attributes.Add("content", value);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }
    }
}