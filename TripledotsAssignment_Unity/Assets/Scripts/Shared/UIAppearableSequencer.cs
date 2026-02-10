using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppearableSequencer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<UIAppearableSequencerEntry> entries;
    [SerializeField] private float fastForwardDelay = 0.1f;

    private void Awake()
    {
        StartCoroutine(AppearSequence());
    }

    private IEnumerator AppearSequence()
    {
        if (ScreenChangeFader.Instance != null)
        {
            yield return new WaitWhile(() => ScreenChangeFader.Instance.Fading);
        }
        
        foreach (UIAppearableSequencerEntry entry in entries)
        {
            float startTIme = Time.time;
            while (Time.time < startTIme + (Input.GetMouseButton(0) ? fastForwardDelay : entry.delay))
            {
                yield return null;
            }
            entry.appearable?.Appear();
        }
    }
}
