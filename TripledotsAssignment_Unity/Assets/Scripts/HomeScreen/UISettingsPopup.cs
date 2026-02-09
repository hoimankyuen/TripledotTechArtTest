using UnityEngine;

public class UISettingsPopup : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private UIPopup popup;

    public void Show(bool show)
    {
        popup.Toggle(show);
        if (show)
        {
            popup.ShowMainPageImmediately();
        }
    }

    public void ToggleMusic(bool value)
    {
        // TODO: Hook up with settings system
    }

    public void ToggleSound(bool value)
    {
        // TODO: Hook up with settings system
    }

    public void ToggleVibration(bool value)
    {
        // TODO: Hook up with settings system
    }

    public void ToggleNotifications(bool value)
    {
        // TODO: Hook up with settings system
    }

    public void ShowLanguagePage()
    {
        popup.ShowSubPage(0);
    }

    public void SetLanguage(int languageIndex)
    {
        // TODO: Hook up with settings system
        popup.ShowMainPage();
    }
    
    public void ShowMainPage()
    {
        popup.ShowMainPage();
    }

}
