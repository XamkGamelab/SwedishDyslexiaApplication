using SwedishApp.UI;
using TMPro;
using UnityEngine;

namespace SwedishApp
{
    public class FontsizeTest : MonoBehaviour
    {
        int smallSize = 30;
        int mediumSize = 39;
        int largeSize = 48;

        [SerializeField] private TextMeshProUGUI testText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UIManager.instance.FontSmallEvent += () => {testText.fontSize = smallSize;};
            UIManager.instance.FontMediumEvent += () => {testText.fontSize = mediumSize;};
            UIManager.instance.FontLargeEvent += () => {testText.fontSize = largeSize;};
        }
    }
}
