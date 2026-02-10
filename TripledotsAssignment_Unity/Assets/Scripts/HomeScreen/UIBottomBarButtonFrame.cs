using System.Collections;
using UnityEngine;

public class UIBottomBarButtonFrame : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    
    [Header("Settings")]
    [SerializeField] private float chaseDuration;
    [SerializeField] private AnimationCurve chasingCurve;
    [SerializeField] private AnimationCurve trailingCurve;
    [SerializeField] private AnimationCurve appearingCurve;
    
    private RectTransform _selfRectTransform;
    private RectTransform _targetRectTransform;
    
    private Coroutine _chaseAnimation;
    private ScreenOrientation _lastOrientation;

    private void Awake()
    {
        _selfRectTransform = transform as RectTransform;
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
        if (_chaseAnimation != null)
        {
            StopCoroutine(_chaseAnimation);
        }
        _chaseAnimation = StartCoroutine(ChaseSequence(target != null ? target.RectTransform : null));
    }
    
    private IEnumerator ChaseSequence(RectTransform target)
    {
        RectTransform previousTarget = _targetRectTransform;
        _targetRectTransform = target;
        
        if (previousTarget != null && _targetRectTransform != null)
        {
            // moving horizontally
            SetY(1);
            float direction = Mathf.Sign(GetXCentre(_targetRectTransform) - GetXCentre(_selfRectTransform));
            float startXMin = GetXMin(_selfRectTransform);
            float startXMax = GetXMax(_selfRectTransform);
            float startTime = Time.time;
            while (Time.time < startTime + chaseDuration)
            {
                float targetXMin = GetXMin(_targetRectTransform);
                float targetXMax = GetXMax(_targetRectTransform);
                float interpolate = (Time.time - startTime) / chaseDuration;
                SetXs(
                    Mathf.LerpUnclamped(startXMin, targetXMin, direction > 0 ? trailingCurve.Evaluate(interpolate) : chasingCurve.Evaluate(interpolate)),
                    Mathf.LerpUnclamped(startXMax, targetXMax, direction < 0 ? trailingCurve.Evaluate(interpolate) : chasingCurve.Evaluate(interpolate)));
                yield return null;
            }
            SetXs(GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
        }
        else
        {
            // moving vertically
            float startPosition = previousTarget == null ? 0f : 1f;
            float targetPosition = _targetRectTransform == null ? 0f : 1f;
            float startTime = Time.time;
            while (Time.time < startTime + chaseDuration)
            {
                if (_targetRectTransform != null)
                {
                    SetXs(GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
                }
                
                float interpolate = (Time.time - startTime) / chaseDuration;
                SetY(Mathf.LerpUnclamped(startPosition, targetPosition, appearingCurve.Evaluate(interpolate)));
                yield return null;
            }
            if (_targetRectTransform != null)
            {
                SetXs(GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
            }
            SetY(targetPosition);
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
            SetXs(GetXMin(_targetRectTransform),  GetXMax(_targetRectTransform));
        }
        _lastOrientation = Screen.orientation;
    }

    private static float GetXCentre(RectTransform rectTransform)
    {
        return GetX(rectTransform, 0f);
    }
    
    private static float GetXMin(RectTransform rectTransform)
    {
        return GetX(rectTransform, -1f);
    }

    private static float GetXMax(RectTransform rectTransform)
    {
        return GetX(rectTransform, 1f);
    }

    private static float GetX(RectTransform rectTransform, float position) // -1 = min, 1 = max
    {
        return rectTransform.position.x + rectTransform.rect.width / 2f * position; 
    }

    private void SetXs(float xMin, float xMax)
    {
        _selfRectTransform.position =  new Vector2((xMax + xMin) / 2f, _selfRectTransform.position.y);
        _selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xMax - xMin);
    }
    
    private void SetY(float position) // 0 = hidden, 1 = extended
    {
        _selfRectTransform.anchoredPosition =  new Vector2(_selfRectTransform.anchoredPosition.x, Mathf.Lerp(-_selfRectTransform.rect.height, 0, position));
    }
}
