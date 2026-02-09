using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopBarHealthDisplay : UITopBarDisplay
{
    [Header("Components")]
    [SerializeField] private UIOutlinedText heartText;
    [SerializeField] private Button addButton;

    private int _maxHealth;
    private int _health;
    
    public void SetMaxHealth(int value)
    {
        _maxHealth = value;
        ResolveDisplay();
    }
    
    public override void SetValue(int value)
    {
        _health = value;
        ResolveDisplay();
    }

    private void ResolveDisplay()
    {
        heartText.SetText(_health.ToString());
        contentText.text = _health == _maxHealth ? "Full" : $"{_health} / {_maxHealth}";
        addButton.gameObject.SetActive(_health < _maxHealth);
    }
}
