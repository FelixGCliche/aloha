using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    // David D
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private float secondsToUpdateText = 0.2f;
        [SerializeField] private string textToGradualyAdd = ".....";
        
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            
            IEnumerator UpdateTextRoutine()
            {
                while (isActiveAndEnabled)
                {
                    foreach (var c in textToGradualyAdd)
                    {
                        yield return new WaitForSeconds(secondsToUpdateText);
                        text.text += c;
                    }
                
                    yield return new WaitForSeconds(secondsToUpdateText);
                    text.text = "";
                }
                yield return null;
            }
            
            StartCoroutine(UpdateTextRoutine());
        }
    }
}