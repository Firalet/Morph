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
    /// The class which handles all game events and structures. One game is played per level.
    /// </summary>
    public class Game : NotifyBase
    {
        #region Properties & Fields

        public event EventHandler GameCompleted;

        private List<Square> _gameGrid;
        public List<Square> GameGrid
        {
            get { return _gameGrid; }
            set { _gameGrid = value; NotifyPropertyChanged("GameGrid"); }
        }

        public int Size { get; set; }

        #endregion

        #region Constructor & Destructor 

        public Game(Level level, GameSettings gameSettings)
        {
            Size = level.GridSize;
            var total = Size * Size;
            GameGrid = new List<Square>();

            var uris = new List<string>();
            var colors = new List<Color>();
            for (int i = 0; i < level.Indexes.Count; i++)
            {
                uris.Add(gameSettings.ColorIconSets.Icon[level.Indexes[i]]);
                colors.Add(gameSettings.ColorIconSets.Color[level.Indexes[i]]);
            }

            for (int i = 0; i < total; i++)
            {
                var init = (level.PrePopulatedGrid != null) ? level.PrePopulatedGrid[i] : 0;

                GameGrid.Add(new Square()
                {
                    Index = i,
                    PropogateDirections = level.PropogateDirections,
                    SetIndex = level.Indexes,
                    Uris = uris,
                    Uri = uris[init],
                    Colors = colors,
                    Color = colors[init],
                    ColorIndex = init,
                    InitIndex = init,
                });
            }


            //hook up adjecent squares
            for (int i = 0; i < total; i++)
            {
                var newSquare = GameGrid[i];
                newSquare.Hit += SquareHit;

                var north = i - Size;
                var northEast = i - Size + 1;
                var east = i + 1;
                var southEast = i + Size + 1;
                var south = i + Size;
                var southWest = i + Size - 1;
                var west = i - 1;
                var northWest = i - Size - 1;

                if (north >= 0)
                    newSquare.North = GameGrid[north];

                if (northEast >= 0 && (northEast % Size) != 0)
                    newSquare.NorthEast = GameGrid[northEast];

                if (east < total && (i + 1) % Size != 0)
                    newSquare.East = GameGrid[east];

                if (southEast < total && (southEast % Size) != 0)
                    newSquare.SouthEast = GameGrid[southEast];

                if (south < total)
                    newSquare.South = GameGrid[south];

                if (southWest < total && (southWest + 1) % Size != 0)
                    newSquare.SouthWest = GameGrid[southWest];

                if (west >= 0 && (i % Size) != 0)
                    newSquare.West = GameGrid[west];

                if (northWest >= 0 && (northWest + 1) % Size != 0)
                    newSquare.NorthWest = GameGrid[northWest];
            }

            //enable square hitting
            foreach (var square in GameGrid)
                square.HitEnabled = true;
        }

        ~Game()
        {
            foreach (var square in GameGrid)
                square.Hit -= SquareHit;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles updates from game settings such as icon, color, contrast, etc.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="gameSettings"></param>
        public void UpdateGameSettings(Level level, GameSettings gameSettings)
        {
            var uris = new List<string>();
            var colors = new List<Color>();
            for (int i = 0; i < level.Indexes.Count; i++)
            {
                uris.Add(gameSettings.ColorIconSets.Icon[level.Indexes[i]]);
                colors.Add(gameSettings.ColorIconSets.Color[level.Indexes[i]]);
            }

            foreach (var square in GameGrid)
            {
                square.Colors = colors;
                square.Uris = uris;

                square.Color = colors[square.ColorIndex];
                square.Uri = uris[square.ColorIndex];
            }
        }

        /// <summary>
        /// Resets current game to initial colors.
        /// </summary>
        public void ResetGame()
        {
            foreach (var square in GameGrid)
                square.ResetColor();
        }

        /// <summary>
        /// Handles events when a square is hit. Generally used from propogating colors
        /// to other squares.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SquareHit(object sender, EventArgs e)
        {
            var square = (Square)sender;

            square.Propogate();

            if (GameEnd())
                OnGameCompleted();
        }

        /// <summary>
        /// Tells you whether the game has hit an end condition.
        /// </summary>
        /// <returns></returns>
        private bool GameEnd()
        {
            foreach (var square in GameGrid)
            {
                if (!square.IsEndColor())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Handles events when the game is complete.
        /// </summary>
        private void OnGameCompleted()
        {
            //disable square hitting
            foreach (var square in GameGrid)
                square.HitEnabled = false;

            if (GameCompleted != null)
                GameCompleted(this, null);
        }

        #endregion
    }
}
