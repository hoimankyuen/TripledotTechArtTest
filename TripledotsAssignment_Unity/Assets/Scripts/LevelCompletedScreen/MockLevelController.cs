using UnityEngine;
using UnityEngine.Events;

public class MockLevelController : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField] private int starValue;
    [SerializeField] private UnityEvent<int> starValueChanged;

    [Header("Coins")] 
    [SerializeField] private int coinValueInitial;
    [SerializeField] private int coinValueIncrease;
    [SerializeField] private UnityEvent<int> initialCoinValueChanged;
    [SerializeField] private UnityEvent<int> currentCoinValueChanged;
    
    [Header("Crown")]
    [SerializeField] private int crownValueInitial;
    [SerializeField] private int crownValueIncrease;
    [SerializeField] private UnityEvent<int> initialCrownValueChanged;
    [SerializeField] private UnityEvent<int> currentCrownValueChanged;

    private int _starValue;
    private int _coinValue;
    private int _crownValue;
    
    private void Start()
    {
        _starValue = starValue;
        starValueChanged?.Invoke(_starValue);
        
        _coinValue = coinValueInitial + coinValueIncrease;
        initialCoinValueChanged?.Invoke(coinValueInitial);
        currentCoinValueChanged?.Invoke(_coinValue);
        
        _crownValue = crownValueInitial + crownValueIncrease;
        initialCrownValueChanged?.Invoke(crownValueInitial);
        currentCrownValueChanged?.Invoke(_crownValue);
    }

    public void IncreaseCoinValue(int amount)
    {
        _coinValue += amount;
        currentCoinValueChanged?.Invoke(_coinValue);
    }
}
