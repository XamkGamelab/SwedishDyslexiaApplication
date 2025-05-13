namespace SwedishApp.Words
{
    /// <summary>
    /// This class houses the very basic variables required for every word.
    /// </summary>
    [System.Serializable]
    public class Word
    {
        public string swedishWord;
        public string finnishWord;

        protected readonly string colorTagStartDark = "<color=#EFA00B>";
        protected readonly string colorTagStartLight = "<color=#016FB9>";
        protected readonly string colorTagEnd = "</color>";
    }
}