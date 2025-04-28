// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SoftCircuits.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FixedWidthParserTests
{
    public class ObjectMappingTests
    {
        private class Product
        {
            [FixedWidthField(36)]
            public Guid Id { get; set; }
            [FixedWidthField(12)]
            public string Description { get; set; }
            [FixedWidthField(12)]
            public string Category { get; set; }
            [FixedWidthField(10)]
            public double Rating { get; set; }

            public Product()
            {
                Description = Category = string.Empty;
            }

            public override string ToString() => $"{Id}/{Category}/{Description}/{Rating}";
        }

        private class ProductComparer : IComparer, IComparer<Product>
        {
            public int Compare(object? a, object? b)
            {
                if (a is not Product ta || b is not Product tb)
                    throw new InvalidOperationException();
                return Compare(ta, tb);
            }

            public int Compare(Product? a, Product? b)
            {
                if (a != null && b != null)
                {
                    int result;

                    result = a.Id.CompareTo(b.Id);
                    if (result != 0) return result;
                    result = a.Description.CompareTo(b.Description);
                    if (result != 0) return result;
                    result = a.Category.CompareTo(b.Category);
                    if (result != 0) return result;
                    result = a.Rating.CompareTo(b.Rating);
                    return result;
                }

                if (a == null && b == null)
                    return 0;
                if (a == null)
                    return -1;
                return 1;
            }
        }

        private readonly List<Product> Products =
        [
            new Product { Id = Guid.NewGuid(), Description = "Coffee Table", Category = "Furniture", Rating = 4.5 },
            new Product { Id = Guid.NewGuid(), Description = "Spoons", Category = "Utensils", Rating = 4.2 },
            new Product { Id = Guid.NewGuid(), Description = "Carpet", Category = "Flooring", Rating = 4.5 },
            new Product { Id = Guid.NewGuid(), Description = "Knives", Category = "Utensils", Rating = 4.7 },
            new Product { Id = Guid.NewGuid(), Description = "Recliner", Category = "Furniture", Rating = 4.5 },
            new Product { Id = Guid.NewGuid(), Description = "Floor Tiles", Category = "Flooring", Rating = 4.5 },
        ];

        [Test]
        public void BasicTests()
        {
            CollectionAssert.AreEqual(Products, WriteReadValues(Products), new ProductComparer());
        }

        #region Support methods

        internal List<T1> WriteReadValues<T1>(List<T1> items, FixedWidthOptions? options = null, Action<FixedWidthWriter<T1>>? initWriter = null, Action<FixedWidthReader<T1>>? initReader = null) where T1 : class, new()
        {
            return WriteReadValues<T1, T1>(items, options, initWriter, initReader);
        }

        internal static List<T2> WriteReadValues<T1, T2>(List<T1> items, FixedWidthOptions? options = null, Action<FixedWidthWriter<T1>>? initWriter = null, Action<FixedWidthReader<T2>>? initReader = null) where T1 : class, new() where T2 : class, new()
        {
            List<T2> results;

            MemoryFile memFile = new();

            using (FixedWidthWriter<T1> writer = new(memFile.GetStream(), options))
            {
                initWriter?.Invoke(writer);
                writer.Write(items);
            }

            using FixedWidthReader<T2> reader = new(memFile.GetStream(), options);
            initReader?.Invoke(reader);
            results = [.. reader.ReadAll()];

            return results;
        }

        #endregion

    }
}
