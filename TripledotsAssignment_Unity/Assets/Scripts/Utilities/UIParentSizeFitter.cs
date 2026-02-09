using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIParentSizeFitter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float preferredWidth;
    [SerializeField] private float preferredHeight;
    [SerializeField] private float verticalPadding;
    [SerializeField] private float horizontalPadding;
    
    private RectTransform _rectTransform;
    private RectTransform _parentRectTransform;
    
    private Vector2 _lastParentSize;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = _rectTransform.parent as RectTransform;
    }

    private void Start()
    {
        FitInParent();
    }
    
    private void Update()
    {
        if (Mathf.Approximately(Vector2.Distance(_lastParentSize, _parentRectTransform.rect.size), 0f))
            return;
        
        FitInParent();
    }
    
    private void FitInParent()
    {
        RectTransform parentRectTransform = _rectTransform.parent as RectTransform;
        if (parentRectTransform == null)
            return;
        
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            Mathf.Min(preferredWidth, parentRectTransform.rect.size.x - horizontalPadding));
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            Mathf.Min(preferredHeight, parentRectTransform.rect.size.y - verticalPadding));
        
        _lastParentSize = parentRectTransform.rect.size;
    }
}
