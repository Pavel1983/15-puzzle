using System;
using System.Linq;
using UnityEngine;

namespace Puzzle15.UI
{
	[Serializable]
	public struct TileViewDescription
	{
		public static TileViewDescription Null = new TileViewDescription();
		public TileType Type;
		public TileView View;
	}

	[CreateAssetMenu]
	public class TilesViewsDescriptionSO: ScriptableObject
	{
		public TileViewDescription[] Descriptions;

		public TileView GetView(TileType type)
		{
			var description = Descriptions.FirstOrDefault(item => item.Type == type);
			if (description.Equals(TileViewDescription.Null))
				Debug.LogError("TilesViewsDescriptionSO.GetView == null");

			return description.View;
		}
	}
}