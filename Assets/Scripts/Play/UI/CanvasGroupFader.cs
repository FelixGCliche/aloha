using System.Collections;
using UnityEngine;

namespace Game
{
    // LouisRD
    public class CanvasGroupFader : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 2f;

        public IEnumerator FadeRoutine(CanvasGroup canvasGroup, float start, float end)
        {
            float count = 0f;

            while (count < fadeDuration)
            {
                count += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, end, count / fadeDuration);

                yield return null;
            }
        }
    }
}