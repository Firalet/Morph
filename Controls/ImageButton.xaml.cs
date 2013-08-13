/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Morph
{
    public partial class ImageButton : Button
    {
        public ImageButton()
        {
            InitializeComponent();
        }

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(ImageButton), new PropertyMetadata(""));

        public string InvUri
        {
            get { return (string)GetValue(InvUriProperty); }
            set { SetValue(InvUriProperty, value); }
        }

        public static readonly DependencyProperty InvUriProperty =
            DependencyProperty.Register("InvUri", typeof(string), typeof(ImageButton), new PropertyMetadata(""));

        public string UsedUri
        {
            get { return (string)GetValue(UsedUriProperty); }
            set { SetValue(UsedUriProperty, value); }
        }

        public static readonly DependencyProperty UsedUriProperty =
            DependencyProperty.Register("UsedUri", typeof(string), typeof(ImageButton), new PropertyMetadata(""));

        private void ButtonIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //to do
        }

        private void ButtonMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UsedUri = InvUri;
        }

        private void ButtonMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UsedUri = Uri;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            UsedUri = Uri;
        }
    }
}
