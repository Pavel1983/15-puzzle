using Puzzle15.Constants;
using PuzzleGame.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle15.UI
{
    public class IntroScreen : BaseScreen
    {
        [SerializeField] private TouchDetector _touchDetector;

        #region life cycle
        private void OnEnable()
        {
            _touchDetector.EventClick += OnTouchScreen;
        }

        private void OnDisable()
        {
            _touchDetector.EventClick -= OnTouchScreen;
        }
        #endregion

        public override void Show()
        {
            base.Show();
            
            string lastSessionJson = PlayerPrefs.GetString(GameConstants.PrefsLastSession, "");
            if (!string.IsNullOrEmpty(lastSessionJson))
            {
                SessionData sessionData = JsonUtility.FromJson<SessionData>(lastSessionJson);
                if (sessionData.Cols != 0)
                {
                    ScreensSwitcher.Instance.Open(GameConstants.ScreenMain);
                }
            }
        }

        private void OnTouchScreen(PointerEventData obj)
        {
            ScreensSwitcher.Instance.Open(GameConstants.ScreenChoice, false);
        }
    }
}
