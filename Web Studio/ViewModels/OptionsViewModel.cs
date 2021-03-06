﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Prism.Mvvm;
using Web_Studio.Events;
using Web_Studio.Properties;
using WPFLocalizeExtension.Engine;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     ViewModel for Option Window
    /// </summary>
    public class OptionsViewModel : BindableBase
    {
        private int _editorFontSize;

        private bool _editorShowLineNumbers;

        private CultureInfo _selectedLanguage;

        /// <summary>
        ///     Default constructor
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OptionsViewModel()
        {
            EditorShowLineNumbers = Settings.Default.EditorShowLineNumbers;
            EditorFontSize = Settings.Default.EditorFontSize;

            Languages = new ObservableCollection<CultureInfo>(LocalizeDictionary.Instance.MergedAvailableCultures);
            Languages.Remove(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Property to mange app language
        /// </summary>
        public CultureInfo SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if(value!=null)
                    Localization.Localization.ChangeLanguage(value);
                SetProperty(ref _selectedLanguage, value);
            }
        }

        /// <summary>
        ///     Available lenguages
        /// </summary>
        public ObservableCollection<CultureInfo> Languages { get; set; }

        /// <summary>
        ///     Enable o disable show line numbers in the editor
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
                    EventSystem.Publish(new ShowLineNumbersEvent {ShowLineNumbers = value});
                }
            }
        }

        /// <summary>
        ///     Manage font size in the editor
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
                    EventSystem.Publish(new FontSizeChangedEvent {FontSize = value});
                }
            }
        }
    }
}