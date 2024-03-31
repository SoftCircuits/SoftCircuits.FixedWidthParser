// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class to read fixed-width fields from a file and map them to class objects.
    /// </summary>
    /// <typeparam name="T">The types fields are mapped to.</typeparam>
    public class FixedWidthReader<T> : FixedWidthReader where T : class, new()
    {
        /// <summary>
        /// Gets the item read after <see cref="Read()"/> or <see cref="ReadAsync()"/> return true.
        /// </summary>
        public T? Item { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), filename, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, Encoding encoding, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), filename, encoding, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), filename, detectEncodingFromByteOrderMarks, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), filename, encoding, detectEncodingFromByteOrderMarks, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), stream, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, Encoding encoding, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), stream, encoding, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), stream, detectEncodingFromByteOrderMarks, options)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(MappedFixedWidthField.MapFields(typeof(T)), stream, encoding, detectEncodingFromByteOrderMarks, options)
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
        /// Reads the next row from the current fixed-width file and returns the raw text values.
        /// </summary>
        /// <returns>The column values read or null if no more rows could be read.</returns>
        public string[]? ReadRawValues() => base.Read() ? Values : null;

        /// <summary>
        /// Asynchronously reads the next row from the current fixed-width file and returns the raw
        /// text values.
        /// </summary>
        /// <returns>The column values read or null if no more rows could be read.</returns>
        public async Task<string[]?> ReadRawValuesAsync() => await base.ReadAsync() ? Values : null;

        /// <summary>
        /// Reads the next item from the current fixed-width file.
        /// </summary>
        /// <param name="item">Returns the item read.</param>
        /// <returns>True if successful, false if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
#if NETSTANDARD2_0
        public bool Read(out T item)
#else
        public bool Read([NotNullWhen(true)] out T? item)
#endif
        {
            if (Read())
            {
                item = Item;
                return true;
            }
            item = null;
            return false;
        }

        /// <summary>
        /// Reads the next item from the current fixed-width file and stores it in the <see cref="P:Item"/> property.
        /// </summary>
        /// <returns>True if successful, false if no more rows could be read.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, nameof(Item))]
#endif
        public new bool Read()
        {
            if (base.Read())
            {
                Item = CreateItem(Values);
                return true;
            }
            Item = null;
            return false;
        }

        // Note: There is a bug displaying a <see> tag with the name Item. The P (property) prefix causes it to display
        // correctly but not highlighted. https://github.com/dotnet/roslyn/issues/65017#issuecomment-1293925310

        /// <summary>
        /// Asynchronously reads the next item from the current fixed-width file and stores it in the <see cref="P:Item"/> property.
        /// </summary>
        /// <remarks>
        /// Note: The <see cref="P:Item"/> property is guaranteed not to be null when this method returns true. However, .NET does not currently
        /// support the <see cref="MemberNotNullWhenAttribute"/> attribute for async methods. So the compiler may generate warnings
        /// when nullable reference types are enabled. In this case, you can safely use the null-forgiving operator (!) when this
        /// method returns true.
        /// </remarks>
        /// <returns>True if successful, false if no more rows could be read.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, nameof(Item))] // Note: Ignored for async method
#endif
        public new async Task<bool> ReadAsync()
        {
            if (await base.ReadAsync())
            {
                Debug.Assert(Values != null);
                Item = CreateItem(Values);
                return true;
            }
            Item = null;
            return false;
        }

        /// <summary>
        /// Reads all items from the current fixed-width file.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of the items read.</returns>
        public IEnumerable<T> ReadAll()
        {
            while (Read())
                yield return Item;
        }

#if !NETSTANDARD2_0

        /// <summary>
        /// Asynchronously reads all items from the current fixed-width file.
        /// </summary>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of the items read.</returns>
        public async IAsyncEnumerable<T> ReadAllAsync()
        {
            while (await ReadAsync())
            {
                Debug.Assert(Item != null);
                yield return Item;
            }
        }

#endif

        /// <summary>
        /// Creates an instance of <typeparamref name="T"/> from the given field values.
        /// </summary>
        private T CreateItem(string[] values)
        {
            T item = new();

            for (int i = 0; i < Math.Min(Fields.Count, values.Length); i++)
                Fields[i].SetValue(item, values[i], Options.ThrowDataException);

            return item;
        }
    }
}
