using System.Data.Entity;

namespace TV.MeanChords.Data.Db
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=ModelContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
