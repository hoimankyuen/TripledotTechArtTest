using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarButton : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Animator animator;
    [SerializeField] private Image unlockedImage;
    [SerializeField] private Image lockedImage;
    [SerializeField] private TextMeshProUGUI[] labelTexts;
        
    private static readonly int SelectedAnimationKey = Animator.StringToHash("Selected");
    
    public RectTransform RectTransform { get; private set; }
    public BottomBarView.ButtonInfo ButtonInfo { get; private set; }

    private bool _selected;
    private bool _locked;

    private event Action<BottomBarButton> _onClick;
    
    public void Setup(BottomBarView.ButtonInfo buttonInfo, Action<BottomBarButton> action)
    {
        RectTransform = transform as RectTransform;
        
        ButtonInfo = buttonInfo;
        _onClick = action;
        _locked = buttonInfo.Locked;
        
        unlockedImage.sprite = buttonInfo.Icon;
        unlockedImage.gameObject.SetActive(!buttonInfo.Locked);
        lockedImage.gameObject.SetActive(buttonInfo.Locked);
        foreach (TextMeshProUGUI labelText in labelTexts)
        {
            labelText.text = buttonInfo.Label;
        }
    }

    public void Select()
    {
        if (_locked)
            return;

        if (_selected)
            return;
        
        _onClick?.Invoke(this);
    }

    public void ShowAsSelected(bool selected)
    {
        _selected = selected;
        animator.SetBool(SelectedAnimationKey, selected);
    }
}
