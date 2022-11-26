using System;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Utils.Repository;

namespace TV.MeanChords.Data.Db.UnitOfWork
{
    public class UoWDiscosChowell : IDisposable
    {
        public static UoWDiscosChowell Create() => new UoWDiscosChowell();

        public UoWDiscosChowell()
        {
            Context = new DiscosChowellEntities();
            AddressRepository = new GenericRepository<Address>(Context);
            AuthorRepository = new GenericRepository<Author>(Context);
            DiscRepository = new GenericRepository<Disc>(Context);
            DiscTagRepository = new GenericRepository<DiscTag>(Context);
            ReportRepository = new GenericRepository<Report>(Context);
            SaleRepository = new GenericRepository<Sale>(Context);
            TagRepository = new GenericRepository<Tag>(Context);
            UserRepository = new GenericRepository<User>(Context);
            SaleDiscRepository = new GenericRepository<SaleDisc>(Context);
        }

        private DiscosChowellEntities Context { get; set; }
        public GenericRepository<Address> AddressRepository { get; set; }
        public GenericRepository<Author> AuthorRepository { get; set; }
        public GenericRepository<Disc> DiscRepository { get; set; }
        public GenericRepository<DiscTag> DiscTagRepository { get; set; }
        public GenericRepository<Report> ReportRepository { get; set; }
        public GenericRepository<Sale> SaleRepository { get; set; }
        public GenericRepository<Tag> TagRepository { get; set; }
        public GenericRepository<User> UserRepository { get; set; }
        public GenericRepository<SaleDisc> SaleDiscRepository { get; set; }
        public GenericRepository<ShoppingCar> ShoppingCarRepository { get; set; }

        public void Save()
        {
            Context.SaveChanges();
        }

        private bool Disposed = false;

        protected virtual void Dispose(bool Disposing)
        {
            if (!this.Disposed)
            {
                if (Disposing)
                {
                    Context.Dispose();
                }
            }
            Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
