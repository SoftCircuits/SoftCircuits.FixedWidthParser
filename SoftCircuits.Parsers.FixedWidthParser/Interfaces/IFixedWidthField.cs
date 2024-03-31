// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Interface that defines a fixed-width field.
    /// </summary>
    public interface IFixedWidthField
    {
        /// <summary>
        /// Sets the column alignment for this field. Set to null to use the default alignment.
        /// </summary>
        /// <param name="alignment">A value that indicates how this column is aligned.</param>
        /// <returns>This <see cref="IFixedWidthField"/> instance.</returns>
        public IFixedWidthField SetAlignment(FieldAlignment? alignment);

        /// <summary>
        /// Sets the character used to pad this field when writing values shorter 
        /// than the field width.
        /// </summary>
        /// <param name="padCharacter">The character used to pad this field.</param>
        /// <returns>This <see cref="IFixedWidthField"/> instance.</returns>
        public IFixedWidthField SetPadCharacter(char? padCharacter);

        /// <summary>
        /// <para>
        /// Sets whether leading and trailing pad characters are trimmed when reading field values.
        /// </para>
        /// <para>
        /// WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that
        /// match the current pad character, those characters will also be trimmed.
        /// </para>
        /// </summary>
        /// <param name="trimField">A value that determines whether or not pad characters are trimmed.</param>
        /// <returns>This <see cref="IFixedWidthField"/> instance.</returns>
        public IFixedWidthField SetTrimField(bool? trimField);

        /// <summary>
        /// Sets this field's skip value. The skip value specifies how many characters to skip before this field. It is
        /// a convenient way to skip fields that you don't want to serialize. Normally, this value should be set to 0.
        /// </summary>
        /// <param name="skip">The number of characters to skip before this field.</param>
        /// <returns>This <see cref="IFixedWidthField"/> instance.</returns>
        public IFixedWidthField SetSkip(int skip);

        /// <summary>
        /// Sets a custom converter type used to convert this field. Set to null to use the default converter (if
        /// there is one for the property type).
        /// </summary>
        /// <param name="converter">The converter class type. The class must derive from <see cref="IDataConverter"/>.
        /// The easiest, type-safe way to create a converter class is to derive the class from <see cref="DataConverter{T}"/>.
        /// </param>
        /// <returns>This <see cref="IFixedWidthField"/> instance.</returns>
        public IFixedWidthField SetConverterType(Type? converter);
    }
}
