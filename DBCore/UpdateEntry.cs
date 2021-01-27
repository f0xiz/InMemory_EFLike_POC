using DBCore;

namespace DBCorev2
{
    public class UpdateEntry
    {
        public EntityType entityType { get; set; }
        
        public EntityState entryState { get; set; }
        
        public object entry { get; set; }
    }
    
    public enum EntityState
    {
        Detached = 0,
        Unchanged = 1,
        Deleted = 2,
        Modified = 3,
        Added = 4
    }
}