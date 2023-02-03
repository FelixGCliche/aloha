using UnityEngine;

namespace Game
{
    // David D
    public class MeltingIceSoundController : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioClip meltingSound;
        
        private AudioSource audioSource;
        private TemperatureStats temperatureStats;
        private float lastTemp;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = meltingSound;
            audioSource.loop = true;
            temperatureStats = GetComponentInParent<TemperatureStats>();
            lastTemp = temperatureStats.Temperature;
        }

        private void Update()
        {
            if(temperatureStats.Temperature <= lastTemp)
            {
                audioSource.Stop();
            }
            else if (temperatureStats.Temperature > lastTemp &&!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(meltingSound);
            }

            lastTemp = temperatureStats.Temperature;
        }
    }
}