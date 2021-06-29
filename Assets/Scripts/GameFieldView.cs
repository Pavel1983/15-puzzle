using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Puzzle15.UI
{
	public class GameFieldView : MonoBehaviour
	{
		[SerializeField] private float _tileMoveTime;
		[SerializeField] private float _shuffleTime;
		[SerializeField] private float _padding; // В процентах от ширины тайла
		[SerializeField] private SpriteRenderer _fieldBackground;
		
		private GameField _model;
		private TileView[] _tileObjects;
		private float _paddingSize;
		private Vector2 _firstTilePos;
		private Vector2 _tileWorldSize = Vector2.zero;
		private int _emptyTileIndex;
		private Camera _mainCamera;

		public void Setup(GameField model, TileView tilePrefab)
		{
			_model = model;
			_model.EventTileMove += OnTileMove;
			_model.EventShuffle += OnFieldShuffle;

			_emptyTileIndex = _model.Cols * _model.Rows - 1;

			_paddingSize = _padding * _fieldBackground.bounds.size.x / 100.0f;

			_mainCamera = Camera.main;
			
			CreateFieldView(tilePrefab);
			CreateInputArea();

		}

		private void CreateFieldView(TileView tilePrefab)
		{
			var tilesData = _model.GetTilesData();
			_tileObjects = new TileView[tilesData.Length];
			_tileWorldSize = CalculateTileWorldSize();
			
			var fieldTransform = _fieldBackground.transform;
			var fieldScale = fieldTransform.localScale;

			fieldTransform.localScale = 
				new Vector3(
					fieldScale.x, 
					((_tileWorldSize.x * _model.Rows + _paddingSize * (_model.Cols + 1)) / _fieldBackground.bounds.size.y) * fieldScale.y, 
					1);

			var fieldWorldSize = _fieldBackground.bounds.size;
			_firstTilePos = fieldTransform.position - 
			                       new Vector3(fieldWorldSize.x, -fieldWorldSize.y) * 0.5f +
			                       new Vector3(_tileWorldSize.x, -_tileWorldSize.y, 0) * 0.5f + 
			                       new Vector3(_paddingSize, -_paddingSize, 0);
			
			for (int i = 0; i < tilesData.Length - 1; ++i)
			{
				var tileViewObject = Instantiate(tilePrefab, transform, false);
				
				tileViewObject.Setup(tilesData[i]);
				tileViewObject.ScaleToSize(_tileWorldSize);
				tileViewObject.transform.position = GetTileWorldPosByIndex(i);

				_tileObjects[i] = tileViewObject;
			}
		}

		private void CreateInputArea()
		{
			var fieldWorldSize = _fieldBackground.bounds.size;
			
			var inputAreaObject = new GameObject();
			inputAreaObject.transform.SetParent(transform);
			inputAreaObject.transform.localPosition = Vector3.zero;
			
			var collider = inputAreaObject.AddComponent<BoxCollider2D>();
			collider.size = fieldWorldSize;

			var touchDetector = inputAreaObject.AddComponent<TouchDetector>();
			touchDetector.EventClick += OnInputAreaTouched;
		}

		private void OnInputAreaTouched(PointerEventData eventData)
		{
			Debug.Log($"touched.pos:{eventData.position}");
			
			var clickPos = eventData.position;
			var fieldWorldSize = _fieldBackground.bounds.size;
			Vector2 inputAreaUpperLeftCornerPos =_mainCamera.WorldToScreenPoint(transform.position - 
			                                                                    new Vector3(fieldWorldSize.x, -fieldWorldSize.y) * 0.5f);
			var localScreenPos = clickPos - inputAreaUpperLeftCornerPos;
			
			var fieldScreenSize = _mainCamera.WorldToScreenPoint(fieldWorldSize) - 
			                      new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
			
			float cellSize = fieldScreenSize.x / _model.Cols;
			Vector2Int logicalPos = new Vector2Int(
				(int)(localScreenPos.x / cellSize), 
				-(int)(localScreenPos.y / cellSize));
			int cellIndex = logicalPos.y * _model.Cols + logicalPos.x;
			
			Debug.Log($"logicalPos:{logicalPos}");
			_model.MoveTile(logicalPos);
		}

		private Vector3 GetTileWorldPosByIndex(int index)
		{
			Assert.IsTrue(_tileWorldSize != Vector2.zero);
			
			int x = index % _model.Cols;
			int y = index / _model.Cols;

			return new Vector3(
				_firstTilePos.x + x * (_tileWorldSize.x + _paddingSize),
				_firstTilePos.y - (y * (_tileWorldSize.y + _paddingSize)),
				0);
		}

		private void OnTileMove(int fromPos, int toPos)
		{
			StartCoroutine(MoveTile(fromPos, toPos, _tileMoveTime, null));
		}
		
		private void OnFieldShuffle(List<int> shuffleData)
		{
			StartCoroutine(Shuffle(shuffleData));
		}

		private IEnumerator Shuffle(List<int> shuffleData)
		{
			Debug.Log(ListToString(shuffleData));
			float tileMoveDuration = _shuffleTime / shuffleData.Count;
			for (int i = 0; i < shuffleData.Count; ++i)
			{
				Debug.Log($"{_emptyTileIndex}->{shuffleData[i]}");
				yield return MoveTile(shuffleData[i], _emptyTileIndex, tileMoveDuration, null);

				_emptyTileIndex = shuffleData[i];
			}
		}

		private string ListToString(List<int> shuffleData)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < shuffleData.Count; ++i)
			{
				sb.Append(shuffleData[i].ToString());
				if (i != shuffleData.Count - 1)
					sb.Append(",");
			}

			return sb.ToString();
		}

		private IEnumerator MoveTile(int from, int to, float duration, Action onFinished)
		{
			var currentTile = _tileObjects[from];
			Assert.IsTrue(currentTile != null);

			var tileTransform = currentTile.transform;
			var startPos = tileTransform.position;
			var destPos = GetTileWorldPosByIndex(to);
			var dir = destPos - startPos;
			
			float timer = 0.0f;
			while (timer < duration)
			{
				timer += Time.deltaTime;
				float normalizedTime = timer / duration;

				tileTransform.position = startPos + normalizedTime * dir;
				yield return null;
			}

			tileTransform.position = destPos;
			
			_tileObjects[from] = _tileObjects[to];
			_tileObjects[to] = currentTile;
			
			onFinished?.Invoke();
		}

		private Vector2 CalculateTileWorldSize()
		{
			Vector2 tileSize = Vector2.zero;

			var gameFieldWorldSize =_fieldBackground.bounds.size;
			
			tileSize.x = (gameFieldWorldSize.x - _paddingSize * (_model.Cols + 1)) / _model.Cols;
			tileSize.y = tileSize.x; 

			return tileSize;
		}
	}
}