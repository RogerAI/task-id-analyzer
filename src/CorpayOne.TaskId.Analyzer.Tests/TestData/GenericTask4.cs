using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpayOne.TaskId.Analyzer.Tests.TestData
{
    internal class GenericTask4
    {
        public static async Task Main()
        {
            ISomeService svc1 = new SomeServiceImpl();

            ISomeOtherService svc2 = new SomeOtherServiceImpl();

            var fish = svc1.GetFish(273);

            var check = await svc2.CheckFish(fish.Id);
        }
    }

    internal class Animal
    {
        public int Id { get; set; }
    }

    internal class Fish : Animal
    {
        public string Species { get; set; }
    }

    internal interface ISomeOtherService
    {
        Task<string> CheckFish(int id);
    }

    internal interface ISomeService
    {
        Task<Fish> GetFish(int id);
    }

    internal class SomeServiceImpl : ISomeService
    {
        public Task<Fish> GetFish(int id)
        {
            return Task.FromResult(new Fish
            {
                Id = id,
                Species = "Nemo"
            });
        }
    }

    internal class SomeOtherServiceImpl : ISomeOtherService
    {
        public async Task<string> CheckFish(int id)
        {
            await Task.Delay(id);

            return "Dory";
        }
    }
}
