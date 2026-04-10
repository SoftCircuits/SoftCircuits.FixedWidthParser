// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Contains basic information about a fixed-width field.
    /// </summary>
    /// <remarks>
    /// Custom converter classes can optionally define a constructor that accepts a
    /// <see cref="FixedWidthDescriptor"/> parameter. These classes will receive an
    /// instance of this class. It provides information about the field being converted.
    /// </remarks>
    public class FixedWidthDescriptor(string name, int length)
    {
        /// <summary>
        /// Specifies the name of the fixed-width field.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Specifies the number of characters occupied by this fixed-width field.
        /// </summary>
        public int Length { get; set; } = length;
    }
}
