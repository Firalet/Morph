/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Morph
{
    public partial class AboutScreen : UserControl
    {
        public AboutScreen()
        {
            InitializeComponent();
        }

        public LevelManager LevelManager
        {
            get { return (LevelManager)GetValue(LevelManagerProperty); }
            set { SetValue(LevelManagerProperty, value); }
        }

        public static readonly DependencyProperty LevelManagerProperty =
            DependencyProperty.Register("LevelManager", typeof(LevelManager), typeof(AboutScreen), new PropertyMetadata(null));

        private void Arrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LevelManager.NotifyUnLockOccurred(0);
        }
    }
}
