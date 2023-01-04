using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpayOne.TaskId.Analyzer.Tests.TestData
{
    internal class GenericTask5
    {
        public static async Task Main()
        {
            ISomeService svc1 = new SomeServiceImpl();

            var a = 7;
            Fish fish;
            if (a % 2 == 0)
            {
                fish = new Fish();
            }
            else
            {
                fish = await svc1.GetFish(a);
                Console.WriteLine(fish.Id);
            }
        }
    }

    internal class Fish
    {
        public int Id { get; set; }

        public string Species { get; set; }
    }

    internal interface ISomeService
    {
        Task<Fish> GetFish(int id);
    }

    internal class SomeServiceImpl : ISomeService
    {
        public async Task<Fish> GetFish(int id)
        {
            await Task.CompletedTask;

            return new Fish
            {
                Id = id,
                Species = "Nemo"
            };
        }
    }
}
