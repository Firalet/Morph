/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Morph
{
    public partial class LockNotification : UserControl
    {
        public LockNotification()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
        }

        public string Level
        {
            get { return (string)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register("Level", typeof(string), typeof(LockNotification), new PropertyMetadata(""));

        public void TriggerAnimation(string level)
        {
            Level = level;


            this.Visibility = Visibility.Visible;
            LockAnimation.Completed += new EventHandler(LockAnimation_Completed);
            LockAnimation.Begin();
        }

        void LockAnimation_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
