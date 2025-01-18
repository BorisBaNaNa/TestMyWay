using System;
using Newtonsoft.Json;

namespace Assets.TestProject.Scripts.Data
{
    [Serializable]
    public class GameSettings
    {
        [JsonProperty] public int StartingNumber { get; private set; } = 0;
    }

    [Serializable]
    public class GameInfo
    {
        [JsonProperty] public GameSettings GameSettings { get; set; }
        [JsonProperty] public CounterInfo CounterInfo { get; set; }
    }

    [Serializable]
    public class CounterInfo
    {
        [JsonProperty] public int LastCount { get; private set; }

        public CounterInfo(int count)
        {
            LastCount = count;
        }
    }
}