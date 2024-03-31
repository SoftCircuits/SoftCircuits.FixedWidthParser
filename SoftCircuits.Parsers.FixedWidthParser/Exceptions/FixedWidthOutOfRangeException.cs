// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Exception thrown when the library attempts to read a fixed-width
    /// field but the current line is not long enough to contain that field.
    /// </summary>
    public class FixedWidthOutOfRangeException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="FixedWidthOutOfRangeException"/> instance.
        /// </summary>
        public FixedWidthOutOfRangeException()
            : base("The current line is not long enough for one or more fixed-width fields.")
        {
        }
    }
}
