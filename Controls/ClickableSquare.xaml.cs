/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Morph
{
    public partial class ClickableSquare : UserControl
    {
        public ClickableSquare()
        {
            InitializeComponent();
        }

        public event EventHandler OnClick; //for GUI level

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ClickableSquare), new PropertyMetadata(""));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(ClickableSquare), new PropertyMetadata(""));

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(ClickableSquare), new PropertyMetadata(Visibility.Collapsed));

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(ClickableSquare), new PropertyMetadata(Colors.Cyan));

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(ClickableSquare), new PropertyMetadata(Colors.White));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IHitable)
            {
                var hit = DataContext as IHitable;
                hit.OnHit();
            }

            if (OnClick != null)
                OnClick(this, null);
        }
    }
}
