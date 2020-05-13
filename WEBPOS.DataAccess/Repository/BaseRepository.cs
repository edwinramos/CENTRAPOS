using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WEBPOS.DataAccess.Repository
{
    public abstract class BaseRepository<TContext, TKData> : IRepository<TKData> where TContext : DbContext where TKData : IDisposable
    {
        private bool _disposed;

        public const string FORMAT_STRING = "{0} - {1}";
        public readonly TContext Context;
        public bool IsDisposableContext;

        protected BaseRepository() : this(null) { }

        protected BaseRepository(TContext context = null)
        {
            DbContext ctx = new WEBPOSContext();
            Context = (TContext)ctx;
            IsDisposableContext = true;
        }

        #region IDisposable Members

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing && IsDisposableContext)
                Context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<TKData> ReadAllQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TKData> ReadAllQueryable(TKData value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TKData> Read(TKData value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TKData> ReadAll()
        {
            throw new NotImplementedException();
        }

        IQueryable<TKData> IRepository<TKData>.ReadAllQueryable()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}