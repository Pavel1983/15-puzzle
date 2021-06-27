using UnityEngine;

namespace Puzzle15
{
	public class GameFactory
	{
		private static GameFactory _instance = null;
		private ITilesFactory<int> _numberedTilesFactory = null;
		private ITilesFactory<Sprite> _imagedTilesFactory = null;
        
		private GameFactory()
		{
		}

		public static GameFactory Instance
		{
			get
			{
				if (_instance == null)
					_instance = new GameFactory();

				return _instance;
			}
		}

		public ITilesFactory<int> GetNumberedTileFactory()
		{
			if (_numberedTilesFactory == null)
			{
				_numberedTilesFactory = new NumberedTilesFactory();
			}

			return _numberedTilesFactory;
		}

		public ITilesFactory<Sprite> GetImagedTileFactory()
		{
			if (_imagedTilesFactory == null)
			{
				_imagedTilesFactory = new ImagedTilesFactory();
			}

			return _imagedTilesFactory;
		}
	}
}