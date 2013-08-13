/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Data;

namespace Morph
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Visible);
        }
    }
}
