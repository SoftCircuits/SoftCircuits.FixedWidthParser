// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;

namespace FixedWidthParser.Tests
{
    public class FixedWidthFieldAttributeTests
    {
        private class Product
        {
            [FixedWidthField(12, Alignment = FieldAlignment.Right, PadCharacter = '+', TrimField = true)]
            public string Description { get; set; }
            [FixedWidthField(12, Alignment = FieldAlignment.Left, PadCharacter = '@', TrimField = false)]
            public string Category { get; set; }
            [FixedWidthField(12, Alignment = FieldAlignment.Right, PadCharacter = '*', TrimField = false)]
            public string Color { get; set; }

            public Product()
            {
                Description = Category = Color = string.Empty;
            }

            public override string ToString() => $"{Category}/{Description}/{Color}";
        }

        private readonly List<Product> Products =
        [
            new Product { Description = "Coffee Table", Category = "Furniture", Color = "Brown/Black" },
            new Product { Description = "Spoons", Category = "Utensils", Color = "Silver" },
            new Product { Description = "Carpet", Category = "Flooring", Color = "Tan" },
            new Product { Description = "Knives", Category = "Utensils", Color = "Silver" },
            new Product { Description = "Recliner", Category = "Furniture", Color = "Maroon" },
            new Product { Description = "Floor Tiles", Category = "Flooring", Color = "Tan" },
        ];

        private readonly List<Product> Results =
        [
            new Product { Description = "Coffee Table", Category = "Furniture@@@", Color = "*Brown/Black" },
            new Product { Description = "Spoons", Category = "Utensils@@@@", Color = "******Silver" },
            new Product { Description = "Carpet", Category = "Flooring@@@@", Color = "*********Tan" },
            new Product { Description = "Knives", Category = "Utensils@@@@", Color = "******Silver" },
            new Product { Description = "Recliner", Category = "Furniture@@@", Color = "******Maroon" },
            new Product { Description = "Floor Tiles", Category = "Flooring@@@@", Color = "*********Tan" },
        ];

        [Fact]
        public void AttributeTests()
        {
            MemoryFile memFile = new();

            // Write using data mapping/attributes
            using (FixedWidthWriter<Product> writer = new(memFile.GetStream()))
            {
                foreach (var product in Products)
                    writer.Write(product);
            }

            List<Product> results = [];
            using FixedWidthReader<Product> reader = new(memFile.GetStream());
            {
                while (reader.Read())
                    results.Add(reader.Item);
            }

            Assert.Equal(Results.Count, results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(Results[i].Description, results[i].Description);
                Assert.Equal(Results[i].Category, results[i].Category);
                Assert.Equal(Results[i].Color, results[i].Color);
            }
        }
    }
}
