using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UISimpleButtonClickAnimation : MonoBehaviour
{
    [SerializeField] private float sizeChange = 0.1f;
    [SerializeField] private float duration = 0.2f;
    
    private Button _button;
    private Coroutine _onClickCoroutine;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }

    private void OnClick()
    {
        if (_onClickCoroutine != null)
        {
            StopCoroutine(_onClickCoroutine);
            _onClickCoroutine = null;
        }
        _onClickCoroutine = StartCoroutine(OnClickSequence());
    }

    private IEnumerator OnClickSequence()
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            _button.transform.localScale = Vector3.one * (1 + Bonce((Time.time - startTime) / duration) * sizeChange);
            yield return null;
        }
        _button.transform.localScale = Vector3.one;
        _onClickCoroutine = null;
    }

    private static float Bonce(float t)
    {
        return Mathf.Abs(Mathf.Sin(t * Mathf.PI));
    }
}
