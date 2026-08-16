// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;

namespace FixedWidthParser.Tests
{
    public class AsyncTests
    {
        public class Employee
        {
            [FixedWidthField(10)]
            public string Name { get; set; } = string.Empty;
            [FixedWidthField(5)]
            public int Age { get; set; }
        }

        private readonly List<Employee> Employees =
        [
            new Employee { Name = "Alice", Age = 30 },
            new Employee { Name = "Bob", Age = 45 },
            new Employee { Name = "Carol", Age = 27 },
        ];

        /// <summary>
        /// FixedWidthWriter&lt;T&gt;.WriteAsync/FixedWidthReader&lt;T&gt;.ReadAllAsync round-trip
        /// correctly. This exercises the async call chain specifically at the generic level -
        /// the existing async coverage (StandardTests.BasicTestAsync) only exercises the
        /// non-generic reader/writer's async methods.
        /// </summary>
        [Fact]
        public async Task GenericAsyncReadWrite_RoundTrips()
        {
            MemoryFile memFile = new();

            using (FixedWidthWriter<Employee> writer = new(memFile.GetStream()))
                await writer.WriteAsync(Employees);

            List<Employee> results = [];
            using (FixedWidthReader<Employee> reader = new(memFile.GetStream()))
            {
                await foreach (Employee employee in reader.ReadAllAsync())
                    results.Add(employee);
            }

            Assert.Equal(Employees.Count, results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(Employees[i].Name, results[i].Name);
                Assert.Equal(Employees[i].Age, results[i].Age);
            }
        }
    }
}
