using System;
using System.Linq;
using Puzzle15.Constants;
using PuzzleGame.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Puzzle15.UI
{
    public class MainScreen : BaseScreen
    {
        [SerializeField] private Button _buttonBack;
        
        [SerializeField] private TilesViewsDescriptionSO _tilesViewsDescription;
        [SerializeField] private TilesMappingSO _tilesMapping;
        [SerializeField] private GameFieldView _gameFieldViewPrefab;

        private GameField _gameField;
        private GameFieldView _gameFieldView;

        #region life cycle

        private void OnEnable()
        {
            _buttonBack.onClick.AddListener(OnButtonBack);
        }

        private void OnDisable()
        {
            _buttonBack.onClick.RemoveListener(OnButtonBack);

        }
        
        private void Start()
        {
            Assert.IsTrue(_tilesViewsDescription != null);
            Assert.IsTrue(_tilesMapping != null);
            Assert.IsTrue(_gameFieldViewPrefab != null);
        }

        private void OnDestroy()
        {
            SaveSession();
        }

        #endregion

        public override void Show()
        {
            base.Show();

            string lastSessionJson = PlayerPrefs.GetString(GameConstants.PrefsLastSession, "");
            if (!string.IsNullOrEmpty(lastSessionJson))
            {
                SessionData sessionData = JsonUtility.FromJson<SessionData>(lastSessionJson);
                if (sessionData.Cols == 0)
                    CreateNewGameField(_tilesMapping);
                else
                    LoadGameFromPreviousSession(sessionData);
            }
            else 
                CreateNewGameField(_tilesMapping);
        }

        private void LoadGameFromPreviousSession(SessionData sessionData)
        {
            ITilesMapping currentProvider = TilesSourceProvider.Instance.GetProvider(sessionData.TilesProviderId);
            Assert.IsTrue(currentProvider != null);
            
            var orderedTiles = CreateTiles(sessionData.Type, sessionData.Cols * sessionData.Rows, currentProvider);

            CreateGameField(
                sessionData.Cols, 
                sessionData.Rows, 
                sessionData.Type, 
                orderedTiles,
                sessionData.TilesIndices);
        }

        private void SaveSession()
        {
            string currentTilesSourceProviderId = PlayerPrefs.GetString(GameConstants.PrefsTilesProviderId);

            SessionData sessionData = new SessionData();
            sessionData.Cols = _gameField.Cols;
            sessionData.Rows = _gameField.Rows;
            sessionData.Type = TileType.Numbered; // todo: fix later
            sessionData.TilesProviderId = currentTilesSourceProviderId;
            sessionData.TilesIndices = GetTileIndices(_gameField.GetOrderedTileData(), _gameField.GetTilesData());
            sessionData.Save();
        }

        private int[] GetTileIndices(TileData[] orderedTiles, TileData[] tiles)
        {
            var orderedTileList = orderedTiles.ToList();
            int[] tilesIndices = new int[tiles.Length];
            for (int i = 0; i < tiles.Length; ++i)
            {
                tilesIndices[i] = orderedTileList.IndexOf(tiles[i]);
            }

            return tilesIndices;
        }

        private void CreateNewGameField(ITilesMapping tilesMapping)
        {
            int cols = PlayerPrefs.GetInt(GameConstants.PrefsCols, 0);
            int rows = PlayerPrefs.GetInt(GameConstants.PrefsRows, 0);
            TileType tileType = (TileType)PlayerPrefs.GetInt(GameConstants.PrefsTileType, 0);
            TileData[] orderedTiles = CreateTiles(tileType, cols * rows, tilesMapping);

            CreateGameField(cols, rows, tileType, orderedTiles);
            
            _gameField.Shuffle();
        }

        private void CreateGameField(int cols, int rows, TileType type, TileData[] orderedTiles, int[] customTilesOrder = null)
        {
            Assert.IsTrue(cols > 0);
            Assert.IsTrue(rows > 0);
            Assert.IsTrue(orderedTiles != null);
            
            if (_gameField != null) 
                _gameField = null;
            
            if (_gameFieldView != null)
            {
                Destroy(_gameFieldView.gameObject);
                _gameFieldView = null;
            }
            
            _gameField = new GameField(cols, rows, orderedTiles, customTilesOrder);
            _gameFieldView = Instantiate(_gameFieldViewPrefab);
            _gameFieldView.Setup(_gameField, _tilesViewsDescription.GetView(type));
            _gameField.EventPuzzleCompleted += OnPuzzleCompleted;
        }

        private TileData[] CreateTiles(TileType type, int tilesCount, ITilesMapping tilesMapping)
        {
            TileData[] tilesOrdered = null; 
            switch (type)
            {
                case TileType.Numbered:
                    tilesOrdered = CreateNumberedTiles(tilesMapping, tilesCount);
                    break;
                case TileType.Imaged:
                    tilesOrdered = CreateImagedTiles(tilesMapping, tilesCount);
                    break;
                case TileType.Unknown:
                    Debug.LogError($"It seems like tileType is unknown. Check TileType");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return tilesOrdered;
        }

        private TileData[] CreateNumberedTiles(ITilesMapping tilesMapping, int tilesCount)
        {
            TileData[] tiles = null;
            int[] orderedTileset = tilesMapping.OrderedTilesContent as int[];
            ITilesFactory<int> tilesFactory = GameFactory.Instance.GetNumberedTileFactory();
            if (orderedTileset != null)
            {
                tiles = new TileData[tilesCount];
                for (int i = 0; i < tilesCount - 1; ++i)
                {
                    int tileContentValue = orderedTileset[i];
                    tiles[i] = tilesFactory.Create(tileContentValue);
                }
            }

            return tiles;
        }
        private TileData[] CreateImagedTiles(ITilesMapping tilesMapping, int tilesCount)
        {
            TileData[] tiles = null;
            Sprite[] orderedTileset = tilesMapping.OrderedTilesContent as Sprite[];
            ITilesFactory<Sprite> imagedTilesFactory = GameFactory.Instance.GetImagedTileFactory();
            if (orderedTileset != null)
            {
                tiles = new TileData[tilesCount];
                for (int i = 0; i < tilesCount - 1; ++i)
                {
                    Sprite tileContentValue = orderedTileset[i];
                    tiles[i] = imagedTilesFactory.Create(tileContentValue);
                }
            }

            return tiles;
        }

        #region event handlers

        private void OnPuzzleCompleted()
        {
            ScreensSwitcher.Instance.Open(GameConstants.ScreenWin);
        }
        
        private void OnButtonBack()
        {
            (new SessionData()).Save();
            ScreensSwitcher.Instance.Back();
        }


        #endregion
    }
}