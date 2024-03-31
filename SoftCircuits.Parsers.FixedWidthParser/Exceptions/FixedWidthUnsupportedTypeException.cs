// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Exception that indicates a class property was mapped to a field, but that there is no data
    /// converter for the class property type. To map this property, a custom data converter must
    /// be supplied.
    /// </summary>
    public class FixedWidthUnsupportedTypeException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="FixedWidthUnsupportedTypeException"/> instance.
        /// </summary>
        /// <param name="type">The type that was unsupported.</param>
        public FixedWidthUnsupportedTypeException(Type type)
            : base($"There is no built-in data converter and no custom data converter has been supplied for type '{type.FullName}'.")
        {
        }
    }
}
