using UnityEngine;

namespace SwedishApp.Words
{
    [System.Serializable]
    public class PronounWord : Word
    {
        public enum GrammaticalGender
        {
            masc = 1,
            fem = 2,
            neu = 3
        }

        public GrammaticalGender grammaticalGender;

        [Header("Omistusmuoto")]
        public string pronounPossessiveEnSwe     = "min";
        public string pronounPossessiveEttSwe    = "mitt";
        public string pronounPossessivePluralSwe = "mina";
        public string pronounPossessiveFin       = "minun";

        [Header("Objektimuoto")]
        public string pronounObjectSwe = "mig";
        public string pronounObjectFin = "minua/minut";
    }
}