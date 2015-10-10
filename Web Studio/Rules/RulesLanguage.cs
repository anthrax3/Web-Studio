namespace Web_Studio.Reglas
{
    /// <summary>
    /// Class to manage the translations of the rules
    /// </summary>
    public class RulesLanguage
    {
        /// <summary>
        /// Language of the translation
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Title of the rule
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of the rule
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Name of the type of the rule
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Description of the suggested solution for this rule
        /// </summary>
        public string SuggestedSolution { get; set; }
    }
}