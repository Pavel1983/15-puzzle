using Puzzle15.Constants;
using Puzzle15.UI;
using UnityEngine;

namespace Puzzle15
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            InitFactories();
            LoadTilesDataProviders();
            
            // Временная подпорка пока не появится новый режим игры
            PlayerPrefs.SetInt(GameConstants.PrefsTileType, (int)TileType.Numbered);
            PlayerPrefs.SetString(GameConstants.PrefsTilesProviderId, GameConstants.PrefsValueNumberedTiles);
            PlayerPrefs.Save();
        }

        private static void InitFactories()
        {
            GameFactory gameFactory = GameFactory.Instance;
        }

        private static void LoadTilesDataProviders()
        {
            var providers = Resources.LoadAll(GameConstants.ResPathProviders);
            if (providers != null)
            {
                var sourceProvider = TilesSourceProvider.Instance;
                for (int i = 0; i < providers.Length; ++i)
                {
                    ITilesMapping provider = (ITilesMapping) providers[i];
                    if (provider != null)
                    {
                        sourceProvider.RegisterProvider(provider.Id, provider);
                    }
                }
            }
            else
            {
                Debug.LogError("Can't find anything in Resources/" + GameConstants.ResPathProviders);
            }
        }
    }
}
