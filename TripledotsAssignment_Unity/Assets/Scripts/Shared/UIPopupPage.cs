using System.Collections;
using UnityEngine;


public class UIPopupPage : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float duration;

    private bool _showing;
    private Coroutine _showCoroutine;
    
    public void Show(bool show,  bool isSubPage, bool immediate = false)
    {
        if (immediate)
        {
            SetPosition(show ? 0f : isSubPage ? 1f : -1f);
            canvasGroup.interactable = show;
            canvasGroup.blocksRaycasts = show;
            _showing = show;
        }
        else
        {
            if (_showing == show)
                return;
            
            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }
            _showCoroutine = StartCoroutine(ShowSequence(show, isSubPage));
        }
    }

    private IEnumerator ShowSequence(bool show, bool isSubPage)
    {
        if (show)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        
        float fromPosition = show ? (isSubPage ? 1f : -1f) : 0f;
        float toPosition = show ? 0f : (isSubPage ? 1f : -1f);
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            SetPosition(Sinerp(fromPosition, toPosition, (Time.time - startTime) / duration));
            yield return null;
        }
        SetPosition(toPosition);
        
        if (!show)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        _showing = show;
        
        _showCoroutine = null;
    }

    private void SetPosition(float position)
    {
        rectTransform.anchorMin = new Vector2(position, 0f);
        rectTransform.anchorMax = new Vector2(position + 1f, 1f);
        rectTransform.anchoredPosition = Vector3.zero;
    }
    
    // Easing out method extracted from Mathfx
    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }
}
