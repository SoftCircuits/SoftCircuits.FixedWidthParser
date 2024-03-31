// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class to map class objects to fields and write them to a fixed-width file.
    /// </summary>
    /// <typeparam name="T">The type fields are being mapped to.</typeparam>
    public class FixedWidthWriter<T> : FixedWidthWriter where T : class, new()
    {
        private string[]? Values;

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to write.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(string filename, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), filename, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to write.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(string filename, Encoding encoding, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), filename, encoding, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to write.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(Stream stream, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), stream, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to write.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(Stream stream, Encoding encoding, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), stream, encoding, options)
        {
        }

        /// <summary>
        /// <para>
        /// Maps a field to the specified property. If a map already exists for the property, that map is modified. Otherwise, a new
        /// map is created and appended as the last field.
        /// </para>
        /// <para>
        /// This method is intended to be called after a <see cref="FixedWidthReader{T}"/> or <see cref="FixedWidthWriter{T}"/> has
        /// been created for a type with no <see cref="FixedWidthFieldAttribute"/> attributes to programmatically specify field mapping.
        /// However, it can also be used to modify existing field mappings.
        /// </para>
        /// </summary>
        /// <typeparam name="TMember">The property type being mapped.</typeparam>
        /// <param name="expression">An expression that specifies the property being mapped.</param>
        /// <param name="length">The number of characters used by this field.</param>
        /// <returns>An <see cref="IFixedWidthField"/> that supports a Fluent interface to set additional mapping properties.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public IFixedWidthField MapField<TMember>(Expression<Func<T, TMember>> expression, int length) => MappedFixedWidthField.MapField(Fields, expression, length);

        /// <summary>
        /// Writes the given item to the fixed-width file.
        /// </summary>
        /// <param name="item">The item to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(T item)
        {
            SetColumnValues(item);
            Write(Values);
        }

        /// <summary>
        /// Asynchronously writes the given item to the fixed-width file.
        /// </summary>
        /// <param name="item">The item to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public async Task WriteAsync(T item)
        {
            SetColumnValues(item);
            await WriteAsync(Values);
        }

        /// <summary>
        /// Writes the given items to the fixed-width file.
        /// </summary>
        /// <param name="items">The items to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (T item in items)
                Write(item);
        }

        /// <summary>
        /// Asynchronously writes the given items to the fixed-width file.
        /// </summary>
        /// <param name="items">The items to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public async Task WriteAsync(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (T item in items)
                await WriteAsync(item);
        }

        /// <summary>
        /// Populates <see cref="Values"/> with string values of each member.
        /// </summary>
#if !NETSTANDARD2_0
        [MemberNotNull(nameof(Values))]
#endif
        private void SetColumnValues(T item)
        {
            // Ensure write buffers are allocated
            if (Values == null || Values.Length != Fields.Count)
                Values = new string[Fields.Count];

            // Get values
            for (int i = 0; i < Fields.Count; i++)
                Values[i] = Fields[i].GetValue(item);
        }
    }
}
