using System;
using BenchmarkDotNet.Running;
using DBCore.Model;
using DBCorev2;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ClientDbContext();
            
            db.Events.Add(new Event(){Id = 5, Date = DateTime.Now, Name = "Dima", SecondId = 10, ThirdId = 11});
            db.Events.Add(new Event(){Id = 9, Date = DateTime.Now, Name = "Dima12", SecondId = 10, ThirdId = 12});

            var result1 = db.Events.FindByKey(5);
            var result2 = db.Events.FindByKey(9);

            //var result3 = db.Events.FindByIndex(x => x.SecondId, 10);
            //var result4 = db.Events.FindByIndex(x => x.Name, "Dima");
            //var result5 = db.Events.FindByIndex(x => new {x.SecondId, x.ThirdId}, new {SecondId = 10, ThirdId = 12});
            
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}