/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Morph
{
    public class SquareGrid : Panel
    {
        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(SquareGrid), new PropertyMetadata(0));

        protected override Size MeasureOverride(Size availableSize)
        {
            var l = Math.Min(availableSize.Width, availableSize.Height);
            var length = double.IsPositiveInfinity(l) ? 0 : l;
            var s = new Size(length / Size, length / Size);

            foreach (UIElement child in Children)
                child.Measure(s);

            return new Size(length, length);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size cellSize = new Size(finalSize.Width / Size, finalSize.Height / Size);
            int row = 0, col = 0;
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(cellSize.Width * col, cellSize.Height * row), cellSize));
                if (++col == Size)
                {
                    row++;
                    col = 0;
                }
            }
            return finalSize;
        }
    }
}
