using UnityEngine;

namespace Game
{
    // Félix B
    public class DashSensor: MonoBehaviour
    {
        private ISensor<IDashDestroyable> dashDestroyableSensor2D;

        public ISensor<IDashDestroyable> DashDestroyableSensor2D => dashDestroyableSensor2D;

        private void Awake()
        {
            dashDestroyableSensor2D = gameObject.GetComponent<TriggerSensor2D>().For<IDashDestroyable>();
        }
    }
}