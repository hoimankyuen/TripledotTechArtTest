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
    [SerializeField] private UITogglable togglable;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private UIBottomBarButtonFrame buttonFrame;

    [Header("Settings")]
    [SerializeField] private List<ButtonInfo> buttonInfos;
    [SerializeField] private int defaultSelectedButtonIndex;
    [SerializeField] private UnityEvent contentActivated;
    [SerializeField] private UnityEvent closed;
    
    private readonly List<UIBottomBarButton> _buttons = new List<UIBottomBarButton>();
    private UIBottomBarButton _selectedButton;
    
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

    public void Show(bool show)
    {
        togglable.Toggle(show);
    }
}
