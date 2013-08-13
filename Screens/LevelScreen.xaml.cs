/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Morph
{
    public partial class LevelScreen : UserControl, IVisibilityChanged
    {
        public LevelScreen()
        {
            InitializeComponent();
        }

        #region Properties & Fields

        private int BackCount = 0;

        private List<LevelGrid> LevelGUI = new List<LevelGrid>();

        public LevelManager LevelManager
        {
            get { return (LevelManager)GetValue(LevelManagerProperty); }
            set { SetValue(LevelManagerProperty, value); }
        }

        public static readonly DependencyProperty LevelManagerProperty =
            DependencyProperty.Register("LevelManager", typeof(LevelManager), typeof(LevelScreen), new PropertyMetadata(null));

        #endregion

        public void OnVisibilityChanged()
        {
            if (this.Visibility == System.Windows.Visibility.Visible)
            {
                foreach (var level in LevelGUI)
                    level.OnVisibilityChanged();
            }

            //set current page to be the level set you are on
            PivotControl.SelectedIndex = LevelManager.SetIndex;

            BackCount = 0;
        }

        #region Private Event Responders

        private void LevelScreen_Loaded(object sender, RoutedEventArgs e)
        {
            var LevelScreen = sender as LevelGrid;
            LevelGUI.Add(LevelScreen);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BackCount++;
            if (BackCount >= 5)
                LevelManager.NotifyUnLockOccurred(4);

            var newIndex = (PivotControl.SelectedIndex == 0) ? (PivotControl.Items.Count - 1) : (PivotControl.SelectedIndex - 1);
            PivotControl.SelectedIndex = (newIndex) % (PivotControl.Items.Count);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            BackCount = 0;
            PivotControl.SelectedIndex = (PivotControl.SelectedIndex + 1) % (PivotControl.Items.Count);
        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LevelManager.NotifyUnLockOccurred(15);

            //small hack for updates to look uniform
            foreach (var screen in LevelGUI)
                if (screen.SetIndex == LevelManager.LockedSet)
                    screen.LevelUnlocked(15);
        }

        #endregion
    }
}
