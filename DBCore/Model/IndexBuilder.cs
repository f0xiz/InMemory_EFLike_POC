namespace DBCore.Model
{
    public class IndexBuilder<TEntity> : EntityTypeBuilder<TEntity>
    {
        private readonly IndexModel _indexModel;

        public IndexBuilder(EntityType entityType, IndexModel indexModel)
           : base(entityType)
        {
            _indexModel = indexModel;
        }

        public IndexBuilder<TEntity> IsUnique()
        {
            _indexModel.IsUnique = true;
            return this;
        }
    }
}