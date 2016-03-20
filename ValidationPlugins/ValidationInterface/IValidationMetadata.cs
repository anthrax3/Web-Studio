namespace ValidationInterface
{
    /// <summary>
    ///     Metadata interface to manage in which order we have to execute the plugins
    /// </summary>
    public interface IValidationMetadata
    {
        /// <summary>
        ///     Plugin name one word only
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Name of the plugin after its execution we have to execute this plugin
        /// </summary>
        string After { get; }
    }
}