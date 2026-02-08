using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Hider hider;

    public void Show(bool show)
    {
        hider.Show(show);
    }
}
