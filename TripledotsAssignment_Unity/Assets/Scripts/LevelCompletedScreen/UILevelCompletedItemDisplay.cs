using System.Collections;
using UnityEngine;

public class UILevelCompletedItemDisplay : UIAnimatedAppearable
{
    [Header("Components")]
    [SerializeField] private UIText valueText;
    [SerializeField] private ParticleSystem burstEffect;
    [SerializeField] private Animator jumpAnimation;

    [Header("Settings")]
    [SerializeField] private float textAnimationDelay;
    [SerializeField] private float textAnimationDuration;

    private static readonly int JumpAnimationKey = Animator.StringToHash("Jump");
    
    private int _currentValue;
    private int _pendingValue;
    
    private Coroutine _animateValueCoroutine;
    private bool _textAppeared;
    
    public override void Appear()
    {
        base.Appear();

        _animateValueCoroutine = StartCoroutine(WaitAndAnimateValueText());
        if (burstEffect != null)
        {
            burstEffect.Play();
        }
    }

    public void SetInitialValue(int value)
    {
        if (_textAppeared)
            return;
        
        _currentValue = value; 
    }
    
    public void SetValue(int newValue)
    {
        if (_textAppeared)
        {
            if (_animateValueCoroutine != null)
            {
                StopCoroutine(_animateValueCoroutine);
                _animateValueCoroutine = null;
            }
            _animateValueCoroutine = StartCoroutine(AnimateValueText(newValue));
        }
        else
        {
            _pendingValue = newValue;
        }
    }

    private IEnumerator WaitAndAnimateValueText()
    {
        valueText.SetText("");
        yield return new WaitForSeconds(textAnimationDelay);
        
        _textAppeared = true;
        yield return AnimateValueText(_pendingValue);
    }

    private IEnumerator AnimateValueText(int newValue)
    {
        if (jumpAnimation != null)
        {
            jumpAnimation.SetTrigger(JumpAnimationKey);
        }

        float fromValue = _currentValue;
        float startTime = Time.time;
        while (Time.time < startTime + textAnimationDuration)
        {
            _currentValue = Mathf.RoundToInt(Mathf.Lerp(fromValue, newValue, (Time.time - startTime) / textAnimationDuration));
            valueText.SetText($"{_currentValue:n0}");
            yield return null;
        }
        _currentValue = newValue;
        valueText.SetText($"{_currentValue:n0}");

        _animateValueCoroutine = null;
    }
}
