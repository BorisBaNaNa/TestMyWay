
using Assets.TestProject.Scripts.Infractructure.Interfaces;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class AllServices
    {
        private class Implement<TService> where TService : IService
        {
            public static TService Instance;
        }

        public static void RegService<TService>(TService serviceObj) where TService : IService
            => Implement<TService>.Instance = serviceObj;

        public static TService GetService<TService>() where TService : IService
            => Implement<TService>.Instance;

        public static void DisposeService<TService>() where TService : IService
        {
            if (Implement<TService>.Instance == null)
                return;

            Implement<TService>.Instance.Dispose();
            Implement<TService>.Instance = default;
        }
    }
}