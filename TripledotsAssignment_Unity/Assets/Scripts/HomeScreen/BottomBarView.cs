using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomBarView : MonoBehaviour
{
    [System.Serializable]
    public class ButtonInfo
    {
        public Sprite Icon;
        public string Label;
        public bool Locked;
        public UnityEvent Callback;
    }

    [Header("Components")] 
    [SerializeField] private Animator animator;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private BottomBarButtonFrame buttonFrame;

    [Header("Settings")]
    [SerializeField] private List<ButtonInfo> buttonInfos;
    [SerializeField] private int defaultSelectedButtonIndex;
    
    private static readonly int ShownAnimationKey = Animator.StringToHash("Shown");
    
    private readonly List<BottomBarButton> _buttons = new List<BottomBarButton>();
    private BottomBarButton _selectedButton;
    
    private void Awake()
    {
        SetupButtons();
    }

    private void Start()
    {
        SelectButton(_buttons[Mathf.Clamp(defaultSelectedButtonIndex, 0, buttonInfos.Count - 1)]);
    }
    
    private void SetupButtons()
    {
        foreach (ButtonInfo buttonInfo in buttonInfos)
        {
            BottomBarButton button = Instantiate(buttonPrefab, buttonContainer).GetComponent<BottomBarButton>();
            button.Setup(buttonInfo, SelectButton);
            _buttons.Add(button);
        }
        buttonPrefab.SetActive(false);
    }

    private void SelectButton(BottomBarButton selectedButton)
    {
        _selectedButton = selectedButton;
        foreach (BottomBarButton button in _buttons)
        {
            button.ShowAsSelected(button == selectedButton);
        }
        buttonFrame.SetTarget(_selectedButton);

        if (_selectedButton != null)
        {
            if (_selectedButton.ButtonInfo != null)
            {
                _selectedButton.ButtonInfo.Callback?.Invoke();
            }
        }
    }

    public void Show(bool show)
    {
        animator.SetBool(ShownAnimationKey, show);
    }
}
