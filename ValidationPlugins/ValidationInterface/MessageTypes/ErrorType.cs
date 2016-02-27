namespace ValidationInterface.MessageTypes
{
    /// <summary>
    ///     Error message
    /// </summary>
    public  class ErrorType : IMessageType
    {
        /// <summary>
        ///     Name of the error type
        /// </summary>
        public string Name { get; } = "Error";

        /// <summary>
        ///     Color of the error type (red)
        /// </summary>
        public string HexColor { get; } = "#ff3300";

        /// <summary>
        ///     Symbol (X) of the error type
        /// </summary>
        public string Symbol { get; } = "";

        /// <summary>
        /// Sigleton pattern
        /// </summary>
        public static IMessageType Instance { get; } = new ErrorType();
    }
}