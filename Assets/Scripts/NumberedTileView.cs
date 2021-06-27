using TMPro;
using UnityEngine;

namespace Puzzle15.UI
{
	public class NumberedTileView : TileView
	{
		[SerializeField] private TextMeshPro _numberText;

		public override void Setup(TileData tileData)
		{
			var data = tileData as NumberedTileData;
			if (data == null)
				Debug.LogError("Incorrect parameters in NumberedTileView.Setup");
			else
				_numberText.text = data.Value.ToString();
		}
	}
}