// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
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
        /// Gets or sets the column alignment for this field. Leave as <c>null</c> to use the
        /// default alignment.
        /// </summary>
        public FieldAlignment? Alignment
        {
            get => Field.Alignment;
            set => Field.Alignment = value;
        }

        /// <summary>
        /// Gets or sets the character used to pad this field when writing values shorter than the field width.
        /// Leave as <c>null</c> to use the default pad character
        /// </summary>
        public char? PadCharacter
        {
            get => Field.PadCharacter;
            set => Field.PadCharacter = value;
        }

        /// <summary>
        /// <para>
        /// Gets or sets whether leading and trailing pad characters are trimmed when reading field values. Leave as
        /// <c>null</c> to use the default trim setting.
        /// </para>
        /// <para>
        /// WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that
        /// match the current pad character, those characters will also be trimmed.
        /// </para>
        /// </summary>
        public bool? TrimField
        {
            get => Field.TrimField;
            set => Field.TrimField = value;
        }

        /// <summary>
        /// 
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
        public Type ConverterType { get; set; }

        public FixedWidthFieldAttribute(int length)
        {
            Field = new FixedWidthField(length);
        }
    }
}
