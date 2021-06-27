using UnityEngine;

namespace Puzzle15
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            InitFactories();
        }

        private static void InitFactories()
        {
            GameFactory gameFactory = GameFactory.Instance;
        }
    }
}
