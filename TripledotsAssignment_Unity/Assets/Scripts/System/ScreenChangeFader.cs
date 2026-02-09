using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChangeFader : MonoBehaviour
{
   public static ScreenChangeFader Instance;

   [Header("Components")] 
   [SerializeField] private CanvasGroup faderCanvasGroup;
   [SerializeField] private Image faderImage;

   [Header("Settings")]
   [SerializeField] private float fadeDuration;
   [SerializeField] private float fadeCoverage;
   [SerializeField] private float loadDelay;

   private static readonly int FadeFromProperty = Shader.PropertyToID("_FadeFrom");
   private static readonly int FadeToProperty = Shader.PropertyToID("_FadeTo");
   
   private Material _faderMaterialInstance;
   
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

      _faderMaterialInstance = Instantiate(faderImage.material);
      faderImage.material = _faderMaterialInstance;
   }

   private void OnDestroy()
   {
      if (Instance == this)
      {
         Instance = null;
         
         Destroy(_faderMaterialInstance);
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

      float height = faderImage.rectTransform.rect.height;
      float lowerFrom = -height / 2f - height * fadeCoverage;
      float lowerTo = height / 2f;
      float upperFrom = -height / 2f;
      float upperTo = height / 2f + height * fadeCoverage;
      
      faderCanvasGroup.alpha = 1;
      faderCanvasGroup.blocksRaycasts = true;
      faderCanvasGroup.interactable = true;
      
      float startTime = Time.time;
      while (Time.time - startTime < fadeDuration)
      {
         float interpolate = (Time.time - startTime) / fadeDuration;
         _faderMaterialInstance.SetFloat(FadeToProperty, Mathf.Lerp(lowerFrom, lowerTo, interpolate));
         _faderMaterialInstance.SetFloat(FadeFromProperty, Mathf.Lerp(upperFrom, upperTo, interpolate));
         yield return null;
      }
      
      onFaded?.Invoke();
      yield return new WaitForSeconds(loadDelay);

      startTime = Time.time;
      while (Time.time - startTime < fadeDuration)
      {
         float interpolate = (Time.time - startTime) / fadeDuration;
         _faderMaterialInstance.SetFloat(FadeFromProperty, Mathf.Lerp(lowerFrom, lowerTo, interpolate));
         _faderMaterialInstance.SetFloat(FadeToProperty, Mathf.Lerp(upperFrom, upperTo, interpolate));
         yield return null;
      }
      faderCanvasGroup.alpha = 0;
      faderCanvasGroup.blocksRaycasts = false;
      faderCanvasGroup.interactable = false;

      Fading = false;
   }
}
