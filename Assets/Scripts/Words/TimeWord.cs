namespace SwedishApp.Words
{
    /// <summary>
    /// This class houses the additional editor input field for the month number, 
    /// -1 values won't be displayed on flashcards etc.
    /// </summary>
    [System.Serializable]
    public class TimeWord : Word 
    {
        public int monthNumber = -1;
    }
}