using System.Collections.Generic;

namespace SwedishApp.Meta
{
    public static class Helpers
    {
        public static string CleanWord(string _wordToClean)
        {
            List<char> chars = new();
            bool ignoreLetters = false;
            string cleanedWord = _wordToClean.Replace(@"\u00AD", null);

            foreach (char c in cleanedWord)
            {
                if (c == '<')
                {
                    ignoreLetters = true;
                    continue;
                }
                else if (c == '>')
                {
                    ignoreLetters = false;
                    continue;
                }
                if (ignoreLetters) continue;

                chars.Add(c);
            }

            return new(chars.ToArray());
        }
    }
}
