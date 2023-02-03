using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    //Author: William L
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private float secondsToWait = 1;

        private InputActions.GameActions gameInputs;
        private OnDialogueToShowEventChannel onDialogueToShowEventChannel;
        
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;

        private void Awake()
        {
            onDialogueToShowEventChannel = Finder.OnDialogueToShowEventChannel;
            gameInputs = Finder.Inputs.Actions.Game;
            
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
        }

        private void OnEnable()
        {
            onDialogueToShowEventChannel.OnDialogueToShow += OnDialogueToShow;
        }

        private void OnDisable()
        {
            onDialogueToShowEventChannel.OnDialogueToShow -= OnDialogueToShow;
        }

        private void OnDialogueToShow(string dialogue)
        {
            StartCoroutine(ShowDialogue());

            IEnumerator ShowDialogue()
            {
                //Ferme la boite de dialogue si un ancien dialogue était encore actif
                if (dialogueBox.activeSelf)
                    dialogueBox.SetActive(false);
                
                dialogueBox.SetActive(true);
                canvasGroup.alpha = 0;
                var dialogueTextMesh = GetComponentInChildren<TextMeshProUGUI>();
                dialogueTextMesh.text = dialogue;
                yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 1));

                //Laisse le temps au joueur de s'arrêter pour lire
                yield return new WaitForSeconds(secondsToWait);
                //Permet d'enlever le texte dès que le joueur va bouger.
                yield return new WaitUntil(() => gameInputs.Move.triggered || gameInputs.Jump.triggered);
                
                yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
                dialogueBox.SetActive(false);
            }
        }
    }
}