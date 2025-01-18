using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Infractructure.Interfaces;

namespace Assets.TestProject.Scripts.Infractructure
{
    public interface IRemoteInfoLoader : IService
    {
        UniTask<T> LoadAsync<T>(string id) where T : class;
    }
}