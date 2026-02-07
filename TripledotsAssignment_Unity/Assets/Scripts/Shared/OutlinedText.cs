using TMPro;
using UnityEngine;

public class OutlinedText : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI[] texts;

    public void SetText(string content)
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = content;
        }
    }
}
