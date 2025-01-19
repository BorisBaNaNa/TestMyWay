using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Data;
using System.Threading;

namespace Assets.TestProject.Scripts.Infractructure.Interfaces
{
    public interface IGameInfoManager : IService
    {
        GameInfo LoadedInfo { get; }

        UniTask<GameInfo> LoadGameInfoAsync(CancellationToken token = default);
        UniTask SaveGameInfo(GameInfo gameInfo = null, CancellationToken token = default);
    }
}