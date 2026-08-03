// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;

namespace FixedWidthParser.Tests
{
    public class ObjectMappingTests
    {
        public class Product
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

        public class ProductComparer : IEqualityComparer<Product>
        {
            public bool Equals(Product? x, Product? y)
            {
                if (x is null && y is null) return true;
                if (x is null || y is null) return false;
                return x.Id == y.Id && x.Description == y.Description && x.Category == y.Category && x.Rating == y.Rating;
            }

            public int GetHashCode(Product obj) =>
                HashCode.Combine(obj.Id, obj.Description, obj.Category, obj.Rating);
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

        [Fact]
        public void BasicTests()
        {
            Assert.Equal(Products, WriteReadValues(Products), new ProductComparer());
        }

        #region Support methods

        internal static List<T1> WriteReadValues<T1>(List<T1> items, FixedWidthOptions? options = null, Action<FixedWidthWriter<T1>>? initWriter = null, Action<FixedWidthReader<T1>>? initReader = null) where T1 : class, new()
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
