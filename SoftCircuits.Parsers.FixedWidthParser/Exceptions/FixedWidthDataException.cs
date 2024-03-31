// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Exception thrown when reading a fixed-width data file and one of the
    /// fields contained data that could not be converted to the target type.
    /// </summary>
    public class FixedWidthDataException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="FixedWidthDataException"/> instance.
        /// </summary>
        /// <param name="name">The name of the property that was being set.</param>
        /// <param name="type">The type name of the property that was being set.</param>
        /// <param name="data">The data that could not be converted.</param>
        public FixedWidthDataException(string name, string type, string data)
            : base($"Unable to convert '{data}' to member '{name}' with type '{type}'.")
        {
        }
    }
}
