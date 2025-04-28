// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Exception thrown when an attempt is made to write a field value that is
    /// too long for the target field length.
    /// </summary>
    /// <remarks>
    /// Constructs a new <see cref="FixedWidthOverflowException"/> instance.
    /// </remarks>
    /// <param name="value">The value that overflowed the field.</param>
    /// <param name="length">The length of the field.</param>
    public class FixedWidthOverflowException(string value, int length) : Exception($"Value '{value}' is too long for fixed-width field with {length} character(s).")
    {
    }
}
