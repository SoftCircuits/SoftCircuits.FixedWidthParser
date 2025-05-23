﻿// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    /// <summary>
    /// Converter for unsupported types. Throws an exception if called to convert
    /// from a string.
    /// </summary>
    /// <exception cref="FixedWidthUnsupportedTypeException"></exception>
    internal class UnsupportedTypeConverter(Type type) : IDataConverter
    {
        private readonly Type Type = type;

        /// <summary>
        /// Returns the type this converter supports.
        /// </summary>
        /// <returns>Returns the type this converter supports.</returns>
        public Type GetDataType() => Type;

        public string ConvertToString(object? value) => value?.ToString() ?? string.Empty;

        public bool TryConvertFromString(string? s, out object value)
        {
            throw new FixedWidthUnsupportedTypeException(Type);
        }
    }
}
