using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using System.ComponentModel;

namespace Morph
{
    /// <summary>
    /// Main Entry Point into Morph Application.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        #region Notify 

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Properties & Fields

        private int RefreshCount = 0;

        private LevelManager _levelManager;
        public LevelManager LevelManager
        {
            get { return _levelManager; }
            set { _levelManager = value; NotifyPropertyChanged("LevelManager"); }
        }

        #endregion

        #region Constructor & Destructor

        public MainPage()
        {
            InitializeComponent();

            LevelManager = new LevelManager();
            LevelManager.NotifyUnlock += LevelManager_NotifyUnlock;
            SettingsScreen.RavenTrigger += PoeScreen_OnHit;
            GameScreen.NextLevelNotFound += GameScreen_NextLevelNotFound;
            GameScreen.NextLevelFound += GameScreen_NextLevelFound;
        }

        ~MainPage()
        {
            LevelManager.NotifyUnlock -= LevelManager_NotifyUnlock;
            SettingsScreen.RavenTrigger -= PoeScreen_OnHit;
            GameScreen.NextLevelNotFound -= GameScreen_NextLevelNotFound;
            GameScreen.NextLevelFound -= GameScreen_NextLevelFound;
        }

        #endregion

        #region Private Event Responders

        private void LevelManager_NotifyUnlock(object sender, EventArgs e)
        {
            LockNotification.TriggerAnimation(sender.ToString());
        }

        private void GameScreen_NextLevelFound(object sender, EventArgs e)
        {
            RefreshCount = 0;
            LevelScreen.OnVisibilityChanged();
            LevelScreen.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void GameScreen_NextLevelNotFound(object sender, EventArgs e)
        {
            LevelScreen.OnVisibilityChanged();
            LevelScreen.Visibility = System.Windows.Visibility.Visible;
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutScreen.Visibility = (AboutScreen.Visibility == Visibility.Visible) ?
                Visibility.Collapsed : Visibility.Visible;

            LevelScreen.Visibility = Visibility.Collapsed;
            SettingsScreen.Visibility = Visibility.Collapsed;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            RefreshCount++;
            if (RefreshCount >= 5)
                LevelManager.NotifyUnLockOccurred(23);

            LevelManager.ResetLevel();
        }

        private void AllLevels_Click(object sender, EventArgs e)
        {
            LevelScreen.Visibility = (LevelScreen.Visibility == Visibility.Visible) ?
                Visibility.Collapsed : Visibility.Visible;

            SettingsScreen.Visibility = Visibility.Collapsed;
            AboutScreen.Visibility = Visibility.Collapsed;

            LevelScreen.OnVisibilityChanged();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            SettingsScreen.Visibility = (SettingsScreen.Visibility == Visibility.Visible) ?
                Visibility.Collapsed : Visibility.Visible;

            LevelScreen.Visibility = Visibility.Collapsed;
            AboutScreen.Visibility = Visibility.Collapsed;

            SettingsScreen.OnVisibilityChanged();
        }

        private void PoeScreen_OnHit(object sender, EventArgs e)
        {
            Poe.Visibility = Visibility.Visible;
            Poe.OnVisibilityChanged();
        }

        #endregion
    }
}