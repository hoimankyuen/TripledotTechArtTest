using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private UITogglable togglable;

    public void Show(bool show)
    {
        togglable.Toggle(show);
    }
}
