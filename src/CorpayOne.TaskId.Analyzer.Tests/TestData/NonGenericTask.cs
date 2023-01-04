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
            var something = Green();

            if (DateTime.UtcNow.Year > 1000)
            {
                Console.WriteLine(something.Id);
            }
        }

        private static async Task Green()
        {
            await Task.Delay(4);
        }
    }
}
