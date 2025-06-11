using UnityEngine;

namespace SwedishApp.Words
{
    [System.Serializable]
    public class PronounsWord : Word
    {
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