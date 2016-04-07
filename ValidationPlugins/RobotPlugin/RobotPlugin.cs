using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using RobotPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace RobotPlugin
{
    /// <summary>
    ///     Plugin to check the robot.txt file
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Robot")]
    [ExportMetadata("After", "Links")]
    public class RobotPlugin : IValidation
    {
      

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;
        
        /// <summary>
        ///     Name of the plugin
        /// </summary>
        public string Name => Strings.Name;

        /// <summary>
        ///     Description
        /// </summary>
        public string Description => Strings.Description;

        /// <summary>
        ///     Category of the plugin
        /// </summary>
        public ICategoryType Type { get; } = DevelopmentType.Instance;
        
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
            List<AnalysisResult> AnalysisResults  = new List<AnalysisResult>();
       
            if (!IsEnabled) return AnalysisResults;

            var robotsPath = Path.Combine(projectPath, "robots.txt");
            if (!File.Exists(robotsPath))
            {
                AnalysisResults.Add(new AnalysisResult
                {
                    PluginName = Name,
                    File = "",
                    Line = 0,
                    Type = ErrorType.Instance,
                    Message = Strings.RobotNotFound
                });
            }
            else
            {
                var lines = File.ReadAllLines(robotsPath);
                for (var index = 0; index < lines.Length; index++)
                {
                    var line = lines[index];
                    var match = Regex.Match(line, @"(User-agent: .*)|(Disallow: .*)|(Allow: .*)|(Sitemap: .*)");
                    if (!match.Success)
                    {
                        AnalysisResults.Add(new AnalysisResult
                        {
                            PluginName = Name,
                            File = robotsPath,
                            Line = index + 1,
                            Type = ErrorType.Instance,
                            Message = Strings.BadFormat
                        });
                    }
                }
            }

            return AnalysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;

            var robotsPath = Path.Combine(projectPath, "robots.txt");
            File.WriteAllText(robotsPath, @"User-agent: *
Allow: /");

            List<AnalysisResult> list = new List<AnalysisResult> {RobotsGenerated(robotsPath)};
            return list;
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        /// <summary>
        /// Creates the generated message
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AnalysisResult RobotsGenerated(string file)
        {
          return   new AnalysisResult
          {
              File = file,
              Line = 0,
              PluginName = Name,
              Type = InfoType.Instance,
              Message =  Strings.Generated
          };
        }
    }
}