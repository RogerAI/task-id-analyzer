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
            var tasks = new[] { GetProductKey(6), GetProductKey(7) };

            var id = tasks[0].Id;

            Console.WriteLine(id);
        }

        private static async Task<int> GetProductKey(int key)
        {
            await Task.Delay(4);

            return 7 + key;
        }
    }
}
