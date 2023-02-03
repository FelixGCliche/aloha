using System.Linq;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class MovingPlatformPath : MonoBehaviour
    {
        public Vector3[] Nodes => gameObject.Children().Select(x => x.transform.position).ToArray();
    }
}