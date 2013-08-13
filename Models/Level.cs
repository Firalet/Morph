/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace Morph
{
    /// <summary>
    /// Level descriptor used for the Level Grid control.
    /// </summary>
    public class LevelDescriptor : Level, IHitable
    {
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

        private int _SetIndex;
        public int SetIndex
        {
            get { return _SetIndex; }
            set { _SetIndex = value; NotifyPropertyChanged("SetIndex"); }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; NotifyPropertyChanged("BackgroundColor"); }
        }

        private bool _unlocked;
        public bool Unlocked
        {
            get { return _unlocked; }
            set { _unlocked = value; NotifyPropertyChanged("Unlocked"); }
        }

        public delegate void LoadLevelEvent(int level, int set);
        public event LoadLevelEvent Hit;

        public void OnHit()
        {
            if (Hit != null)
                Hit(LevelIndex, SetIndex);
        }
    }

    /// <summary>
    /// Basic Levels for both unlocked and locked levels.
    /// </summary>
    public class Level : LevelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        private List<int> _indexes;
        public List<int> Indexes
        {
            get { return _indexes; }
            set { _indexes = value; NotifyPropertyChanged("Indexes"); }
        }

        private List<Direction> _PropogateDirections;
        public List<Direction> PropogateDirections
        {
            get { return _PropogateDirections; }
            set { _PropogateDirections = value; NotifyPropertyChanged("PropogateDirections"); }
        }

        private List<int> _prePopulatedGrid = null;
        public List<int> PrePopulatedGrid
        {
            get { return _prePopulatedGrid; }
            set { _prePopulatedGrid = value; NotifyPropertyChanged("PrePopulatedGrid"); }
        }

        private int? _unlockedOnComplete = null;
        public int? UnlockedOnComplete
        {
            get { return _unlockedOnComplete; }
            set { _unlockedOnComplete = value; NotifyPropertyChanged("UnlockedOnComplete"); }
        }

    }

    /// <summary>
    /// Special level for Raven Mode.
    /// </summary>
    public class RavenLevel : LevelBase
    {
        private List<ColorIconContrastSet> _questionGrid = null;
        public List<ColorIconContrastSet> QuestionGrid
        {
            get { return _questionGrid; }
            set { _questionGrid = value; NotifyPropertyChanged("QuestionGrid"); }
        }

        private List<ColorIconContrastSet> _answerGrid = null;
        public List<ColorIconContrastSet> AnswerGrid
        {
            get { return _answerGrid; }
            set { _answerGrid = value; NotifyPropertyChanged("AnswerGrid"); }
        }

        public int AnswerIndex { get; set; }
    }

    /// <summary>
    /// Level Base clases from which all levels are derived.
    /// </summary>
    public abstract class LevelBase : NotifyBase
    {
        private int _gridSize;
        public int GridSize
        {
            get { return _gridSize; }
            set { _gridSize = value; NotifyPropertyChanged("GridSize"); }
        }
    }
}
