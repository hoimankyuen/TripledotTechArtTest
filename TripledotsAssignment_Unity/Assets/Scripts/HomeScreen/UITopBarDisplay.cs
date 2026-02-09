using TMPro;
using UnityEngine;

public class UITopBarDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI contentText;

    public void SetText(string text)
    {
        contentText.text = text;
    }
}
