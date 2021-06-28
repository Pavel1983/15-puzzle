using UnityEngine;
using UnityEngine.Assertions;

namespace Puzzle15.UI
{
    public class MainWindow : MonoBehaviour
    {
        [SerializeField] private TilesViewsDescriptionSO _tilesViewsDescription;
        [SerializeField] private TilesMappingSO _tilesMapping;
        [SerializeField] private GameFieldView _gameFieldViewPrefab;

        private GameField _gameField;
        private GameFieldView _gameFieldView;

        #region life cycle

        private void Start()
        {
            Assert.IsTrue(_tilesViewsDescription != null);
            Assert.IsTrue(_tilesMapping != null);
            Assert.IsTrue(_gameFieldViewPrefab != null);
            
            // Для теста
            TileType type = TileType.Numbered;
            int cols = 3;
            int rows = 3;

            _gameField = new GameField(cols, rows, type, _tilesMapping);
            _gameFieldView = Instantiate(_gameFieldViewPrefab);
            _gameFieldView.Setup(_gameField, _tilesViewsDescription.GetView(type));
            
            _gameField.Shuffle();
        }

        #endregion
    }
}