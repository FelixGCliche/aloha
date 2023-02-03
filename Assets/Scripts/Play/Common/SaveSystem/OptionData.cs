using UnityEngine;

namespace Game
{
    // Author: David D
    public class OptionData
    {
        // Public because of JSON
        public int volume;
        public bool fullscreen;
        public int width;
        public int height;
        public int refreshRate;

        public OptionData()
        {
            volume = 0;
            fullscreen = true;
            width = Screen.currentResolution.width;
            height = Screen.currentResolution.height;
            refreshRate = Screen.currentResolution.refreshRate;
        }
    }
}