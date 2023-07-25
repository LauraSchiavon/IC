using UnityEngine;

namespace JigsawPuzzlesCollection.Scripts
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        private const string c_backgroundPrefKey = "BackgroundPreference";
        private const string c_selectedBoardPrefKey = "SelectedBoardPreference";
        private const int c_show = 1;
        private const int c_hide = -1;

        public Sprite[] Boards;

        public bool ShowBackground()
        {
            return PlayerPrefs.GetInt(c_backgroundPrefKey, c_show) == c_show;
        }

        public void SetShowBackground(bool show)
        {
            PlayerPrefs.SetInt(c_backgroundPrefKey, show ? c_show : c_hide);
        }

        public int SelectedBoard()
        {
            return PlayerPrefs.GetInt(c_selectedBoardPrefKey, 0);
        }

        public void SetSelectedBoard(int index)
        {
            PlayerPrefs.SetInt(c_selectedBoardPrefKey, index);
        }
    }
}