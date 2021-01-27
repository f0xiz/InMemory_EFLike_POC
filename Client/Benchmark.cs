using System;
using BenchmarkDotNet.Attributes;
using DBCore.Model;

namespace Client
{
    public class Benchmark
    {
        public ClientDbContext Db = new ();

        [IterationSetup(Target = nameof(BenchmarkWrite))]
        public void IterationSetupWrite()
        {
            Db = new ClientDbContext();
        }

        [Benchmark]
        public void BenchmarkWrite()
        {
            for (int i = 0; i < 100000; i++)
            {
                Db.Events.Add(new Event(){Id = i, SecondId = i / 10, ThirdId = i + 1 , Date = DateTime.Now, Name = "Dima" + i / 20 });
            }
        }
        
        
        // [GlobalSetup(Targets = new [] {  nameof(BenchmarkReadIndex) })]
        // public void GlobalSetupRead()
        // {
        //     Db = new ClientDbContext();
        //     for (int i = 0; i < 100000; i++)
        //     {
        //         Db.Events.Add(new Event(){Id = i, SecondId = i / 10, ThirdId = i + 1, Date = DateTime.Now, Name = "Dima" + i / 20 });
        //     }
        // }

        // [Benchmark]
        // public void BenchmarkReadKey()
        // {
        //     for (int i = 0; i < 100000; i++)
        //     {
        //         Db.Events.FindByKey(i);
        //     }
        // }
        
        // [Benchmark]
        // public void BenchmarkReadIndex()
        // {
        //     var exp = MakeExpression(e => new {e.SecondId, e.ThirdId});
        //     
        //     for (int i = 0; i < 100000; i++)
        //     {
        //         Db.Events.FindByIndex(exp, new {SecondId = i / 10, ThirdId = i + 1});
        //     }
        // }
        //
        // public Expression<Func<Event, TProperty>> MakeExpression<TProperty>(
        //     Expression<Func<Event, TProperty>> indexExpression)
        // {
        //     return indexExpression;
        // }
        
    }
}