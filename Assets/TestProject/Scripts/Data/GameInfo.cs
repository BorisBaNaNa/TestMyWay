using System;
using Newtonsoft.Json;

namespace Assets.TestProject.Scripts.Data
{
    [Serializable]
    public class GameInfo
    {
        [JsonProperty] public GameSettings GameSettings { get; set; }
        [JsonProperty] public CounterInfo CounterInfo { get; set; }
    }
}