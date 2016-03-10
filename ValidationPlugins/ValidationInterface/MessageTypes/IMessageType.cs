namespace ValidationInterface.MessageTypes
{
    /// <summary>
    ///     Message type interface
    /// </summary>
    public interface IMessageType
    {
        /// <summary>
        ///     Name of the message type
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Hexadecimal value (#FFFFFF) of the message type
        /// </summary>
        string HexColor { get; }

        /// <summary>
        ///     Symbol of the message type. X for error, ! for warning ...
        /// </summary>
        string Symbol { get; }
    }
}