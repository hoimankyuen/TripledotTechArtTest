using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITopBarDisplay : UIAnimatedAppearable
{
    [Header("Components")]
    [SerializeField] protected UIText contentText;
    [SerializeField] protected Button addButton;
    
    [Header("Settings")]
    [SerializeField] private float textAnimationDuration;
    [SerializeField] private UnityEvent onAddButtonClicked;
    
    private int _currentValue;
    private Coroutine _animateValueCoroutine;

    private void Awake()
    {
        addButton.onClick.AddListener(onAddButtonClicked.Invoke);
    }

    private void OnDestroy()
    {
        if (addButton != null)
        {
            addButton.onClick.RemoveListener(onAddButtonClicked.Invoke);
        }
    }

    public virtual void SetValue(int value)
    {
        if (_animateValueCoroutine != null)
        {
            StopCoroutine(_animateValueCoroutine);
            _animateValueCoroutine = null;
        }
        _animateValueCoroutine = StartCoroutine(AnimateValueText(value));
    }
    
    private IEnumerator AnimateValueText(int value)
    { 
        float startTime = Time.time;
        float fromValue = _currentValue;
        while (Time.time < startTime + textAnimationDuration)
        {
            _currentValue = Mathf.RoundToInt(Mathf.Lerp(fromValue, value, (Time.time - startTime) / textAnimationDuration));
            contentText.SetText($"{_currentValue:n0}");
            yield return null;
        }
        _currentValue = value;
        contentText.SetText($"{_currentValue:n0}");

        _animateValueCoroutine = null;
    }
}
