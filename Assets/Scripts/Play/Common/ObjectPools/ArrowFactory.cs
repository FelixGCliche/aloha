using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David D
    [Findable(Tags.ObjectPool)]
    public class ArrowFactory : MonoBehaviour
    {
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private bool shouldExpand = true;
        [SerializeField] [Min(1)] private int numberOfArrowsInPool = 8;

        private List<GameObject> arrowPool;

        private void Awake()
        {
            arrowPool = new List<GameObject>();

            for (var i = 0; i < numberOfArrowsInPool; i++)
                CreateArrow();
        }

        private void OnDestroy()
        {
            foreach (var arrow in arrowPool) Destroy(arrow);
        }

        private GameObject CreateArrow()
        {
            var obj = Instantiate(arrowPrefab, transform, true);
            obj.SetActive(false);
            arrowPool.Add(obj);
            return obj;
        }

        public GameObject GetNextAvailableArrow(Quaternion rotation, Vector3 position, Vector2 spawnOffset, Vector2 velocity)
        {
            GameObject arrowToGive = null;
            arrowToGive = arrowPool.FirstOrDefault(arrow => !arrow.activeInHierarchy);

            if (arrowToGive == null)
            {
                if (shouldExpand)
                    arrowToGive = CreateArrow();
                else
                    return null;
            }

            arrowToGive.SetActive(true);
            arrowToGive.transform.rotation = rotation;
            arrowToGive.transform.position = position;
            arrowToGive.transform.Translate(spawnOffset);
            Rigidbody2D arrowRigidbody = arrowToGive.GetComponent<Rigidbody2D>();
            arrowRigidbody.velocity = velocity;

            return arrowToGive;
        }
    }
}