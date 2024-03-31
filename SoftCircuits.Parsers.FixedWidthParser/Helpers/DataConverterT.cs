// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Diagnostics.CodeAnalysis;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Base class for strongly typed data converters. The easiest way
    /// to implement <see cref="IDataConverter"/> in a type-safe way is
    /// to derive your data converter class from this class with the
    /// appropriate data type.
    /// </summary>
    /// <typeparam name="T">The data type of the property being
    /// converted.</typeparam>
    public abstract class DataConverter<T> : IDataConverter
    {
        /// <summary>
        /// Returns the type this converter supports.
        /// </summary>
        /// <returns>Returns the type this converter supports.</returns>
        public Type GetDataType() => typeof(T);

        /// <summary>
        /// Converts the variable to a string.
        /// </summary>
        /// <param name="value">The variable to be converted to a string.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        public string ConvertToString(object? value) => ConvertToString((T?)value);

        /// <summary>
        /// Converts a string back to a value. Returns <c>true</c> if
        /// successful or <c>false</c> if the string could not be
        /// converted.
        /// </summary>
        /// <param name="s">The string to convert to a value.</param>
        /// <param name="value">Receives the resulting value that was
        /// parsed from the string.</param>
        /// <returns>True if successful, false if the string could not
        /// be converted.</returns>
#if NETSTANDARD2_0
        public bool TryConvertFromString(string s, out object value)
#else
        public bool TryConvertFromString(string? s, [NotNullWhen(true)] out object? value)
#endif
        {
            if (TryConvertFromString(s, out T? temp))
            {
                value = temp;
                return true;
            }
            value = default;
            return false;
        }

        #region Abstract methods

        /// <summary>
        /// Override this abstract method to implement your own logic to convert
        /// a value of type <typeparamref name="T"/> to a string.
        /// </summary>
        /// <param name="value">The value to be converted to a string.</param>
        /// <returns>Returns a string representation of
        /// <paramref name="value"/>.</returns>
        public abstract string ConvertToString(T? value);

        /// <summary>
        /// Override this abstract method to implement your own logic to convert
        /// a string back to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="s">The string to convert to a value.</param>
        /// <param name="value">Receives the resulting value that was
        /// parsed from the string.</param>
        /// <returns>True if successful, false if the string could not
        /// be converted.</returns>
#if NETSTANDARD2_0
        public abstract bool TryConvertFromString(string s, out T value);
#else
        public abstract bool TryConvertFromString(string? s, [NotNullWhen(true)] out T? value);
#endif

        #endregion

    }
}
