using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class houses the additional editor input fields for the different word forms 
    /// </summary>
    [System.Serializable]
    public class PronounWord : Word
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