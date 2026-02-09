using System.Collections;
using UnityEngine;

public class UIBottomBarButtonFrame : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float chaseDuration;
    [SerializeField] private AnimationCurve chasingCurve;
    [SerializeField] private AnimationCurve trailingCurve;
    
    private RectTransform _selfRectTransform;
    private RectTransform _targetRectTransform;
    
    private Coroutine _chaseAnimation;
    
    private ScreenOrientation _lastOrientation;
    
    private void Awake()
    {
        _selfRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        RefreshTransform();
    }

    private void LateUpdate()
    {
        if (Screen.orientation == _lastOrientation)
            return;

        RefreshTransform();
    }
    
    public void SetTarget(UIBottomBarButton target)
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
                    Mathf.LerpUnclamped(startXMax, targetXMax, direction < 0 ? trailingCurve.Evaluate(interpolate) : chasingCurve.Evaluate(interpolate)));
                yield return null;
            }
            SetXs(_selfRectTransform, GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
        }

        _chaseAnimation = null;
    }

    private void RefreshTransform()
    {
        StartCoroutine(DelayedRefreshTransform());
    }
    
    private IEnumerator DelayedRefreshTransform()
    {
        // wait for layout group to refresh by delaying a single frame
        yield return null;
        
        if (_chaseAnimation != null)
            yield break;

        if (_targetRectTransform != null)
        {
            SetXs(_selfRectTransform, GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
        }
        _lastOrientation = Screen.orientation;
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
