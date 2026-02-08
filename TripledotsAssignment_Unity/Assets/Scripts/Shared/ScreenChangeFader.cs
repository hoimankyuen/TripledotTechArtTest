using System;
using System.Collections;
using UnityEngine;

public class ScreenChangeFader : MonoBehaviour
{
   public static ScreenChangeFader Instance;

   [Header("Components")]
   [SerializeField] private CanvasGroup canvasGroup;

   [Header("Settings")]
   [SerializeField] private float fadeDuration;
   [SerializeField] private float loadDelay;
   
   public bool Fading { get; private set; }
   
   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }
      Instance = this;
      DontDestroyOnLoad(gameObject);
   }

   private void OnDestroy()
   {
      if (Instance == this)
      {
         Instance = null;
      }
   }

   public void Fade(Action onFaded)
   {
      if (Fading)
         return;

      StartCoroutine(FadeSequence(onFaded));
   }

   private IEnumerator FadeSequence(Action onFaded)
   {
      Fading = true;
      
      canvasGroup.alpha = 0;
      canvasGroup.blocksRaycasts = true;
      canvasGroup.interactable = true;
      
      float startTime = Time.time;
      while (Time.time - startTime < fadeDuration)
      {
         canvasGroup.alpha = Sinerp(0f, 1f, (Time.time - startTime) / fadeDuration);
         yield return null;
      }
      canvasGroup.alpha = 1;
      
      onFaded?.Invoke();
      yield return new WaitForSeconds(loadDelay);

      startTime = Time.time;
      while (Time.time - startTime < fadeDuration)
      {
         canvasGroup.alpha = Coserp(1f, 0f, (Time.time - startTime) / fadeDuration);
         yield return null;
      }
      canvasGroup.alpha = 0;
      canvasGroup.blocksRaycasts = false;
      canvasGroup.interactable = false;

      Fading = false;
   }
   
   // Easing out method extracted from Mathfx
   public static float Sinerp(float start, float end, float value)
   {
      return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
   }
   
   // Easing in method extracted from Mathfx
   public static float Coserp(float start, float end, float value)
   {
      return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
   }
}
