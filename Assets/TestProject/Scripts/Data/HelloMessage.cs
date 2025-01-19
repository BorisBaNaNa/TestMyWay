using Newtonsoft.Json;

namespace Assets.TestProject.Scripts.Data
{
    public class HelloMessage
    {
        [JsonProperty] public string Message { get; private set; }

        public HelloMessage(string message)
        {
            Message = message;
        }
    }
}