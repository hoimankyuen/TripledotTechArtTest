using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISafeAreaContainer : MonoBehaviour
{
    private Canvas _canvas;
    private RectTransform _rectTransform;
    
    private ScreenOrientation _lastOrientation;
    private Vector2 _lastScreenSize;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>(); 
    }
    
    private void Start()
    {
        _rectTransform.anchorMin = Vector2.zero;
        _rectTransform.anchorMax = Vector2.one;
        _rectTransform.pivot = Vector2.one / 2f;
        
        RefreshSafeArea();
    }

    private void Update()
    {
        // note: checking for screen size for detecting editor window resizing only, should be safe removing in production
        if (Screen.orientation == _lastOrientation && 
            Mathf.Approximately(Vector2.SqrMagnitude(_lastScreenSize - new Vector2(Screen.width, Screen.height)), 0f))
            return;
        
        RefreshSafeArea();
    }

    private void RefreshSafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        float scaleFactor = _canvas != null ? 1f / _canvas.transform.localScale.x : 1.0f;
        
        Vector2 newSizeDelta = new Vector2(safeArea.width - screenSize.x, safeArea.height - screenSize.y);
        Vector2 newAnchoredPosition = new Vector2(newSizeDelta.x / 2f + safeArea.x, newSizeDelta.y / 2f + safeArea.y);

        _rectTransform.sizeDelta = newSizeDelta * scaleFactor;
        _rectTransform.anchoredPosition = newAnchoredPosition * scaleFactor;
        
        _lastOrientation = Screen.orientation;
        _lastScreenSize = screenSize;
    }
}
