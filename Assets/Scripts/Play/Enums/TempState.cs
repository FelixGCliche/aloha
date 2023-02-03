namespace Game
{
    public enum TempState
    {
        Frozen = 0,
        Hot = 1,
        Neutral = 2
    }

    public static class TemperatureExtension
    {
        public static TempState Next(this TempState tempState)
        {
            if (tempState == TempState.Frozen) return TempState.Hot;

            if (tempState == TempState.Hot) return TempState.Frozen;
            //Pas supposé passer par la...
            return TempState.Neutral;
        }

        public static TempState Previous(this TempState tempState)
        {
            if (tempState == TempState.Frozen) return TempState.Hot;

            if (tempState == TempState.Hot) return TempState.Frozen;
            //Pas supposé passer par la...
            return TempState.Neutral;
        }
    }
}