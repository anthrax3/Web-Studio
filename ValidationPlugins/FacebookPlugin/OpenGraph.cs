using System.Collections.Generic;
using FacebookPlugin.Properties;
using HtmlAgilityPack;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace FacebookPlugin
{
    /// <summary>
    ///     Class to manage the OpenGraph protocol
    /// </summary>
    public class OpenGraph
    {
        private readonly HtmlDocument _document;
        private readonly string _domain;
        private readonly string _file;
        private HtmlNode _headNode;
        private readonly string _projectPath;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public OpenGraph(string file, string domain, string projectPath)
        {
            _file = file;
            _domain = domain;
            _projectPath = projectPath;
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
            result = UrlNode();
            if (result != null) list.Add(result);
            _document.Save(_file);
            return list;
        }


        private AnalysisResult TitleNode()
        {
            var titleTag = _document.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
            if (titleTag != null)
            {
                var title = _document.DocumentNode.SelectSingleNode("//title");
                if (title == null)
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.TitleNotFound, ErrorType.Instance);
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("property", "og:title");
                metaTag.Attributes.Add("content", title.InnerText);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult TypeNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@property='og:type']");
            if (tempTag != null)
            {
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("property", "og:type");
                metaTag.Attributes.Add("content", "article");
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult DescriptionNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
            if (tempTag != null)
            {
                var description = _document.DocumentNode.SelectSingleNode("//meta[@name='description']");
                var value = description?.GetAttributeValue("content", null);
                if (value == null)
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.DescriptionNotFound, ErrorType.Instance);
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("property", "og:description");
                metaTag.Attributes.Add("content", value);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult ImageNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            if (tempTag != null)
            {
                var image = _document.DocumentNode.SelectSingleNode("//img");
                var value = image?.GetAttributeValue("src", null);
                if (value == null)
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.ImgNotFound, WarningType.Instance);
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("property", "og:image");
                metaTag.Attributes.Add("content", value);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }

        private AnalysisResult UrlNode()
        {
            var tempTag = _document.DocumentNode.SelectSingleNode("//meta[@property='og:url']");
            if (tempTag != null)
            {
                if (string.IsNullOrWhiteSpace(_domain))
                    return new AnalysisResult(_file, 0, Strings.Name, Strings.DomainMalformated, ErrorType.Instance);
                var relativeUrl = _file.Replace(_projectPath, string.Empty).Replace(@"\", @"/");
                //change file separator
                var metaTag = _document.CreateElement("meta");
                metaTag.Attributes.Add("property", "og:url");
                metaTag.Attributes.Add("content", _domain + relativeUrl);
                _headNode.AppendChild(metaTag);
            }
            return null;
        }
    }
}