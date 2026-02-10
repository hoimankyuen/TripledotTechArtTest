using UnityEngine;
using UnityEngine.Events;

public class MockHomeController : MonoBehaviour
{
     
    [Header("Coins")] 
    [SerializeField] private int coinValue;
    [SerializeField] private UnityEvent<int> coinValueChanged;

    [Header("Life")]
    [SerializeField] private int maxHealthValue;
    [SerializeField] private UnityEvent<int> maxHealhtValueChanged;
    [SerializeField] private int healthValue;
    [SerializeField] private UnityEvent<int> healthValueChanged;
    
    [Header("Stars")]
    [SerializeField] private int starValue;
    [SerializeField] private UnityEvent<int> starValueChanged;
    
    private int _coinValue;
    private int _maxHealthValue;
    private int _healthValue;
    private int _starValue;
    
    private void Start()
    {
        _coinValue = coinValue;
        coinValueChanged?.Invoke(_coinValue);
        
        _maxHealthValue = maxHealthValue;
        maxHealhtValueChanged?.Invoke(_maxHealthValue);
        _healthValue = healthValue;
        healthValueChanged?.Invoke(_healthValue);
        
        _starValue = starValue;
        starValueChanged?.Invoke(_starValue);
    }

    public void IncreaseCoinValue(int amount)
    {
        _coinValue += amount;
        coinValueChanged?.Invoke(_coinValue);
    }
    
    public void IncreaseLifeValue(int amount)
    {
        _healthValue = Mathf.Min(_healthValue + amount, _maxHealthValue);
        healthValueChanged?.Invoke(_healthValue);
    }

    public void OnBottomBarViewContentActivated()
    {
        Debug.Log("Content Activated");
    }
    
    public void OnBottomBarViewClosed()
    {
        Debug.Log("Closed");
    }
}
