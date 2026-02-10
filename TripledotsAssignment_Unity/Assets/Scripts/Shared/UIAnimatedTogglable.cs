using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatedTogglable : UIAppearable
{
    private static readonly int ShownAnimationKey = Animator.StringToHash("Shown");
    
    private Animator _animator;

    public override void Appear()
    {
        Toggle(true);
    }
    
    public void Toggle(bool show)
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
        _animator.SetBool(ShownAnimationKey, show);
    }
}
