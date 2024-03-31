// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Diagnostics.CodeAnalysis;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Interface for all data converters. For best result, derive your custom
    /// data converter classes from <see cref="DataConverter{T}"/> instead of
    /// implementing this interface directly.
    /// </summary>
    public interface IDataConverter
    {
        /// <summary>
        /// Gets the data type that this object converts.
        /// </summary>
        /// <returns>The data type that this object converts.</returns>
        Type GetDataType();

        /// <summary>
        /// Converts the object to a string.
        /// </summary>
        /// <param name="value">The object to be converted to a string.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        string ConvertToString(object? value);

        /// <summary>
        /// Converts a string back to an object. Returns <c>true</c> if
        /// successful or <c>false</c> if the string could not be
        /// converted. If the string cannot be converted, <paramref name="value"/>
        /// should be set to its default value.
        /// </summary>
        /// <param name="s">The string to convert to an object.</param>
        /// <param name="value">Receives the resulting value that was
        /// parsed from the string.</param>
        /// <returns>True if successful, false if the string could not
        /// be converted.</returns>
#if NETSTANDARD2_0
        bool TryConvertFromString(string? s, out object value);
#else
        bool TryConvertFromString(string? s, [NotNullWhen(true)] out object? value);
#endif
    }
}
