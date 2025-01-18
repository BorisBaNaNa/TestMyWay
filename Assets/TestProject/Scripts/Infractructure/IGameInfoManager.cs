using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Data;
using Assets.TestProject.Scripts.Infractructure.Interfaces;

namespace Assets.TestProject.Scripts.Infractructure
{
    public interface IGameInfoManager : IService
    {
        GameInfo LoadedInfo { get; }

        UniTask<GameInfo> LoadGameInfoAsync();
        UniTask SaveGameInfo(GameInfo gameInfo = null);
    }
}