namespace TwitterCheck
{
    /// <summary>
    ///     Class to parse the json response (to is x user available) from twitter
    /// </summary>
    public class TwitterJson
    {
        /// <summary>
        ///     Is valid
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        ///     why is valid
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        ///     System msg about the availability of that username
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        ///     Description
        /// </summary>
        public string Desc { get; set; }
    }
}