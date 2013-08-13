/*
 * Copyright © 2013 Megan Chiu.  All rights reserved.
 */

using System;
using System.Net;
using System.Windows;

namespace Morph
{
    /// <summary>
    /// Directions possible to propogate square color to its neighboring squares.
    /// </summary>
    public enum Direction { Myself, North, NorthWest, West, SouthWest, South, SouthEast, East, NorthEast }
}
