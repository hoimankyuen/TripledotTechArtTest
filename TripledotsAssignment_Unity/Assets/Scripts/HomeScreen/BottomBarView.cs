using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomBarView : UIAnimatedTogglable
{
    [System.Serializable]
    public class ButtonInfo
    {
        public string id;
        public Sprite Icon;
        public string Label;
        public bool Locked;
        public UnityEvent Callback;
    }
    
    [Header("Components")] 
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private UIBottomBarButtonFrame buttonFrame;

    [Header("Settings")]
    [SerializeField] private List<ButtonInfo> buttonInfos;
    [SerializeField] private UnityEvent contentActivated;
    [SerializeField] private UnityEvent closed;
    
    private readonly List<UIBottomBarButton> _buttons = new List<UIBottomBarButton>();
    private UIBottomBarButton _selectedButton;

    public string SelectedButtonId => _selectedButton != null ? _selectedButton.ButtonInfo.id : "";
    
    private void Awake()
    {
        SetupButtons();
    }

    private void Start()
    {
        SelectButton(null);
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
        _selectedButton = _selectedButton != selectedButton ? selectedButton : null;
        
        foreach (UIBottomBarButton button in _buttons)
        {
            button.ShowAsSelected(button == _selectedButton);
        }
        buttonFrame.SetTarget(_selectedButton);

        if (_selectedButton != null)
        {
            _selectedButton.ButtonInfo?.Callback?.Invoke();
            contentActivated?.Invoke();
        }
        else
        {
            closed?.Invoke();
        }
    }

    public void SelectNoButton()
    {
        SelectButton(null);
    }
}
