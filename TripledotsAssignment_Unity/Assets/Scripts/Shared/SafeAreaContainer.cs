using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaContainer : MonoBehaviour
{
    private Canvas _canvas;
    private RectTransform _rectTransform;
    
    private ScreenOrientation _lastOrientation;

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
        if (Screen.orientation == _lastOrientation)
            return;
        
        RefreshSafeArea();
    }

    private void RefreshSafeArea()
    {
        float scaleFactor = _canvas != null ? 1f / _canvas.transform.localScale.x : 1.0f;
        Rect safeArea = Screen.safeArea;
        Resolution resolution = Screen.currentResolution;
        
        Vector2 newSizeDelta = new Vector2(safeArea.width - resolution.width, safeArea.height - resolution.height);
        Vector2 newAnchoredPosition = new Vector2(newSizeDelta.x / 2f + safeArea.x, newSizeDelta.y / 2f + safeArea.y);

        _rectTransform.sizeDelta = newSizeDelta * scaleFactor;
        _rectTransform.anchoredPosition = newAnchoredPosition * scaleFactor;
        
        _lastOrientation = Screen.orientation;
    }
}
