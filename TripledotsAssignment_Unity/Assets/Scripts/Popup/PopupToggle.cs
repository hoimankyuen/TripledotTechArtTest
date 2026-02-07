using UnityEngine;
using UnityEngine.UI;

public class PopupToggle : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private Animator animator;
    
    private static readonly int IsOnAnimationKey = Animator.StringToHash("IsOn");

    private void Awake()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(toggle.isOn);
    }

    private void OnDestroy()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        animator.SetBool(IsOnAnimationKey, isOn);
    }
}
