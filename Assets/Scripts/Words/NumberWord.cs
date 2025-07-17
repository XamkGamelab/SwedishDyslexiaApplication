namespace SwedishApp.Words
{
    [System.Serializable]
    public class NumberWord : Word
    {
        /// <summary>
        /// This class houses the additional editor input fields for the ordinal forms and numerical number
        /// </summary>
        public string ordinalSwedish = "första";
        public string ordinalFinnish = "ensimmäinen";
        public int number = 1;
    }
}