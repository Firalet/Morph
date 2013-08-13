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
    /// One Square in a game. Consists of a a color, and logic for
    /// propogating the color to it's neighboring squares.
    /// </summary>
    public class Square : NotifyBase, IHitable
    {
        #region Properties and Fields

        private bool _hitEnabled;
        public bool HitEnabled
        {
            get { return _hitEnabled; }
            set { _hitEnabled = value; NotifyPropertyChanged("HitEnabled"); }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { _color = value; NotifyPropertyChanged("Color"); }
        }

        private string _uri;
        public string Uri
        {
            get { return _uri; }
            set { _uri = value; NotifyPropertyChanged("Uri"); }
        }

        public int InitIndex { get; set; }

        public Square North { get; set; }
        public Square NorthWest { get; set; }
        public Square West { get; set; }
        public Square SouthWest { get; set; }
        public Square South { get; set; }
        public Square SouthEast { get; set; }
        public Square East { get; set; }
        public Square NorthEast { get; set; }

        public List<Direction> PropogateDirections { get; set; }

        public List<int> SetIndex { get; set; } //too keep track of level subset

        public List<Color> Colors { get; set; }

        public List<string> Uris { get; set; }

        public int ColorIndex { get; set; }

        public int Index { get; set; } //for easy debugging

        public event EventHandler Hit;

        #endregion

        #region Methods

        public void OnHit()
        {
            if (Hit != null)
                Hit(this, null);
        }

        public void Propogate()
        {
            foreach (var direction in PropogateDirections)
            {
                switch (direction)
                {
                    case Direction.Myself:
                        UpdateColor();
                        break;
                    case Direction.North:
                        if (North != null) North.UpdateColor();
                        break;
                    case Direction.NorthWest:
                        if (NorthWest != null) NorthWest.UpdateColor();
                        break;
                    case Direction.West:
                        if (West != null) West.UpdateColor();
                        break;
                    case Direction.SouthWest:
                        if (SouthWest != null) SouthWest.UpdateColor();
                        break;
                    case Direction.South:
                        if (South != null) South.UpdateColor();
                        break;
                    case Direction.SouthEast:
                        if (SouthEast != null) SouthEast.UpdateColor();
                        break;
                    case Direction.East:
                        if (East != null) East.UpdateColor();
                        break;
                    case Direction.NorthEast:
                        if (NorthEast != null) NorthEast.UpdateColor();
                        break;
                }
            }
        }

        public void UpdateColor()
        {
            ColorIndex = (ColorIndex + 1) % Colors.Count;
            Color = Colors[ColorIndex];
            Uri = Uris[ColorIndex]; //for colorblind options
        }

        public void ResetColor()
        {
            ColorIndex = InitIndex;
            Color = Colors[InitIndex];
            Uri = Uris[InitIndex]; //for colorblind options
        }

        public bool IsEndColor()
        {
            return Color == Colors[Colors.Count - 1];
        }

        #endregion
    }
}
