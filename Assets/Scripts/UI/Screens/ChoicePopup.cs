using System.Collections;
using System.Collections.Generic;
using Puzzle15.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleGame.UI
{
    public class ChoicePopup : BaseScreen
    {
        [SerializeField] private Button _button3x3;
        [SerializeField] private Button _button4x4;
        [SerializeField] private Button _button5x5;

        private void OnEnable()
        {
            _button3x3.onClick.AddListener(On3x3Click);
            _button4x4.onClick.AddListener(On4x4Click);
            _button5x5.onClick.AddListener(On5x5Click);
        }

        private void OnDisable()
        {
            _button3x3.onClick.RemoveListener(On3x3Click);
            _button4x4.onClick.RemoveListener(On4x4Click);
            _button5x5.onClick.RemoveListener(On5x5Click);
        }
        
        #region event handlers
        private void On3x3Click()
        {
            SaveFieldDimensions(3, 3);
            ScreensSwitcher.Instance.Back();
            ScreensSwitcher.Instance.Open(GameConstants.ScreenMain);
        }
        private void On4x4Click()
        {
            SaveFieldDimensions(4, 4);
            ScreensSwitcher.Instance.Back();
            ScreensSwitcher.Instance.Open(GameConstants.ScreenMain);
        }
        private void On5x5Click()
        {
            SaveFieldDimensions(5, 5);
            ScreensSwitcher.Instance.Back();
            ScreensSwitcher.Instance.Open(GameConstants.ScreenMain);
        }

        private void SaveFieldDimensions(int cols, int rows)
        {
            PlayerPrefs.SetInt(GameConstants.PrefsCols, cols);
            PlayerPrefs.SetInt(GameConstants.PrefsRows, rows);
            PlayerPrefs.Save();
        }

        #endregion
    }
}
