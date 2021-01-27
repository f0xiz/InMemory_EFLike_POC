using System;

namespace DBCore.Model
{
    public class Event
    {
        public int Id { get; set; }
        
        public int SecondId { get; set; }
        
        public int ThirdId { get; set; }
        public string Name { get; set; }
        
        public DateTime Date { get; set; }
    }
}