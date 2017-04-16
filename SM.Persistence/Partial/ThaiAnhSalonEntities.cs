using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Persistence
{
    public partial class ThaiAnhSalonEntities : DbContext, IObjectContextAdapter
    {
        private EventHandler _handler;

        public ThaiAnhSalonEntities(string connectionString):base(connectionString)
        {
            (this as IObjectContextAdapter).ObjectContext.CommandTimeout = 60;
        }

        /// <summary>
        /// Set savingChanges handler to update trail like lastmodified, modifiedby .. 
        /// only need one handler to be set in context 
        /// </summary>
        /// <param name="handler"></param>
        public void SetSavingChangesHandler(EventHandler handler)
        {
            if (_handler == null)
            {
                var oc = this as IObjectContextAdapter;
                oc.ObjectContext.SavingChanges += handler;
                _handler = handler;
            }
        }

        public EventHandler DetachSavingChangesHandler()
        {
            var oc = this as IObjectContextAdapter;
            oc.ObjectContext.SavingChanges -= _handler;
            var temp = _handler;
            _handler = null;
            return temp;

        }

        public void Add(object entity, bool flush = true)
        {

            Entry(entity).State = EntityState.Added;
            if (flush)
                SaveChanges();
        }

        public async void AddAsync(object entity, bool flush = true)
        {
            Entry(entity).State = EntityState.Added;
            if (flush)
                await SaveChangesAsync();
        }

        public void Update(object entity, bool flush = true)
        {
            var entry = Entry(entity);
            if (entry.State != EntityState.Modified)
                Entry(entity).State = EntityState.Modified;
            if (flush)
                SaveChanges();
        }

        public async void UpdateAsync(object entity, bool flush = true)
        {
            var entry = Entry(entity);
            if (entry.State != EntityState.Modified)
                Entry(entity).State = EntityState.Modified;
            if (flush)
                await SaveChangesAsync();
        }

        public bool Delete(object entity, bool flush = true)
        {
            Entry(entity).State = EntityState.Deleted;
            if (flush)
                SaveChanges();
            return true;
        }

        public async Task<bool> DeleteAsync(object entity, bool flush = true)
        {
            Entry(entity).State = EntityState.Deleted;
            if (flush)
                await SaveChangesAsync();
            return true;
        }

        public void Detach(object entity)
        {
            Entry(entity).State = EntityState.Detached;
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public IQueryable QueryReport(Func<IQueryable> lambda)
        {
            return lambda.Invoke();
        }
    }
}
