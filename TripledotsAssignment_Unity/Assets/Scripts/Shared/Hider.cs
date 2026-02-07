using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hider : MonoBehaviour
{
    private static readonly int ShownAnimationKey = Animator.StringToHash("Shown");
    
    private Animator _animator;
    
    public void Show(bool show)
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
        _animator.SetBool(ShownAnimationKey, show);
    }
}
