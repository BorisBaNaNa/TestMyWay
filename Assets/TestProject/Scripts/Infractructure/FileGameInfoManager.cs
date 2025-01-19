using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Data;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using Assets.TestProject.Scripts.Infractructure.Interfaces;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class FileGameInfoManager : IGameInfoManager
    {
        public GameInfo LoadedInfo => _gameInfo;

        private GameInfo _gameInfo;
        private readonly string _filePath;

        public FileGameInfoManager()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "SavedData", "gameInfo.json");
        }

        public async UniTask<GameInfo> LoadGameInfoAsync(CancellationToken token = default)
        {
            if (File.Exists(_filePath))
            {
                string json = await File.ReadAllTextAsync(_filePath, token).AsUniTask();
                _gameInfo = JsonConvert.DeserializeObject<GameInfo>(json);
            }
            else
            {
                string directoryPath = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                _gameInfo = new GameInfo
                {
                    GameSettings = new GameSettings(),
                };
            }

            return _gameInfo;
        }

        public async UniTask SaveGameInfo(GameInfo gameInfo = null, CancellationToken token = default)
        {
            if (gameInfo == null)
                gameInfo = LoadedInfo;
            if (gameInfo == null)
                return;

            _gameInfo = gameInfo;

            string json = JsonConvert.SerializeObject(_gameInfo, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json, token).AsUniTask();
        }

        public void Dispose()
        {
            // Implement disposal logic if needed
        }
    }
}