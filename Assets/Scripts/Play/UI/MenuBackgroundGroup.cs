using UnityEngine;

namespace Game
{
    // LouisRD
    public class MenuBackgroundGroup : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 30;
        [SerializeField] private float gameObjectPosX;

        private GameObject parentCanvas;

        private void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>().gameObject;
        }

        private void Update()
        {
            gameObjectPosX = gameObject.transform.position.x;
            if (gameObjectPosX <= 0)
                gameObject.transform.position +=
                    new Vector3(transform.position.x + parentCanvas.transform.position.x * 2, 0, 0);
            else
                gameObject.transform.position += new Vector3(-1 * scrollSpeed * Time.deltaTime, 0, 0);
        }
    }
}