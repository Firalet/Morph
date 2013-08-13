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
    public partial class RavenScreen : UserControl, IVisibilityChanged
    {
        public RavenScreen()
        {
            InitializeComponent();
            Levels = _levels;
        }

        #region Properties & Fields

        private int CurrentIndex = 0;

        private List<RavenLevel> _levels = new List<RavenLevel>()
        { 
            new Level_Raven_01(),
            new Level_Raven_02(),
            new Level_Raven_03(),
            new Level_Raven_04(),
            new Level_Raven_05(),
        };

        public List<RavenLevel> Levels
        {
            get { return (List<RavenLevel>)GetValue(LevelsProperty); }
            set { SetValue(LevelsProperty, value); }
        }

        public static readonly DependencyProperty LevelsProperty =
            DependencyProperty.Register("Levels", typeof(List<RavenLevel>), typeof(RavenScreen), new PropertyMetadata(null));

        public RavenLevel CurrentLevel
        {
            get { return (RavenLevel)GetValue(CurrentLevelProperty); }
            set { SetValue(CurrentLevelProperty, value); }
        }

        public static readonly DependencyProperty CurrentLevelProperty =
            DependencyProperty.Register("CurrentLevel", typeof(RavenLevel), typeof(RavenScreen), new PropertyMetadata(null));

        public int CorrectAnswers
        {
            get { return (int)GetValue(CorrectAnswersProperty); }
            set { SetValue(CorrectAnswersProperty, value); }
        }

        public static readonly DependencyProperty CorrectAnswersProperty =
            DependencyProperty.Register("CorrectAnswers", typeof(int), typeof(RavenScreen), new PropertyMetadata(0));

        #endregion

        /// <summary>
        /// Reset Raven Screen to start level.
        /// </summary>
        public void OnVisibilityChanged()
        {
            CorrectAnswers = 0;
            CurrentIndex = 0;
            UpdateLevel();
        }

        #region Private Event Responders

        private void UpdateLevel()
        {
            if (CurrentIndex < Levels.Count)
            {
                if (CurrentLevel != null)
                    foreach (var ans in CurrentLevel.AnswerGrid)
                        ans.Hit -= OnHit;

                CurrentLevel = Levels[CurrentIndex++];

                foreach (var ans in CurrentLevel.AnswerGrid)
                    ans.Hit += OnHit;
            }
            else
            {
                CurrentIndex++;
                CloseSeq();
            }
        }

        private void OnHit(object sender, EventArgs e)
        {
            var set = sender as ColorIconContrastSet;

            if (set != null && set == CurrentLevel.AnswerGrid[CurrentLevel.AnswerIndex])
            {
                CorrectAnswers++;
                UpdateLevel();
            }
            else if (set != null && set != CurrentLevel.AnswerGrid[CurrentLevel.AnswerIndex])
                CloseSeq();
        }

        private void CloseSeq()
        {
            //unhook
            if (CurrentLevel != null)
                foreach (var ans in CurrentLevel.AnswerGrid)
                    ans.Hit -= OnHit;

            //fade transition
            Score.Opacity = 0.0;
            Score.Visibility = System.Windows.Visibility.Visible;
            ScoreFadeTransition.Completed += new EventHandler(ScoreFade_Completed);
            ScoreFadeTransition.Begin();
        }

        private void ScoreFade_Completed(object sender, EventArgs e)
        {
            //fade out all, fix opacity and score for next time
            Score.Visibility = System.Windows.Visibility.Collapsed;
            this.Visibility = Visibility.Collapsed;
            Question.Opacity = 1.0;
            Answer.Opacity = 1.0;
        }

        #endregion
    }
}
