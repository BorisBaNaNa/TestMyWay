using System;
using Newtonsoft.Json;

namespace Assets.TestProject.Scripts.Data
{
    [Serializable]
    public class GameSettings
    {
        [JsonProperty] public int StartingNumber { get; private set; } = 0;
    }
}