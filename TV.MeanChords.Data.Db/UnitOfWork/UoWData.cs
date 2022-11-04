using System;

namespace TV.MeanChords.Data.Db.UnitOfWork
{
    public class UoWData : IDisposable
    {
        public static UoWData Create() => new UoWData();

        public UoWData()
        {
            Context = new ModelContext();
        }

        private ModelContext Context { get; set; }

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
