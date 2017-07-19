using CatFight.Util;

using UnityEngine;

namespace CatFight
{
    public sealed class LoadingManager : SingletonBehavior<LoadingManager>
    {
        [SerializeField]
        private LoadingScreen _loadingScreen;

        private void Start()
        {
            CreateManagers();

// TODO: when the actual flow is worked out, the loading manager will destroy itself
            //Destroy();
        }

        private void CreateManagers()
        {
// TODO: AirConsole manager should be last so that its OnReady fires last
// this does mean we don't gain time by loading shit in parallel, buuuuuut it's safest
// because it prevents connections until we're ready for them
        }

        public void Destroy()
        {
            Destroy(_loadingScreen.gameObject);
            _loadingScreen = null;

            Destroy(gameObject);
        }
    }
}
