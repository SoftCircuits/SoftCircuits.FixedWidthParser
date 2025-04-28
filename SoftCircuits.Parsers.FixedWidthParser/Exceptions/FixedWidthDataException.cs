// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Exception thrown when reading a fixed-width data file and one of the
    /// fields contained data that could not be converted to the target type.
    /// </summary>
    /// <remarks>
    /// Constructs a new <see cref="FixedWidthDataException"/> instance.
    /// </remarks>
    /// <param name="name">The name of the property that was being set.</param>
    /// <param name="type">The type name of the property that was being set.</param>
    /// <param name="data">The data that could not be converted.</param>
    public class FixedWidthDataException(string name, string type, string data) : Exception($"Unable to convert '{data}' to member '{name}' with type '{type}'.")
    {
    }
}
