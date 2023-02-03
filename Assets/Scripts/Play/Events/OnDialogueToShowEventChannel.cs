using Harmony;
using UnityEngine;

namespace Game
{
    //Author : William L
    [Findable(Tags.GameController)]
    public class OnDialogueToShowEventChannel : MonoBehaviour
    {
        public event OnDialogueToShowEvent OnDialogueToShow;

        public void Publish(string dialogue)
        {
            OnDialogueToShow?.Invoke(dialogue);
        }
    }
    public delegate void OnDialogueToShowEvent(string dialogue);
}