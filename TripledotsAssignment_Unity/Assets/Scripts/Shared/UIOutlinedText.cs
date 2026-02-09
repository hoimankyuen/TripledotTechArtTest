using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIOutlinedText : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<TextMeshProUGUI> textMeshProUGUIs;

    [Header("Settings")]
    [SerializeField] private string text;

    private void OnValidate()
    {
        ApplyTextToComponents();
    }
    
    public void SetText(string newText)
    {
        text = newText;
        ApplyTextToComponents();
    }

    private void ApplyTextToComponents()
    {
        foreach (TextMeshProUGUI textMeshProUGUI in textMeshProUGUIs)
        {
            textMeshProUGUI.text = text;
        }
    }
}
