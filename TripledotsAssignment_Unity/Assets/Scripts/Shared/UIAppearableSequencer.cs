using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppearableSequencer : MonoBehaviour
{
    [SerializeField] private List<UIAppearableSequencerEntry> entries;

    private void Awake()
    {
        StartCoroutine(AppearSequence());
    }

    private IEnumerator AppearSequence()
    {
        foreach (UIAppearableSequencerEntry entry in entries)
        {
            yield return new WaitForSeconds(entry.delay);
            entry.appearable?.Appear();
        }
    }
}
