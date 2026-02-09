using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIParticleRenderer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera renderCamera;

    private RectTransform _rectTransform;
    
    private Vector2 _lastSize;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    private void Start()
    {
        ResizeCamera();
    }
    
    private void Update()
    {
        if (Mathf.Approximately(Vector2.Distance(_lastSize, _rectTransform.rect.size), 0f))
            return;
        
        ResizeCamera();
    }

    private void ResizeCamera()
    {
        renderCamera.orthographicSize = Mathf.Max(
            _rectTransform.rect.size.x * _rectTransform.lossyScale.x,
            _rectTransform.rect.size.y * _rectTransform.lossyScale.y) / 2f;
    }
}
