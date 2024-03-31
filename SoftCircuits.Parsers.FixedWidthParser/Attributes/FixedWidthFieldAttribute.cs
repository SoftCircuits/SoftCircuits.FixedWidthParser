// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Attribute that can be applied to class properties and fields to map the member
    /// to a fixed-width field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FixedWidthFieldAttribute : Attribute
    {
        internal readonly FixedWidthField Field;

        /// <summary>
        /// Gets or sets the column alignment for this field.
        /// </summary>
        public FieldAlignment Alignment
        {
            get => Field.Alignment ?? default;
            set => Field.Alignment = value;
        }

        /// <summary>
        /// Gets or sets the character used to pad this field when writing values shorter 
        /// than the field width.
        /// </summary>
        public char PadCharacter
        {
            get => Field.PadCharacter ?? default;
            set => Field.PadCharacter = value;
        }

        /// <summary>
        /// <para>
        /// Gets or sets whether leading and trailing pad characters are trimmed when reading field values.
        /// </para>
        /// <para>
        /// WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that
        /// match the current pad character, those characters will also be trimmed.
        /// </para>
        /// </summary>
        public bool TrimField
        {
            get => Field.TrimField ?? default;
            set => Field.TrimField = value;
        }

        /// <summary>
        /// Gets or sets the number of characters to skip before the field. Normally, this property
        /// is set to zero. You can use this property to skip fixed-width fields that you don't
        /// want to read.
        /// </summary>
        public int Skip
        {
            get => Field.Skip;
            set => Field.Skip = value;
        }

        /// <summary>
        /// Gets or sets the data type that converts this field to and from a string.
        /// Must derive from <see cref="IDataConverter"/>. For best results and type
        /// safety, derive the class from <see cref="DataConverter{T}"/>.
        /// </summary>
        public Type? ConverterType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">The length of this field.</param>
        public FixedWidthFieldAttribute(int length)
        {
            Field = new FixedWidthField(length);
        }
    }
}
