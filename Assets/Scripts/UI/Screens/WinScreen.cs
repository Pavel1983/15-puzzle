using PuzzleGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle15.UI
{
    public class WinScreen : BaseScreen
    {
        [SerializeField] private Button _newGameButton;

        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(OnNewGameButton);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(OnNewGameButton);
        }

        private void OnNewGameButton()
        {
            ScreensSwitcher.Instance.Open("ChoicePopup", false);
        }
    }
}
