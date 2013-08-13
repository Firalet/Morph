/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;

namespace Morph
{
    /// <summary>
    /// A collection of levels, with some added features for indexing.
    /// </summary>
    public class LevelSet : NotifyBase
    {
        private List<Level> _levels;
        public List<Level> Levels
        {
            get { return _levels; }
            set { _levels = value; NotifyPropertyChanged("Levels"); }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; NotifyPropertyChanged("Index"); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        private bool _isLocked;
        public bool IsLocked
        {
            get { return _isLocked; }
            set { _isLocked = value; NotifyPropertyChanged("IsLocked"); }
        }
    }

    public class LevelSetCollection : List<LevelSet>
    {
        public void AddWithIndex(LevelSet set)
        {
            set.Index = this.Count;
            this.Add(set);
        }
    }
}
