using UnityEngine;

namespace Assets.TestProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewRemoteDatasURLCollector", menuName = "Configs/RemoteDatasURLCollector")]
    public class RemoteDatasURLCollector : ScriptableObject
    {
        public const string GOOGLE_DISK_LOAD_URL = "https://drive.google.com/uc?export=download&id=";

        [field: SerializeField] public string GameSettingsID { get; private set; }
        [field: SerializeField] public string HelloMessageID { get; private set; }
        [field: SerializeField] public string SimplesSceneBundleID { get; private set; }
    }
}