/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace Morph
{
    /// <summary>
    /// Class which handles storing of current game settings.
    /// Using Isolated storage, these settings are saved over anytime
    /// the user opens or closes the app.
    /// </summary>
    public class GameSettings : NotifyBase
    {
        public GameSettings()
        {
            //Settings.Clear();
            //UnlockedLevelIndex = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            UpdateSettings();
        }

        #region Properties & Fields

        public event EventHandler ColorIconSetChanged;

        private ColorIconSets _colorIconSets;
        public ColorIconSets ColorIconSets
        {
            get { return _colorIconSets; }
            set { _colorIconSets = value; NotifyPropertyChanged("ColorIconSet"); }
        }

        private Square[] _exampleSquares;
        public Square[] ExampleSquares
        {
            get { return _exampleSquares; }
            set { _exampleSquares = value; NotifyPropertyChanged("ExampleSquares"); }
        }

        #endregion

        #region Settings

        private const string UNLOCKED_LEVEL = "UnlockedLevel";
        private const string DEFEATED_LEVEL = "DefeatedLevel";
        private const string CURRENT_LEVEL = "CurrentLevel";
        private const string CURRENT_SET = "CurrentSet";
        private const string ENABLE_COLORBLIND_OPTIONS = "EnableColorBlindOption";
        private const string COLORBLIND_ICON = "ColorBlindIcon";
        private const string COLORBLIND_CONTRAST = "ColorBlindContrast";

        private IsolatedStorageSettings Settings = IsolatedStorageSettings.ApplicationSettings;

        public List<int> UnlockedLevelIndex
        {
            get { return (Settings.Contains(UNLOCKED_LEVEL)) ? (List<int>)Settings[UNLOCKED_LEVEL] : new List<int>() { 12 }; }
            set
            {
                if (Settings.Contains(UNLOCKED_LEVEL))
                    Settings[UNLOCKED_LEVEL] = value;
                else
                    Settings.Add(UNLOCKED_LEVEL, value);
                Settings.Save();
                NotifyPropertyChanged("UnlockedLevelIndex");
            }
        }

        public List<int> DefeatedLevelIndex
        {
            get { return (Settings.Contains(DEFEATED_LEVEL)) ? (List<int>)Settings[DEFEATED_LEVEL] : new List<int>() { 1, 1, 1, 1, 0 }; }
            set
            {
                if (Settings.Contains(DEFEATED_LEVEL))
                    Settings[DEFEATED_LEVEL] = value;
                else
                    Settings.Add(DEFEATED_LEVEL, value);
                Settings.Save();
                NotifyPropertyChanged("DefeatedLevelIndex");
            }
        }

        public int CurrentLevelIndex
        {
            get { return (Settings.Contains(CURRENT_LEVEL)) ? (int)Settings[CURRENT_LEVEL] : 1; }
            set
            {
                if (Settings.Contains(CURRENT_LEVEL))
                    Settings[CURRENT_LEVEL] = value;
                else
                    Settings.Add(CURRENT_LEVEL, value);
                Settings.Save();
                NotifyPropertyChanged("CurrentLevelIndex");
            }
        }

        public int CurrentSetIndex
        {
            get { return (Settings.Contains(CURRENT_SET)) ? (int)Settings[CURRENT_SET] : 0; }
            set
            {
                if (Settings.Contains(CURRENT_SET))
                    Settings[CURRENT_SET] = value;
                else
                    Settings.Add(CURRENT_SET, value);
                Settings.Save();
                NotifyPropertyChanged("CurrentSetIndex");
            }
        }

        public bool EnableColorblindOption
        {
            get { return (Settings.Contains(ENABLE_COLORBLIND_OPTIONS)) ? (bool)Settings[ENABLE_COLORBLIND_OPTIONS] : false; }
            set
            {
                if (Settings.Contains(ENABLE_COLORBLIND_OPTIONS))
                    Settings[ENABLE_COLORBLIND_OPTIONS] = value;
                else
                    Settings.Add(ENABLE_COLORBLIND_OPTIONS, value);
                Settings.Save();
                NotifyPropertyChanged("EnableColorblindOption");
            }
        }

        public StandardIcons.IconSet ColorblindIcon
        {
            get { return (Settings.Contains(COLORBLIND_ICON)) ? (StandardIcons.IconSet)Settings[COLORBLIND_ICON] : StandardIcons.IconSet.Shapes; }
            set
            {
                if (Settings.Contains(COLORBLIND_ICON))
                    Settings[COLORBLIND_ICON] = value;
                else
                    Settings.Add(COLORBLIND_ICON, value);
                Settings.Save();
                NotifyPropertyChanged("ColorblindIcon");
            }
        }

        public StandardContrasts.IconContrast ColorblindContrast
        {
            get { return (Settings.Contains(COLORBLIND_CONTRAST)) ? (StandardContrasts.IconContrast)Settings[COLORBLIND_CONTRAST] : StandardContrasts.IconContrast.White; }
            set
            {
                if (Settings.Contains(COLORBLIND_CONTRAST))
                    Settings[COLORBLIND_CONTRAST] = value;
                else
                    Settings.Add(COLORBLIND_CONTRAST, value);
                Settings.Save();
                NotifyPropertyChanged("ColorblindContrast");
            }
        }

        private bool _ravenMode;
        public bool RavenMode
        {
            get { return _ravenMode; }
            set { _ravenMode = value; NotifyPropertyChanged("RavenMode"); }
        }

        #endregion

        #region Methods

        public void UpdateSettings()
        {
            ColorIconSets = new ColorIconSets() { Color = StandardColors.Colors, Icon = GetIconSet() };

            if (ColorIconSetChanged != null)
                ColorIconSetChanged(this, null);

            ExampleSquares = new Square[]
            {
                new Square() { Color = ColorIconSets.Color[0], Uri = ColorIconSets.Icon[0] },
                new Square() { Color = ColorIconSets.Color[1], Uri = ColorIconSets.Icon[1] },
                new Square() { Color = ColorIconSets.Color[2], Uri = ColorIconSets.Icon[2] },
                new Square() { Color = ColorIconSets.Color[3], Uri = ColorIconSets.Icon[3] },
                new Square() { Color = ColorIconSets.Color[4], Uri = ColorIconSets.Icon[4] },
            };
        }

        private string[] GetIconSet()
        {
            return GetIconSet(ColorblindIcon, ColorblindContrast);
        }

        private string[] GetIconSet(StandardIcons.IconSet set, StandardContrasts.IconContrast contrast)
        {
            string[] iconSet = null;

            switch (set)
            {
                case StandardIcons.IconSet.Shapes:
                    iconSet = StandardIcons.ShapeIcons;
                    break;
                case StandardIcons.IconSet.Suites:
                    iconSet = StandardIcons.SuiteIcons;
                    break;
            }

            string[] iconsChosen = ((string[])iconSet.Clone());
            string[] iconsFinal = new string[iconsChosen.Length];

            for (int i = 0; i < iconsChosen.Length; i++)
            {
                iconsFinal[i] = "/Icons/" + contrast.ToString() + "/" + iconsChosen[i] + StandardIcons.iconFileType;
            }

            return iconsFinal;
        }

        public static string GetIconSet(string icon, string contrast)
        {
            return "/Icons/" + contrast + "/" + icon + StandardIcons.iconFileType;
        }

        public void AddToUnlockedIndex(int level)
        {
            List<int> settings = new List<int>();
            foreach (var item in UnlockedLevelIndex)
                settings.Add(item);

            settings.Add(level);
            settings.Sort();

            UnlockedLevelIndex = settings;
        }

        #endregion
    }

    /// <summary>
    /// Color and Icons for one game.
    /// </summary>
    public class ColorIconSets
    {
        public Color[] Color;
        public string[] Icon;
    }

    /// <summary>
    /// Colors and Icons for one square.
    /// </summary>
    public class ColorIconContrastSet : NotifyBase, IHitable
    {
        public event EventHandler Hit;

        public void OnHit()
        {
            if (Hit != null)
                Hit(this, null);
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { _color = value; NotifyPropertyChanged("Color"); }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; NotifyPropertyChanged("Icon"); NotifyPropertyChanged("Uri"); }
        }

        private string _contrast;
        public string Contrast
        {
            get { return _contrast; }
            set { _contrast = value; NotifyPropertyChanged("Contrast"); NotifyPropertyChanged("Uri"); }
        }

        public string Uri
        {
            get { return GameSettings.GetIconSet(Icon, Contrast); }
        }
    }

}
