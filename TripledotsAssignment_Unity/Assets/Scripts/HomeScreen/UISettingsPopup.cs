using UnityEngine;

public class UISettingsPopup : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private UITogglable togglable;

    public void Show(bool show)
    {
        togglable.Toggle(show);
    }
}
