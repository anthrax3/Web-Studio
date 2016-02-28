namespace ValidationInterface.MessageTypes
{
    /// <summary>
    ///     Type of the information message
    /// </summary>
    public class InfoType : IMessageType
    {
        /// <summary>
        ///     Name of the information message
        /// </summary>
        public string Name { get; } = "Information";

        /// <summary>
        ///     Color of the information message (blue)
        /// </summary>
        public string HexColor { get; } = "#0099ff";

        /// <summary>
        ///     Symbol (i) of the information message
        /// </summary>
        public string Symbol { get; } = "";

        /// <summary>
        /// Sigleton pattern
        /// </summary>
        public static IMessageType Instance { get; } = new InfoType();
    }
}