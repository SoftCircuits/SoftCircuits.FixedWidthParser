// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System.Text;

namespace FixedWidthParser.Tests
{
    /// <summary>
    /// Helper class for reading and writing to memory files.
    /// </summary>
    public class MemoryFile
    {
        private MemoryStream? Stream;

        /// <summary>
        /// Gets a memory stream for reading or writing.
        /// </summary>
        /// <param name="loadLastStreamContent">If true, the returned stream is loaded with the content
        /// written to the previous stream returned by this method.</param>
        public Stream GetStream(bool loadLastStreamContent = true)
        {
            MemoryStream? oldStream = Stream;
            Stream = new();
            if (loadLastStreamContent && oldStream != null)
            {
                Stream.Write(oldStream.ToArray());
                Stream.Seek(0, SeekOrigin.Begin);
            }
            return Stream;
        }

        /// <summary>
        /// Gets the content of the most recent stream returned by <see cref="GetStream(bool)"/>.
        /// </summary>
        public string GetContent()
        {
            if (Stream != null)
            {
                byte[] buffer = Stream.ToArray();
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            return string.Empty;
        }
    }
}
