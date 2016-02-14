namespace SocialCheckInterface
{
    /// <summary>
    /// Interface for social name checking
    /// </summary>
    public interface ISocialCheck
    {
        /// <summary>
        /// Name of the service
        /// </summary>
        string ServiceName { get; set; }
        /// <summary>
        /// Name of the user in the service
        /// </summary>
        string NameInService { get; set; }
        /// <summary>
        /// Available
        /// </summary>
        bool IsAvailable { get; set; }

        /// <summary>
        /// Method to check the availability of a name in a server
        /// </summary>
        /// <param name="name"></param>
        void CheckAvailability(string name);
    }
}
