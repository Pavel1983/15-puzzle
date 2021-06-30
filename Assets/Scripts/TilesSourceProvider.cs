using System.Collections.Generic;

namespace Puzzle15
{
	public class TilesSourceProvider
	{
		private static TilesSourceProvider _instance = new TilesSourceProvider();
        
		private Dictionary<string, ITilesMapping> _sourceProviders = new Dictionary<string, ITilesMapping>();

		private TilesSourceProvider()
		{
		}

		public static TilesSourceProvider Instance => _instance;

		public void RegisterProvider(string id, ITilesMapping provider)
		{
			_sourceProviders.Add(id, provider);
		}

		public ITilesMapping GetProvider(string id)
		{
			return _sourceProviders[id];
		}
	}
}