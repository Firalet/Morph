/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Media;

namespace Morph
{
    /// <summary>
    /// Handles the current level the user is on, and holds all levels possible.
    /// Also handles the locking and unlocking of locked levels and game settings for
    /// the application.
    /// </summary>
    public class LevelManager : NotifyBase
    {
        #region Properties & Fields

        public const int LockedSet = 4;
        public event EventHandler LevelComplete;
        public event EventHandler AllLevelsComplete;
        public event EventHandler NewLevelLoaded;
        public event EventHandler NotifyUnlock;

        private GameSettings _gameSettings;
        public GameSettings GameSettings
        {
            get { return _gameSettings; }
            set { _gameSettings = value; NotifyPropertyChanged("GameSettings"); }
        }

        private Game _game;
        public Game Game
        {
            get { return _game; }
            set { _game = value; NotifyPropertyChanged("Game"); }
        }

        private Color _startColor;
        public Color StartColor
        {
            get { return _startColor; }
            set { _startColor = value; NotifyPropertyChanged("StartColor"); }
        }

        private Color _endColor;
        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; NotifyPropertyChanged("EndColor"); }
        }

        private string _setTitle;
        public string SetTitle
        {
            get { return _setTitle; }
            set { _setTitle = value; NotifyPropertyChanged("SetTitle"); }
        }

        private int _setIndex;
        public int SetIndex
        {
            get { return _setIndex; }
            set { _setIndex = value; NotifyPropertyChanged("SetIndex"); }
        }

        private int _levelIndex;
        public int LevelIndex
        {
            get { return _levelIndex; }
            set { _levelIndex = value; NotifyPropertyChanged("LevelIndex"); }
        }

        private string _levelTitle;
        public string LevelTitle
        {
            get { return _levelTitle; }
            set { _levelTitle = value; NotifyPropertyChanged("LevelTitle"); }
        }

        private LevelSetCollection _levelSets;
        public LevelSetCollection LevelSets
        {
            get { return _levelSets; }
            set { _levelSets = value; NotifyPropertyChanged("LevelSets"); }
        }

        #endregion

        #region Constructor & Desctructor

        public LevelManager()
        {
            GameSettings = new GameSettings();
            GameSettings.ColorIconSetChanged += GameSettingsChanged;

            //instantiate all levels here
            LevelSets = new LevelSetCollection();
            LevelSets.AddWithIndex(StandardLevelSets.Levels3x3);
            LevelSets.AddWithIndex(StandardLevelSets.Levels4x4);
            LevelSets.AddWithIndex(StandardLevelSets.Levels5x5);
            LevelSets.AddWithIndex(StandardLevelSets.Levels6x6);
            LevelSets.AddWithIndex(StandardLevelSets.LevelsUnlockable); //4

            LoadNewLevel(GameSettings.CurrentLevelIndex, GameSettings.CurrentSetIndex);
        }

        ~LevelManager()
        {
            GameSettings.ColorIconSetChanged -= GameSettingsChanged;
        }

        #endregion

        #region Convienence Functions

        public bool NextLevelIsUnlocked()
        {
            return !LevelSets[SetIndex].IsLocked;
        }

        public bool NextSetIsUnlocked()
        {
            return !LevelSets[SetIndex + 1].IsLocked;
        }

        public string GetLevelTitle(int setIndex, int levelIndex)
        {
            return (LevelSets[setIndex].Title != "Unlockable") ? (levelIndex).ToString() : GetLevelTitle(levelIndex);
        }

        #endregion

        #region Level Management

        private string GetLevelTitle(int levelIndex)
        {
            return (((levelIndex + 64) < 82) ? ((char)(levelIndex + 64)).ToString() : ((char)(levelIndex + 65)).ToString());
        }

        private void GameSettingsChanged(object sender, EventArgs e)
        {
            if (Game != null)
            {
                Game.UpdateGameSettings(LevelSets[SetIndex].Levels[LevelIndex - 1], GameSettings);
            }
        }

        private void GameCompleted(object sender, EventArgs e)
        {
            if (Game != null)
                Game.GameCompleted -= new EventHandler(GameCompleted);

            UnlockCheck(LevelSets[SetIndex].Levels[LevelIndex - 1]);

            if (LevelComplete != null)
                LevelComplete(this, null);
        }

        private void UnlockCheck(Level level)
        {
            var unlocked = level.UnlockedOnComplete;
            if (unlocked.HasValue)
                NotifyUnLockOccurred(unlocked.Value);

        }

        private void SetupLevel(int index, int set)
        {
            var newLevel = LevelSets[set].Levels[index - 1];
            Game = new Game(newLevel, GameSettings);
            StartColor = GameSettings.ColorIconSets.Color[newLevel.Indexes[0]];
            EndColor = GameSettings.ColorIconSets.Color[newLevel.Indexes[newLevel.Indexes.Count - 1]];
            SetTitle = LevelSets[SetIndex].Title;
            Game.GameCompleted += GameCompleted;
        }

        public void LoadNextSet()
        {
            SetIndex++;
            LevelIndex = 0;

            GameSettings.CurrentSetIndex = SetIndex;
            GameSettings.CurrentLevelIndex = LevelIndex;

            if (SetIndex >= LevelSets.Count) return;

            LoadNextLevel();
        }

        public void LoadNextLevel()
        {
            Game = null;

            if (LevelIndex == LevelSets[SetIndex].Levels.Count)
            {
                if (AllLevelsComplete != null)
                    AllLevelsComplete(this, null);

                return;
            }

            LevelIndex++;

            LevelTitle = GetLevelTitle(SetIndex, LevelIndex);

            GameSettings.CurrentLevelIndex = LevelIndex;

            var newDefeated = (GameSettings.CurrentLevelIndex > GameSettings.DefeatedLevelIndex[SetIndex]) ?
                 GameSettings.CurrentLevelIndex : GameSettings.DefeatedLevelIndex[SetIndex];
            var DefeatedList = GameSettings.DefeatedLevelIndex;
            DefeatedList[SetIndex] = newDefeated;

            GameSettings.DefeatedLevelIndex = DefeatedList;

            SetupLevel(LevelIndex, SetIndex);
        }

        public void LoadNewLevel(int level, int set)
        {
            GameSettings.CurrentSetIndex = set;
            GameSettings.CurrentLevelIndex = level;
            SetIndex = set;
            LevelIndex = level;

            LevelTitle = GetLevelTitle(SetIndex, LevelIndex);

            SetupLevel(LevelIndex, SetIndex);

            if (NewLevelLoaded != null)
                NewLevelLoaded(this, null);
        }

        public void ResetLevel()
        {
            if (Game != null)
                Game.ResetGame();
        }

        public void NotifyUnLockOccurred(int level)
        {
            if (GameSettings.UnlockedLevelIndex.Contains(level)) return;

            GameSettings.AddToUnlockedIndex(level);

            var title = GetLevelTitle(level + 1);

            //trigger any gui
            if (NotifyUnlock != null)
                NotifyUnlock(title, null);
        }

        #endregion
    }
}
