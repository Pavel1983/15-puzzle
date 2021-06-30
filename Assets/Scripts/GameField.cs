using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Puzzle15
{
    public class GameField
    {
        public event Action<int, int> EventTileMove; // fromIndex, toIndex
        public event Action<List<int>> EventShuffle;
        public event Action EventPuzzleCompleted;

        // locals
        private TileData[] _tiles;
        private TileData[] _tilesOrdered;
        
        private int _emptyTileIndex;
        private int _tilesCount;
        private int _cols;
        private int _rows;
        private int _shuffleIterations;
        
        // properties
        public int TilesCount => _tilesCount;
        public int Cols => _cols;
        public int Rows => _rows;
        
        public GameField(int cols, int rows, TileData[] tiles, int[] customOrder = null)
        {
            Assert.IsTrue(cols > 0);
            Assert.IsTrue(rows > 0);

            _cols = cols;
            _rows = rows;
            _tilesCount = cols * rows;
            _shuffleIterations = _tilesCount;

            _tiles = tiles;
            _tilesOrdered = new TileData[_tiles.Length];
            _tiles.CopyTo(_tilesOrdered, 0);

            _emptyTileIndex = _tilesCount - 1;
            
            if (customOrder != null)
                SetCustomOrder(customOrder);
            
            Assert.IsTrue(_tiles != null);
        }

        private void SetCustomOrder(int[] tilesIndices)
        {
            for (int i = 0; i < tilesIndices.Length; ++i)
            {
                _tiles[i] = _tilesOrdered[tilesIndices[i]];
            }
        }

        public TileData[] GetTilesData()
        {
            return _tiles;
        }

        public TileData[] GetOrderedTileData()
        {
            return _tilesOrdered;
        }

        public void Shuffle()
        {
            List<int> shuffleListPos = new List<int>();
            for (int i = 0; i < _shuffleIterations; ++i)
            {
                int newTileIndex = GetRandomNeighbourToEmptyTile();
                int x = newTileIndex % _cols;
                int y = newTileIndex / _cols;
                
                TryMoveTile(new Vector2Int(x, y));

                shuffleListPos.Add(newTileIndex);
            }
            
            EventShuffle?.Invoke(shuffleListPos);
        }

        public void MoveTile(Vector2Int pos)
        {
            int movingTileIndex = pos.y * _cols + pos.x;
            int toPos = _emptyTileIndex;
            if (TryMoveTile(pos))
            {
                EventTileMove?.Invoke(movingTileIndex, toPos);
                if (IsPuzzleCompleted())
                    EventPuzzleCompleted?.Invoke();
            }
        }

        private bool IsPuzzleCompleted()
        {
            for (int i = 0; i < _tilesOrdered.Length; ++i)
            {
                if (_tiles[i] != _tilesOrdered[i])
                    return false;
            }

            return true;
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
