using System;
using Newtonsoft.Json;

namespace Assets.TestProject.Scripts.Data
{
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