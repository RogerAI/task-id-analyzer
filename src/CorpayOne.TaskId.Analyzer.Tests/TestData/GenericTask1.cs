using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpayOne.TaskId.Analyzer.Tests.TestData
{
    internal class GenericTask1
    {
        public static async Task Main()
        {
            var userId = LoadFromDatabase(78);

            if (userId.Id > 10)
            {
                Console.WriteLine("Wrong User Found");
            }
        }

        private static async Task<int> LoadFromDatabase(int id)
        {
            await Task.Delay(1);

            return 7;
        }
    }
}
