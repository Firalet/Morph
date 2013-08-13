/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Morph
{
    public partial class LevelGrid : UserControl
    {
        public LevelGrid()
        {
            InitializeComponent();
        }

        public int SetIndex
        {
            get { return (int)GetValue(SetIndexProperty); }
            set { SetValue(SetIndexProperty, value); }
        }

        public static readonly DependencyProperty SetIndexProperty =
            DependencyProperty.Register("SetIndex", typeof(int), typeof(LevelGrid), new PropertyMetadata(0));

        public List<LevelDescriptor> Levels
        {
            get { return (List<LevelDescriptor>)GetValue(LevelsProperty); }
            set { SetValue(LevelsProperty, value); }
        }

        public static readonly DependencyProperty LevelsProperty =
            DependencyProperty.Register("Levels", typeof(List<LevelDescriptor>), typeof(LevelGrid), new PropertyMetadata(new List<LevelDescriptor>()));

        public LevelManager LevelManager
        {
            get { return (LevelManager)GetValue(LevelManagerProperty); }
            set { SetValue(LevelManagerProperty, value); }
        }

        public static readonly DependencyProperty LevelManagerProperty =
            DependencyProperty.Register("LevelManager", typeof(LevelManager), typeof(LevelGrid), new PropertyMetadata(null, LevelManagerChanged));

        public void OnVisibilityChanged()
        {
            if (this.Visibility == System.Windows.Visibility.Visible)
                UpdateLevels();
        }

        public static void LevelManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs p)
        {
            var control = d as LevelGrid;
            if (control != null)
                control.UpdateLevels();
        }

        public void UpdateLevels()
        {
            if (LevelManager == null)
                return;

            //populate levels available
            var available = new List<LevelDescriptor>();
            var possibleLevels = LevelManager.LevelSets[SetIndex].Levels.Count;
            var maxLevel = Math.Min(LevelManager.GameSettings.DefeatedLevelIndex[SetIndex], possibleLevels);
            var locked = LevelManager.LevelSets[SetIndex].IsLocked;

            for (int i = 0; i < possibleLevels; i++)
            {
                var title = LevelManager.GetLevelTitle(SetIndex, i + 1);
                var keyLevel = LevelManager.GameSettings.UnlockedLevelIndex.Contains(i);
                var unlocked = (!locked && i < maxLevel) || (locked && keyLevel);

                var desc = new LevelDescriptor()
                {
                    LevelTitle = title,
                    LevelIndex = (i + 1),
                    SetIndex = SetIndex,
                    BackgroundColor = Colors.Black,
                    Unlocked = unlocked,
                };
                desc.Hit += LoadLevel;
                available.Add(desc);
            }

            // mark currently playing level
            var currentSet = LevelManager.SetIndex;
            if (SetIndex == currentSet && LevelManager.GameSettings.CurrentLevelIndex - 1 < available.Count)
                available[LevelManager.GameSettings.CurrentLevelIndex - 1].BackgroundColor = LevelManager.StartColor;

            Levels = available;
        }

        void LoadLevel(int level, int set)
        {
            foreach (var desc in Levels)
                desc.Hit -= LoadLevel;

            LevelManager.LoadNewLevel(level, set);
        }

        public void LevelUnlocked(int level)
        {
            Levels[level].Unlocked = true;
        }
    }
}
