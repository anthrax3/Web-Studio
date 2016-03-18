namespace Web_Studio.Events
{
    /// <summary>
    ///    Event raised when changed the visibility of message container
    /// </summary>
    public class MessageContainerVisibilityChangedEvent
    {

        /// <summary>
        /// Visibility
        /// </summary>
        public bool IsVisible { get; set; } 
    }
}