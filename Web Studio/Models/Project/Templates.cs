using System;

namespace Web_Studio.Models.Project
{
    /// <summary>
    /// Class to manage web templates
    /// </summary>
    public class Templates
    {
        /// <summary>
        /// Simple HTML5 template
        /// </summary>
        /// <returns></returns>
        public static string Html5Template()
        {
            return @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
</head> 
<body>

</body>
</html>";
        }

        /// <summary>
        /// HTML5 template with include tag
        /// </summary>
        /// <returns></returns>
        public static string Html5WithIncludeTemplate()
        {
            return @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
</head> 
<body>
    <include src=""footer.html"" />
</body>
</html>";
        }

        /// <summary>
        /// Footer template
        /// </summary>
        /// <returns></returns>
        public static string FooterTemplate()
        {
            return "<p>Copyright " + DateTime.Today.Year.ToString() + " " + ProjectModel.Instance.Name+@"</p>";
        }
    }
}