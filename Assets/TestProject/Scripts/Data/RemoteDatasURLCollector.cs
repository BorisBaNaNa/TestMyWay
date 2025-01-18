using UnityEngine;

namespace Assets.TestProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewRemoteDatasURLCollector", menuName = "Configs/RemoteDatasURLCollector")]
    public class RemoteDatasURLCollector : ScriptableObject
    {
        [field: SerializeField] public string GameSettingsURL { get; private set; }
        [field: SerializeField] public string HelloMessageURL { get; private set; }
    }
}