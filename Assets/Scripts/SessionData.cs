using System;

namespace Puzzle15.UI
{
	[Serializable]
	public struct SessionData
	{
		private static SessionData _null = new SessionData();
		public static SessionData Null => _null;
        
		public int Cols;
		public int Rows;
		public TileType Type;
        
		// id "раздавателя" тайлов
		public string TilesProviderId;
		// реальный порядок тайлов (индексы упорядоченного массива)
		public int[] TilesIndices;
	}
}