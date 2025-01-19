using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Infractructure.Interfaces;
using System.Threading;

namespace Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces
{
    public interface IRemoteInfoLoader : IService
    {
        UniTask<T> LoadAsync<T>(string id, CancellationToken token = default) where T : class;
    }
}