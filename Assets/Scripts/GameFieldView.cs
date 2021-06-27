using UnityEngine;

namespace Puzzle15.UI
{
	public class GameFieldView : MonoBehaviour
	{
		private GameField _model;
		private TileView[] _tileObjects;

		public void Setup(GameField model, TileView tilePrefab)
		{
			_model = model;
			_model.EventTileMove += OnTileMove;

			var tilesData = _model.GetTilesData();
			_tileObjects = new TileView[tilesData.Length - 1];
			
			for (int i = 0; i < tilesData.Length - 1; ++i)
			{
				var tileViewObject = Instantiate(tilePrefab, transform, false);
				tileViewObject.Setup(tilesData[i]);
				
				int x = i % _model.Cols;
				int y = i / _model.Cols;
				tileViewObject.transform.position = new Vector3(x, y, 0);

				_tileObjects[i] = tileViewObject;
			}
		}

		private void OnTileMove(int fromPos, int toPos)
		{

		}
	}
}