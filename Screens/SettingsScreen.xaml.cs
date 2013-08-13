/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Morph
{
    public partial class SettingsScreen : UserControl, IVisibilityChanged
    {
        public SettingsScreen()
        {
            InitializeComponent();
        }

        #region Properties & Fields

        public event EventHandler RavenTrigger;

        public GameSettings GameSettings
        {
            get { return (GameSettings)GetValue(GameSettingsProperty); }
            set { SetValue(GameSettingsProperty, value); }
        }

        public bool Trigger
        {
            get { return (bool)GetValue(TriggerProperty); }
            set { SetValue(TriggerProperty, value); }
        }

        public static readonly DependencyProperty TriggerProperty =
            DependencyProperty.Register("Trigger", typeof(bool), typeof(SettingsScreen), new PropertyMetadata(false));

        public static readonly DependencyProperty GameSettingsProperty =
            DependencyProperty.Register("GameSettings", typeof(GameSettings), typeof(SettingsScreen), new PropertyMetadata(null, GameSettingsChanged));

        public static void GameSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs p)
        {
            var control = (d as SettingsScreen);
            var settings = (d as SettingsScreen).GameSettings;

            if (settings != null)
            {
                foreach (var square in settings.ExampleSquares)
                    square.Hit += control.OnHit; // hook up initial
            }
        }

        private Queue<string> History = new Queue<string>();

        #endregion

        #region Raven Trigger Handling

        public void OnVisibilityChanged()
        {
            History.Clear();
            Trigger = false;
        }

        private void OnHit(object sender, EventArgs e)
        {
            //log history
            var uri = (sender as Square).Uri;
            var split = uri.Split(new char[] { '.', '\\', '/' });
            var item = split[split.Count() - 2];
            History.Enqueue(item.ToString());

            if (History.Count > 5)
                History.Dequeue();

            if (History.Count != 5)
            {
                Trigger = false;
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                var w1 = History.ElementAt(i);
                var w2 = History.ElementAt(i + 1);
                if (!Alphabetize(w1, w2))
                {
                    Trigger = false;
                    return;
                }

            }

            //play animation, then show it
            Raven.Opacity = 0.0;
            Trigger = true;
            RavenAnimationFadeIn.Begin();
        }

        private bool Alphabetize(string one, string two)
        {
            for (int j = 0; j < Math.Min(one.Count(), two.Count()); j++)
            {
                var comp = (one[j]) - (two[j]);

                if (comp < 0)
                    return true;
                else if (comp == 0)
                    continue;
                else
                    return false;
            }

            return false; //both are equal
        }

        #endregion

        #region Private Event Responders

        private void Set_Checked(object sender, RoutedEventArgs e)
        {
            var button = ((RadioButton)sender);
            if (button.IsChecked.HasValue && button.IsChecked.Value)
            {
                var enumString = ((RadioButton)sender).DataContext.ToString();
                var enumIcon = (StandardIcons.IconSet)Enum.Parse(typeof(StandardIcons.IconSet), enumString, true);
                if (GameSettings.ColorblindIcon != enumIcon)
                {
                    foreach (var square in GameSettings.ExampleSquares)
                        square.Hit -= OnHit;

                    GameSettings.ColorblindIcon = enumIcon;
                    GameSettings.UpdateSettings();

                    foreach (var square in GameSettings.ExampleSquares)
                        square.Hit += OnHit;
                }
            }
        }

        private void Contrast_Checked(object sender, RoutedEventArgs e)
        {
            var button = ((RadioButton)sender);
            if (button.IsChecked.HasValue && button.IsChecked.Value)
            {
                var enumString = ((RadioButton)sender).DataContext.ToString();
                var enumIcon = (StandardContrasts.IconContrast)Enum.Parse(typeof(StandardContrasts.IconContrast), enumString, true);
                if (GameSettings.ColorblindContrast != enumIcon)
                {
                    foreach (var square in GameSettings.ExampleSquares)
                        square.Hit -= OnHit;

                    GameSettings.ColorblindContrast = enumIcon;
                    GameSettings.UpdateSettings();

                    foreach (var square in GameSettings.ExampleSquares)
                        square.Hit += OnHit;
                }
            }
        }

        private void Set_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            var radioString = button.DataContext.ToString();
            var radioEnum = (StandardIcons.IconSet)Enum.Parse(typeof(StandardIcons.IconSet), radioString, true);

            if (radioEnum == GameSettings.ColorblindIcon)
                button.IsChecked = true;
        }

        private void Contrast_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            var radioString = button.DataContext.ToString();
            var radioEnum = (StandardContrasts.IconContrast)Enum.Parse(typeof(StandardContrasts.IconContrast), radioString, true);

            if (radioEnum == GameSettings.ColorblindContrast)
                button.IsChecked = true;
        }

        private void Raven_Click(object sender, EventArgs e)
        {
            if (Raven.Opacity == 1.0) //stop multi clicks
            {
                if (RavenTrigger != null)
                    RavenTrigger(this, null);
                RavenAnimationFadeOut.Completed += new EventHandler(RavenAnimationFadeOut_Completed);
                RavenAnimationFadeOut.Begin();
            }
        }

        private void RavenAnimationFadeOut_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            this.Opacity = 1.0;
        }

        #endregion
    }
}
