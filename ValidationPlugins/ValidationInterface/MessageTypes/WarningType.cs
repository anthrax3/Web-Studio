namespace ValidationInterface.MessageTypes
{
    /// <summary>
    ///     Warning message type
    /// </summary>
    public class WarningType : IMessageType
    {
        /// <summary>
        ///     Sigleton pattern
        /// </summary>
        public static IMessageType Instance { get; } = new WarningType();

        /// <summary>
        ///     Name of the warning type
        /// </summary>
        public string Name => Strings.Warning;

        /// <summary>
        ///     Color of the warning type (orange)
        /// </summary>
        public string HexColor { get; } = "#ff8000";

        /// <summary>
        ///     Symbol (!) of the warning type
        /// </summary>
        public string Symbol { get; } = "";
    }
}