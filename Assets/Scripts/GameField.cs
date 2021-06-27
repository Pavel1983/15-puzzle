using System;
using Puzzle15.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Puzzle15
{
    public interface ITile<TTileArgs>
    {
        void Init(TTileArgs args);
    }

    public class NumberedTile : ITile<int>
    {
        private int _number;
        
        public void Init(int number)
        {
            _number = number;
        }
    }
    
    public class GameField
    {
        public event Action<int, int> EventTileMove; // from, to
        
        // locals
        private TileData[] _tiles;
        private int _emptyTileIndex;
        private int _tilesCount;

        private int _cols;
        private int _rows;

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

        public void MoveTile(Vector2Int pos)
        {
            Assert.IsTrue(pos.x >= 0 && pos.x < _cols);
            Assert.IsTrue(pos.y >= 0 && pos.y < _rows);
            
            int emptyTileX = _emptyTileIndex % _tilesCount;
            int emptyTileY = _emptyTileIndex / _tilesCount;

            int movingTileIndex = pos.y * _cols + pos.x;
            
            int movingTileX = movingTileIndex % _tilesCount;
            int movingTileY = movingTileIndex / _tilesCount;

            int deltaX = Mathf.Abs(emptyTileX - movingTileX);
            int deltaY = Mathf.Abs(emptyTileY - movingTileY);
            if ((deltaX == 1 && deltaY == 0) ||
                (deltaX == 0 && deltaY == 1))
            {
                SwapTiles(ref _emptyTileIndex, ref movingTileIndex);
                EventTileMove?.Invoke(movingTileIndex, _emptyTileIndex);
            }
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
            int tmpTile = tile1;
            tile1 = tile2;
            tile2 = tmpTile;
        }
    }
}
