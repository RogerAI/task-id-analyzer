using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CorpayOne.TaskId.Analyzer.Tests
{
    internal class TestFile1
    {
        public static async Task Main()
        {
            var prod = await GetProductByKey(7);

            Console.WriteLine(prod.Id);
        }

        private static async Task<Product> GetProductByKey(int key)
        {
            await Task.Delay(4);

            return new Product
            {
                Id = key,
                Name = "Product " + key
            };
        }
    }

    internal class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}