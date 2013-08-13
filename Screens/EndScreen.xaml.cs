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
    public partial class EndScreen : UserControl
    {
        public EndScreen()
        {
            InitializeComponent();
        }

        public event EventHandler OnHit;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnHit != null)
                OnHit(this, null);
        }
    }
}
