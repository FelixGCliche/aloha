using System.Collections;
using UnityEngine;

namespace Game
{
    // Author : Félix B
    public interface IDashDestroyable 
    {
         bool DoorBreakingArtefactNeeded { get;}
         Collider2D Collider { get; }
         IEnumerator DestructionCountDown(float timeBeforeDestruction);
    }
}