using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string homeSceneName;
    [SerializeField] private string levelCompletedSceneName;

    public void ChangeToHomeScreen()
    {
        ScreenChangeFader.Instance.Fade(() => SceneManager.LoadScene(homeSceneName));
    }
    
    public void ChangeToLevelCompletedScreen()
    {
        ScreenChangeFader.Instance.Fade(() => SceneManager.LoadScene(levelCompletedSceneName));
    }
}
