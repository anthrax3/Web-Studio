using Web_Studio.Editor;

namespace Web_Studio.Events
{
    /// <summary>
    /// Manage the document closed event. Raised in a EditorView and caught in MainWindowViewModel 
    /// </summary>
    public class ClosedDocumentEvent
    {
        /// <summary>
        /// The document
        /// </summary>
        public EditorViewModel ClosedDocument { get; set; }
    }
}