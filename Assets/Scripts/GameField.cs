using System;
using System.Collections.Generic;
using Puzzle15.UI;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Puzzle15
{
    public class GameField
    {
        public event Action<int, int> EventTileMove; // from, to
        public event Action<List<int>> EventShuffle;
        
        
        // locals
        private TileData[] _tiles;
        private int _emptyTileIndex;
        private int _tilesCount;

        private int _cols;
        private int _rows;
        private int _shuffleIterations;

        // properties
        public int TilesCount => _tilesCount;
        public int Cols => _cols;
        public int Rows => _rows;
        
        public GameField(int cols, int rows, TileType tileType, ITilesMapping tilesMapping)
        {
            Assert.IsTrue(cols > 0);
            Assert.IsTrue(rows > 0);

            _cols = cols;
            _rows = rows;
            _tilesCount = cols * rows;
            _shuffleIterations = _tilesCount * 5;

            switch (tileType)
            {
                case TileType.Numbered:
                    _tiles = CreateNumberedTiles(tilesMapping);
                    break;
                case TileType.Imaged:
                    _tiles = CreateImagedTiles(tilesMapping);
                    break;
                case TileType.Unknown:
                    Debug.LogError($"It seems like tileType is unknown. Check TileType");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
            }

            _emptyTileIndex = _tilesCount - 1;
            
            Assert.IsTrue(_tiles != null);
        }

        public TileData[] GetTilesData()
        {
            return _tiles;
        }

        public void Shuffle()
        {
            Debug.Log("Shuffle");
            List<int> shuffleListPos = new List<int>();
            for (int i = 0; i < _shuffleIterations; ++i)
            {
                Debug.Log($"emptyTileIndex = {_emptyTileIndex} before");
                int newTileIndex = GetRandomNeighbourToEmptyTile();
                int x = newTileIndex % _cols;
                int y = newTileIndex / _cols;
                
                MoveTile(new Vector2Int(x, y));
                Debug.Log($"emptyTileIndex = {_emptyTileIndex} after");

                
                shuffleListPos.Add(newTileIndex);
            }
            
            EventShuffle?.Invoke(shuffleListPos);
        }

        public void MoveTile(Vector2Int pos)
        {
            if (TryMoveTile(pos))
            {
                int movingTileIndex = pos.y * _cols + pos.x;
                EventTileMove?.Invoke(movingTileIndex, _emptyTileIndex);
            }
        }

        private int GetRandomNeighbourToEmptyTile()
        {
            int x = _emptyTileIndex % _cols;
            int y = _emptyTileIndex / _cols;
            
            List<int> choices = new List<int>();
            
            if (x == 0)
                choices.Add(_emptyTileIndex + 1);
            else if (x == _cols - 1)
                choices.Add(_emptyTileIndex - 1);
            else
            {
                choices.Add(_emptyTileIndex + 1);
                choices.Add(_emptyTileIndex - 1);
            }

            if (y == 0)
                choices.Add(_emptyTileIndex + _cols);
            else if (y == _cols - 1)
                choices.Add(_emptyTileIndex - _cols);
            else
            {
                choices.Add(_emptyTileIndex + _cols);
                choices.Add(_emptyTileIndex - _cols);
            }
            
            return choices[Random.Range(0, choices.Count)];
        }

        private bool TryMoveTile(Vector2Int pos)
        {
            Assert.IsTrue(pos.x >= 0 && pos.x < _cols);
            Assert.IsTrue(pos.y >= 0 && pos.y < _rows);
            
            int emptyTileX = _emptyTileIndex % _cols;
            int emptyTileY = _emptyTileIndex / _cols;

            int movingTileIndex = pos.y * _cols + pos.x;
            
            int movingTileX = movingTileIndex % _cols;
            int movingTileY = movingTileIndex / _cols;

            int deltaX = Mathf.Abs(emptyTileX - movingTileX);
            int deltaY = Mathf.Abs(emptyTileY - movingTileY);
            if ((deltaX == 1 && deltaY == 0) ||
                (deltaX == 0 && deltaY == 1))
            {
                SwapTiles(ref _emptyTileIndex, ref movingTileIndex);
                return true;
            }

            return false;
        }

        private TileData[] CreateNumberedTiles(ITilesMapping tilesMapping)
        {
            TileData[] tiles = null;
            int[] orderedTileset = tilesMapping.OrderedTilesContent as int[];
            ITilesFactory<int> tilesFactory = GameFactory.Instance.GetNumberedTileFactory();
            if (orderedTileset != null)
            {
                tiles = new TileData[_tilesCount];
                for (int i = 0; i < _tilesCount - 1; ++i)
                {
                    int tileContentValue = orderedTileset[i];
                    tiles[i] = tilesFactory.Create(tileContentValue);
                }
            }

            return tiles;
        }

        private TileData[] CreateImagedTiles(ITilesMapping tilesMapping)
        {
            TileData[] tiles = null;
            Sprite[] orderedTileset = tilesMapping.OrderedTilesContent as Sprite[];
            ITilesFactory<Sprite> imagedTilesFactory = GameFactory.Instance.GetImagedTileFactory();
            if (orderedTileset != null)
            {
                tiles = new TileData[_tilesCount];
                for (int i = 0; i < _tilesCount - 1; ++i)
                {
                    Sprite tileContentValue = orderedTileset[i];
                    tiles[i] = imagedTilesFactory.Create(tileContentValue);
                }
            }

            return tiles;
        }

        private void SwapTiles(ref int tile1, ref int tile2)
        {
            var firstTile =_tiles[tile1];
            _tiles[tile1] = _tiles[tile2];
            _tiles[tile2] = firstTile;
            
            int tmpTile = tile1;
            tile1 = tile2;
            tile2 = tmpTile;
        }
    }
}
