using System;

namespace Puzzle15
{
	[Serializable]
	public abstract class TileData
	{
		
	}

	public class NumberedTileData : TileData
	{
		private int _value;

		public int Value => _value;
		
		public NumberedTileData(int value) 
		{
			_value = value;
		}
	}
}