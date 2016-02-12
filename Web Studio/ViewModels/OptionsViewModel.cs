using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using Prism.Mvvm;
using Web_Studio.Events;
using Web_Studio.Properties;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Web_Studio.ViewModels
{
    /// <summary>
    /// ViewModel for Option Window
    /// </summary>
    public class OptionsViewModel : BindableBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OptionsViewModel()
        {
            EditorShowLineNumbers = Settings.Default.EditorShowLineNumbers;
            EditorFontSize = Settings.Default.EditorFontSize;
            LocalizeDictionary.Instance.IncludeInvariantCulture = false;
            Languages = LocalizeDictionary.Instance.MergedAvailableCultures;
        }

        private CultureInfo _selectedLanguage;

        /// <summary>
        /// Property to mange app language
        /// </summary>
        public CultureInfo SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {

                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = value;
                SetProperty(ref _selectedLanguage, value);
            }
            
        }

        /// <summary>
        /// Available lenguages
        /// </summary>
        public ObservableCollection<CultureInfo> Languages { get; set; } 

        private bool _editorShowLineNumbers;

        /// <summary>
        /// Enable o disable show line numbers in the editor
        /// </summary>
        public bool EditorShowLineNumbers
        {
            get { return _editorShowLineNumbers; }
            set
            {
                if (value != EditorShowLineNumbers)
                {
                    SetProperty(ref _editorShowLineNumbers, value);
                    Settings.Default.EditorShowLineNumbers = EditorShowLineNumbers;
                    EventSystem.Publish<ShowLineNumbersChanged>(new ShowLineNumbersChanged {ShowLineNumbers = value});
                }
            }
        }

        private int _editorFontSize;

        /// <summary>
        /// Manage font size in the editor
        /// </summary>
        public int EditorFontSize
        {
            get { return _editorFontSize; }
            set
            {
                if (value != EditorFontSize)
                {
                    SetProperty(ref _editorFontSize, value);
                    Settings.Default.EditorFontSize = EditorFontSize;
                    EventSystem.Publish<FontSizeChanged>(new FontSizeChanged { FontSize = value });
                }
            }
        }
    }
}