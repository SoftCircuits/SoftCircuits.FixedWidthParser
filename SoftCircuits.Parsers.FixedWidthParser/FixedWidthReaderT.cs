// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftCircuits.Parsers
{
    public class FixedWidthReader<T> : FixedWidthReader where T : class, new()
    {
        private List<MemberDescriptor> MemberDescriptors;

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), filename, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, Encoding encoding, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), filename, encoding, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), filename, detectEncodingFromByteOrderMarks, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(string filename, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), filename, encoding, detectEncodingFromByteOrderMarks, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), stream, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, Encoding encoding, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), stream, encoding, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), stream, detectEncodingFromByteOrderMarks, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
            : base(Enumerable.Empty<FixedWidthField>(), stream, encoding, detectEncodingFromByteOrderMarks, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Initializes the class member descriptors.
        /// </summary>
#if !NETSTANDARD2_0
        [MemberNotNull(nameof(MemberDescriptors))]
#endif
        private void InitializeMemberDescriptors()
        {
            MemberDescriptors = MemberDescriptor.GetMemberDescriptors(typeof(T));
            Debug.Assert(Fields.Count == 0);
            Fields.AddRange(MemberDescriptors);
        }

        /// <summary>
        /// Reads the next row from the current fixed-width file and returns the raw
        /// text values.
        /// </summary>
        /// <returns></returns>
        public string[]? ReadRawValues() => base.Read();

        /// <summary>
        /// Asynchronously reads the next row from the current fixed-width file and returns the raw
        /// text values.
        /// </summary>
        /// <returns></returns>
        public async Task<string[]?> ReadRawValuesAsync() => await base.ReadAsync();

        /// <summary>
        /// Reads the next item from the current fixed-width file.
        /// </summary>
        /// <param name="item">Returns the item read.</param>
        /// <returns>True if successful, false if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
        [Obsolete("This method is deprecated and will be removed in a future version of this library. Please use a version of Read() that takes no parameters.")]
#if !NETSTANDARD2_0
        public bool Read([NotNullWhen(true)] out T? item)
#else
        public bool Read(out T item)
#endif
        {
            item = Read();
            return item != null;
        }

        /// <summary>
        /// Reads the next item from the current fixed-width file.
        /// </summary>
        /// <param name="item">Returns the item read.</param>
        /// <returns>The item read or null if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
        public new T? Read()
        {
            string[]? columns = base.Read();
            if (columns == null)
                return null;

            // Create and populate item
            int memberCount = Math.Min(columns.Length, MemberDescriptors.Count);
            T item = Activator.CreateInstance<T>();
            for (int i = 0; i < memberCount; i++)
                MemberDescriptors[i].SetValue(item, columns[i], Options.ThrowDataException);
            return item;
        }

        /// <summary>
        /// Asynchronously reads the next item from the current fixed-width file.
        /// </summary>
        /// <param name="item">Returns the item read.</param>
        /// <returns>The item read or null if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
        public new async Task<T?> ReadAsync()
        {
            string[]? columns = await base.ReadAsync();
            if (columns == null)
                return null;

            // Create and populate item
            int memberCount = Math.Min(columns.Length, MemberDescriptors.Count);
            T item = Activator.CreateInstance<T>();
            for (int i = 0; i < memberCount; i++)
                MemberDescriptors[i].SetValue(item, columns[i], Options.ThrowDataException);
            return item;
        }

        /// <summary>
        /// Reads all items from the current fixed-width file.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of the items read.</returns>
        public IEnumerable<T> ReadAll()
        {
            T? item;
            while ((item = Read()) != null)
                yield return item;
        }

#if !NETSTANDARD2_0

        /// <summary>
        /// Asynchronously reads all items from the current fixed-width file.
        /// </summary>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of the items read.</returns>
        public async IAsyncEnumerable<T> ReadAllAsync()
        {
            T? item;
            while ((item = await ReadAsync()) != null)
                yield return item;
        }

#endif

    }
}
