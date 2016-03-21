using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.IO;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using HumansPlugin.Properties;
using ValidationInterface.MessageTypes;

namespace HumansPlugin
{
    /// <summary>
    /// Plugin to genere humans.txt file
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "Humans")]         
    [ExportMetadata("After", "")]              
    public class HumansPlugin :IValidation  
    {
        /// <summary>
        /// Default constructor inject the vm to the view
        /// </summary>
        public HumansPlugin()
        {
            View = new View(this);
        }

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

        /// <summary>
        /// Text about team in humans
        /// </summary>
        public string TeamText => Strings.TeamText;

        /// <summary>
        /// Text about thanks in humans
        /// </summary>
        public string ThanksText => Strings.ThanksText;

        /// <summary>
        /// Text about technology in humans
        /// </summary>
        public string TechnologyText => Strings.TechnologyText;

        /// <summary>
        /// Team of humans
        /// </summary>
        public string Team { get; set; }

        /// <summary>
        /// Thanks of humans
        /// </summary>
        public string Thanks { get; set; }

        /// <summary>
        /// Tecnology of humans
        /// </summary>
        public string Technology { get; set; }

        #region IValidation
        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl View { get; }

        /// <summary>
        ///     Name of the plugin
        /// </summary>
        public string Name  => Strings.Name;

        /// <summary>
        ///     Description
        /// </summary>
        public string Description => Strings.Description;

        /// <summary>
        ///     Category of the plugin
        /// </summary>
        public ICategoryType Type { get; } = DevelopmentType.Instance;

        /// <summary>
        ///     Results of the check method.
        /// </summary>
        public List<AnalysisResult> AnalysisResults { get; } = new List<AnalysisResult>();

        /// <summary>
        ///     can we automatically fix some errors?
        /// </summary>
        public bool IsAutoFixeable { get; set; } = false;

        /// <summary>
        ///     Is enabled this plugin
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        ///     Method to validate the project with this plugin
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public List<AnalysisResult> Check(string projectPath)
        {
            AnalysisResults.Clear();
            if (!IsEnabled) return AnalysisResults;

            var humansPath = Path.Combine(projectPath, "humans.txt");

            if (!File.Exists(humansPath))
            {
                AnalysisResults.Add(new AnalysisResult
                {
                    PluginName = Name,
                    File = humansPath,
                    Line = 0,
                    Type = ErrorType.Instance,
                    Message = Strings.NotFound
                });
            }

            if (string.IsNullOrWhiteSpace(Team) || string.IsNullOrWhiteSpace(Thanks) ||
                string.IsNullOrWhiteSpace(Technology))
            {
                AnalysisResults.Add(new AnalysisResult
                {
                    PluginName = Name,
                    File = "",
                    Line = 0,
                    Type = WarningType.Instance,
                    Message = Strings.DataNeeded
                });
            }

            return AnalysisResults;

        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable) return null;
            var humansPath = Path.Combine(projectPath, "humans.txt");
            StringBuilder content = new StringBuilder();
            content.AppendLine("# Humans.txt file see more in http://humanstxt.org");
            content.AppendLine("\n# TEAM");
            content.AppendLine(Team);
            content.AppendLine("\n# THANKS");
            content.AppendLine(Thanks);
            content.AppendLine("\n# TECHNOLOGY");
            content.AppendLine(Technology);

            File.WriteAllText(humansPath,content.ToString());

            return null;
        }
        #endregion
    }
}
