using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpayOne.TaskId.Analyzer.Tests.TestData
{
    internal class GenericTask6
    {
        public static async Task Main()
        {
            ISomeService svc1 = new SomeServiceImpl();

            var a = 66;
            IFish fish;
            fish = await svc1.GetFish(a);
            Console.WriteLine(fish.Id);
        }
    }

    internal interface IFish
    {
        int Id { get; set; }

        string Type { get; set; }
    }

    public class ZebraFish : IFish
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int StripeThickness { get; set; }
    }

    internal interface ISomeService
    {
        Task<IFish> GetFish(int id);
    }

    internal class SomeServiceImpl : ISomeService
    {
        public async Task<IFish> GetFish(int id)
        {
            await Task.CompletedTask;

            return new ZebraFish
            {
                Id = id,
                Type = "zebra",
                StripeThickness = 6
            };
        }
    }
}
