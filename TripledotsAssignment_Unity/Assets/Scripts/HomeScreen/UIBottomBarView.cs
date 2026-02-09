using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIBottomBarView : MonoBehaviour
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
    [SerializeField] private UITogglable togglable;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private UIBottomBarButtonFrame buttonFrame;

    [Header("Settings")]
    [SerializeField] private List<ButtonInfo> buttonInfos;
    [SerializeField] private int defaultSelectedButtonIndex;
    
    private readonly List<UIBottomBarButton> _buttons = new List<UIBottomBarButton>();
    private UIBottomBarButton _selectedButton;
    
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
            UIBottomBarButton button = Instantiate(buttonPrefab, buttonContainer).GetComponent<UIBottomBarButton>();
            button.Setup(buttonInfo, SelectButton);
            _buttons.Add(button);
        }
        buttonPrefab.SetActive(false);
    }

    private void SelectButton(UIBottomBarButton selectedButton)
    {
        _selectedButton = selectedButton;
        foreach (UIBottomBarButton button in _buttons)
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
        togglable.Toggle(show);
    }
}
