/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Morph
{
    public partial class GameScreen : UserControl
    {
        public GameScreen()
        {
            InitializeComponent();
        }

        #region Properties and Fields

        public event EventHandler NextLevelNotFound;
        public event EventHandler NextLevelFound;

        public bool IsLockable
        {
            get { return (bool)GetValue(IsLockableProperty); }
            set { SetValue(IsLockableProperty, value); }
        }

        public static readonly DependencyProperty IsLockableProperty =
            DependencyProperty.Register("IsLockable", typeof(bool), typeof(GameScreen), new PropertyMetadata(false));

        public LevelManager LevelManager
        {
            get { return (LevelManager)GetValue(LevelManagerProperty); }
            set { SetValue(LevelManagerProperty, value); }
        }

        public static readonly DependencyProperty LevelManagerProperty =
            DependencyProperty.Register("LevelManager", typeof(LevelManager), typeof(GameScreen), new PropertyMetadata(null, LevelManagerChanged));

        #endregion

        public static void LevelManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs p)
        {
            var control = d as GameScreen;

            if (control.LevelManager != null)
                control.LoadLevelManager();
        }

        public void LoadLevelManager()
        {
            LevelCompleteCanvas.Visibility = Visibility.Collapsed;

            //I know, there are no unhooks (-=) but we only have one level manager in the 
            //lifetime of the app, so this is not much of a memory leak
            LevelManager.AllLevelsComplete += LevelManager_AllLevelsComplete;
            LevelManager.NewLevelLoaded += LevelManager_NewLevelLoaded;
            LevelManager.LevelComplete += LevelManager_LevelComplete;
            GameGridFadeOut.Completed += GameGridAnimation_Completed;

            IsLockable = LevelManager.LevelSets[LevelManager.SetIndex].IsLocked;
        }

        #region Private Event Responders

        private void LevelManager_AllLevelsComplete(object sender, EventArgs e)
        {
            EndScreen.Visibility = System.Windows.Visibility.Visible;
        }

        private void LevelManager_NewLevelLoaded(object sender, EventArgs e)
        {
            if (GameGrid.Opacity == 0.0)
                GameGridFadeIn.Begin();

            LevelNotFound.Visibility = Visibility.Collapsed;
            LevelCompleteCanvas.Visibility = Visibility.Collapsed;
            EndScreen.Visibility = Visibility.Collapsed;

            if (NextLevelFound != null)
                NextLevelFound(this, null);

            IsLockable = LevelManager.LevelSets[LevelManager.SetIndex].IsLocked;
        }

        private void LevelManager_LevelComplete(object sender, EventArgs e)
        {
            LevelCompleteCanvas.Visibility = Visibility.Visible;
            LevelCompleteStoryboard.Begin();
            GameGridFadeOut.Begin();
        }

        private void GameGridAnimation_Completed(object sender, EventArgs e)
        {
            LevelCompleteCanvas.Visibility = Visibility.Collapsed;
            if (LevelManager.NextLevelIsUnlocked())
            {
                LevelManager.LoadNextLevel();
                GameGridFadeIn.Begin();
            }
            else
            {
                NextNotFound();
                LevelNotFound.Visibility = Visibility.Visible;
            }
        }

        private void EndScreen_NextSet(object sender, EventArgs e)
        {
            EndScreen.Visibility = Visibility.Collapsed;
            if (LevelManager.NextSetIsUnlocked())
                LevelManager.LoadNextSet();
            else
                NextNotFound();
        }

        private void NextNotFound()
        {
            //tells main screen to display level screen
            if (NextLevelNotFound != null)
                NextLevelNotFound(this, null);
        }

        #endregion

        #region Unlocks

        private void Start_Click(object sender, EventArgs e)
        {
            LevelManager.NotifyUnLockOccurred(17);
        }

        private void Complete_Click(object sender, MouseButtonEventArgs e)
        {
            LevelManager.NotifyUnLockOccurred(2);
        }

        private void Lock_Click(object sender, MouseButtonEventArgs e)
        {
            LevelManager.NotifyUnLockOccurred(9);
        }

        #endregion
    }
}
