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

namespace SoftCircuits.Parsers
{
    public class FixedWidthReader<T> : FixedWidthReader where T : class, new()
    {
        private List<MemberDescriptor> MemberDescriptors;
        private string[]? ReadValues;

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
#if NET5_0
        [MemberNotNull(nameof(MemberDescriptors))]
        //[MemberNotNull(nameof(MemberDescriptors))]
#endif
        private void InitializeMemberDescriptors()
        {
            MemberDescriptors = MemberDescriptor.GetMemberDescriptors(typeof(T));
            Debug.Assert(Fields.Count == 0);
            Fields.AddRange(MemberDescriptors);
        }

        /// <summary>
        /// Reads the next item from the fixed-width file.
        /// </summary>
        /// <param name="item">Returns the item read.</param>
        /// <returns>True if successful, false if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Please use Read() instead.")]
#if NETSTANDARD2_0
        public bool ReadItem(out T item) => ReadItem(out item);
#else
        public bool ReadItem([MaybeNullWhen(false)] out T item) => Read(out item);
#endif

        /// <summary>
        /// Reads the next item from the current fixed-width file.
        /// </summary>
        /// <param name="item">Returns the item read.</param>
        /// <returns>True if successful, false if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthDataException"></exception>
#if NETSTANDARD2_0
        public bool Read(out T item)
#else
        public bool Read([MaybeNullWhen(false)] out T item)
#endif
        {
            // Ensure read buffers are allocated
            if (ReadValues == null || ReadValues.Length != MemberDescriptors.Count)
                ReadValues = new string[MemberDescriptors.Count];

            // Get values
            if (!Read(ref ReadValues))
            {
                item = null;
                return false;
            }

            // Create and populate item
            item = Activator.CreateInstance<T>();
            for (int i = 0; i < MemberDescriptors.Count; i++)
                MemberDescriptors[i].SetValue(item, ReadValues[i], Options.ThrowDataException);

            return true;
        }

        /// <summary>
        /// Reads all items from the current fixed-width file.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that iterates the items read.</returns>
        public IEnumerable<T> ReadAll()
        {
            while (Read(out T? item))
                yield return item;
        }
    }
}
