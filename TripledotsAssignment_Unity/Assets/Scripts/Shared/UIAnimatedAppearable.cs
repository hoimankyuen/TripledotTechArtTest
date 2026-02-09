using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatedAppearable : UIAppearable
{
    private static readonly int AppearAnimationKey = Animator.StringToHash("Appear");
    
    private Animator _animator;

    public override void Appear()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
        _animator.SetTrigger(AppearAnimationKey);
    }
}
