using System.Collections;
using UnityEngine;

public class BottomBarButtonFrame : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float chaseDuration;
    [SerializeField] private AnimationCurve chasingCurve;
    [SerializeField] private AnimationCurve trailingCurve;
    
    private RectTransform _selfRectTransform;
    private RectTransform _targetRectTransform;
    
    private Coroutine _chaseAnimation;
    
    private void Awake()
    {
        _selfRectTransform = GetComponent<RectTransform>();
    }
    
    public void SetTarget(BottomBarButton target)
    {
        _targetRectTransform = target != null ? target.RectTransform : null;
        
        if (_chaseAnimation != null)
        {
            StopCoroutine(_chaseAnimation);
        }
        _chaseAnimation = StartCoroutine(ChaseSequence());
    }
    
    private IEnumerator ChaseSequence()
    {
        if (_targetRectTransform != null)
        {
            float direction = Mathf.Sign(GetXCentre(_targetRectTransform) - GetXCentre(_selfRectTransform));
            float startXMin = GetXMin(_selfRectTransform);
            float startXMax = GetXMax(_selfRectTransform);
            
            float startTime = Time.time;
            while (Time.time < startTime + chaseDuration)
            {
                float targetXMin = GetXMin(_targetRectTransform);
                float targetXMax = GetXMax(_targetRectTransform);
                float interpolate = (Time.time - startTime) / chaseDuration;
                SetXs(_selfRectTransform, 
                    Mathf.LerpUnclamped(startXMin, targetXMin, direction > 0 ? trailingCurve.Evaluate(interpolate) : chasingCurve.Evaluate(interpolate)),
                    Mathf.LerpUnclamped(startXMax, targetXMax, direction > 0 ? chasingCurve.Evaluate(interpolate) : trailingCurve.Evaluate(interpolate)));
                yield return null;
            }
            SetXs(_selfRectTransform, GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
        }
    }

    private float GetXCentre(RectTransform rectTransform)
    {
        return GetX(rectTransform, 0f);
    }
    
    private float GetXMin(RectTransform rectTransform)
    {
        return GetX(rectTransform, -1f);
    }

    private float GetXMax(RectTransform rectTransform)
    {
        return GetX(rectTransform, 1f);
    }

    private float GetX(RectTransform rectTransform, float position) // -1 = min, 1 = max
    {
        return rectTransform.position.x + rectTransform.rect.width / 2f * position; 
    }

    private void SetXs(RectTransform rectTransform, float xMin, float xMax)
    {
        rectTransform.position =  new Vector2((xMax + xMin) / 2f, rectTransform.position.y);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xMax - xMin);
    }
}
