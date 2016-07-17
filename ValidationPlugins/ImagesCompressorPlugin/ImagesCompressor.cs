using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using ImageMagick;
using ImagesCompressorPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace ImagesCompressorPlugin
{
    /// <summary>
    ///     Plugin to manage the image compressor
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "ImagesCompressor")]
    [ExportMetadata("After", "Include")]
    public class ImagesCompressor : IValidation
    {
        private readonly object _myLock = new object();

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

        #region IValidation
      
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
        public ICategoryType Type { get; } = OptimizationType.Instance;
        
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
            List<AnalysisResult> analysisResults  = new List<AnalysisResult>();
            analysisResults.Clear();
            if (!IsEnabled) return analysisResults;

            if (!IsAutoFixeable)
                analysisResults.Add(new AnalysisResult("", 0, Name, Strings.NotEnabled, WarningType.Instance));
            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;
            //Compress png
            var filesToCheck = Directory.EnumerateFiles(projectPath, "*.png", SearchOption.AllDirectories);
            ulong originalSize = 0, afterSize = 0;

            Parallel.ForEach(filesToCheck, file =>
            {
                var myFile = new FileInfo(file);
                lock (_myLock)
                {
                    //Only one thread can enter
                    originalSize += (ulong) myFile.Length;
                }
                using (var image = new MagickImage(file))
                {
                    image.Quality = 82;
                    image.FilterType = FilterType.Triangle;
                    image.Posterize(136);
                    image.Settings.SetDefine(MagickFormat.Jpg, "fancy-upsampling", "off");
                    image.Settings.SetDefine(MagickFormat.Png, "compression-filter", "5");
                    image.Settings.SetDefine(MagickFormat.Png, "compression-level", "9");
                    image.Settings.SetDefine(MagickFormat.Png, "compression-strategy", "1");
                    image.Settings.SetDefine(MagickFormat.Png, "exclude-chunk", "all");
                    image.Interlace = Interlace.NoInterlace;
                    image.ColorSpace = ColorSpace.sRGB;
                    image.Strip();
                    image.Write(file);
                }
                myFile.Refresh();
                lock (_myLock) //Only one thread can enter
                {
                    afterSize += (ulong) myFile.Length;
                }
            });

            return new List<AnalysisResult>
            {
                new AnalysisResult("", 0, Name,
                    string.Format(Strings.CompressionRate, originalSize/(1024.0*1024), afterSize/(1024.0*1024)),
                    InfoType.Instance)
            };
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        #endregion
    }
}