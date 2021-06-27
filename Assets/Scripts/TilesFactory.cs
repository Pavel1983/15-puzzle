using System;
using System.Collections.Generic;
using Puzzle15.UI;
using UnityEngine;

namespace Puzzle15
{
	public interface ITilesFactory<TTilesArgs>
	{
		TileData Create(TTilesArgs args);
	}

	public class NumberedTilesFactory : ITilesFactory<int>
	{
		public TileData Create(int number)
		{
			return new NumberedTileData(number);
		}
	}

	public class ImagedTilesFactory : ITilesFactory<Sprite>
	{
		public TileData Create(Sprite arg)
		{
			throw new NotImplementedException();
		}
	}

//	public class TilesFactory
//	{
//		private static TilesFactory _instance;
//		private Dictionary<string, TileType> _id2typeDict = new Dictionary<string, TileType>();
//
//		private TilesFactory()
//		{
//		}
//
//		public static TilesFactory Instance
//		{
//			get
//			{
//				if (_instance == null)
//					_instance = new TilesFactory();
//
//				return _instance;
//			}
//		}
//
//		public void Setup(List<TileSO> tilesList)
//		{
//			for (int i = 0; i < tilesList.Count; ++i)
//			{
//				TileType tileType = GetTypeBySO(tilesList[i]);
//				_id2typeDict.Add(tilesList[i].Id, tileType);
//			}
//		}
//
//		public TileData Create(string id)
//		{
//			if (_id2typeDict.TryGetValue(id, out var tileType))
//			{
//				switch (tileType)
//				{
//					case TileType.Numbered:
//						return new NumberedTileData(id);
//                    
//					case TileType.Imaged:
//						Debug.LogError("Can't create a tile. Imaged tile type is not implemented yet");
//						return null;
//                    
//					case TileType.Unknown:
//						Debug.LogError("Can't create a tile. Unknown tile type.");
//						break;
//					
//					default:
//						throw new ArgumentOutOfRangeException();
//				}
//			}
//			else
//			{
//				Debug.LogError($"Can't find data for tile with id == {id}");
//			}
//
//			return null;
//		}
//
//		private TileType GetTypeBySO(TileSO tileData)
//		{
//			if ((tileData as NumbersTileSO) != null)
//				return TileType.Numbered;
//
////            if ((tileData as ImagedTileSO) != null)
////                return TileType.Imaged;
//
//			return TileType.Unknown;
//		}
//	}
}