using TMPro;
using UnityEngine;

namespace SwedishApp.UI
{
    public class FontsizeTest : MonoBehaviour
    {
        int smallSize = 36;
        int mediumSize = 46;
        int largeSize = 56;

        [SerializeField] private TextMeshProUGUI testText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UIManager.Instance.FontSmallEvent += () => testText.fontSize = smallSize;
            UIManager.Instance.FontMediumEvent += () => testText.fontSize = mediumSize;
            UIManager.Instance.FontLargeEvent += () => testText.fontSize = largeSize;
        }
    }
}
