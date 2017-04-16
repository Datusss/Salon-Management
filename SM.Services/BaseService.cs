using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;
using System.Collections;
using System.Reflection;

namespace SM.Services
{
    public class BaseService<TEntity> where TEntity : class
    {
        public ThaiAnhSalonEntities Db { get; set; }

        public BaseService(ThaiAnhSalonEntities db)
        {
            Db = db;
        }

        #region blocking CRUD

        public async Task FlushAsync()
        {
            await Db.SaveChangesAsync();
        }

        /// <summary>
        /// Internal method to get unguarded data source
        /// </summary>
        /// <returns></returns>        
        public virtual IQueryable<TEntity> GetAll()
        {
            return Db.GetAll<TEntity>();
        }

        public void Flush()
        {
            Db.SaveChanges();
        }

        /// <summary>
        /// Add an item to DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        /// <param name="flush">Save item to DB if flush is true</param>        
        public virtual void Add(TEntity entity, bool flush = true)
        {
            Db.Add(entity, flush);
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="entity">entity object</param>
        /// <param name="flush">>Save item to DB if flush is true</param>        
        public void Update(TEntity entity, bool flush = true)
        {
            Db.Update(entity, false);
            if (flush)
            {
                Db.SaveChanges();
            }

        }

        /// <summary>
        /// Remove an item from DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        /// <param name="flush">Remove item form DB</param>
        public void Delete(TEntity entity, bool flush = true)
        {

            Db.Delete(entity, flush);
            //    Log.Warn(string.Format("{0} Id removed by user {1}", _baseType.Name, AuthService.Context.User.Id));
        }

        public void BatchDelete(IEnumerable<TEntity> entities, bool flush = true)
        {

            Db.Set<TEntity>().RemoveRange(entities);
            // Log.WarnFormat("Batch delete on {0} done by user {1} ", _baseType.Name, AuthService.Context.User.Id);
            if (flush)
                Db.SaveChanges();
        }

        #endregion

        #region non-blocking CRUD
        /*******
         * Async alternatives. There is no option of flush. 'cause w/o flusing, the operation is 
         * non-blocking
         *******/
        public virtual async Task DeleteAsync(TEntity entity)
        {

            Delete(entity, false);
            await Db.SaveChangesAsync();

        }

        public virtual async Task UpdateAsync(TEntity entity)
        {

            Update(entity, false);
            await Db.SaveChangesAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {

            Db.Add(entity, false);
            await Db.SaveChangesAsync();

        }
        #endregion

        #region helper
        public TEntity DetachByClone(TEntity entity, string[] includes = null)
        {
            return (TEntity)_detachByClone(entity, typeof(TEntity), includes);
        }

        private object _detachByClone(object entity, Type type, string[] includes = null)
        {
            //var type = typeof(TEntity);
            var clone = type.GetConstructor(new Type[] { type }).Invoke(new object[] { entity });
            if (includes != null)
            {

                foreach (var p in includes)
                {
                    var prop = type.GetProperty(p);

                    if (prop != null && prop.CanRead)
                    {
                        var buffer = prop.GetValue(entity);
                        if (buffer == null)
                            continue;
                        if (prop.PropertyType.Namespace.IndexOf("Collection") >= 0)
                        {
                            Type sample = prop.PropertyType.GetGenericArguments().FirstOrDefault();

                            if (sample != null && sample.Namespace.IndexOf("SM.Persistence") >= 0)
                            {
                                var list = (IList)typeof(List<>).MakeGenericType(sample).GetConstructor(Type.EmptyTypes).Invoke(null);
                                ConstructorInfo con = sample.GetConstructor(new Type[] { sample });
                                foreach (var o in buffer as IEnumerable)
                                {
                                    list.Add(con.Invoke(new object[] { o }));
                                }
                                prop.SetValue(clone, list);
                            }

                            //prop.SetValue(clone, buffer);

                        }
                        else if (prop.PropertyType.Namespace.IndexOf("SM.Persistence") >= 0)
                        {
                            var copy = prop.PropertyType.GetConstructor(new Type[] { prop.PropertyType }).Invoke(new object[] { buffer });
                            prop.SetValue(clone, copy);
                        }
                        else if (prop.PropertyType.Namespace.IndexOf("DynamicProxies") >= 0)
                        {
                            //do what 
                        }
                        else
                        {
                            //should do a check for primitive type here 
                            prop.SetValue(clone, buffer);
                        }
                    }
                }

            }
            return clone;
        }

        public IList<TEntity> DetachByClone(IList<TEntity> entitySet, string[] includes = null)
        {
            var type = typeof(TEntity);
            return entitySet.Select(e => (TEntity)_detachByClone(e, type, includes)).ToList();
        }

        public IList<object> DetachByClone(IList<object> entitySet, string[] includes = null)
        {
            var set = new List<object>();
            Type t = null;
            foreach (var e in entitySet)
            {
                if (t == null)
                {
                    t = e.GetType();
                }
                set.Add(_detachByClone(e, t, includes));
            }
            return set;
        }

        /// <summary>
        /// Detach an entity from dbcontext to avoid auto loading of navigation property
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitySet">Attached set of entity</param>
        /// <param name="dbService">Any db service, main point is to get the dbcontext object and do detach</param>
        /// <param name="relations">Nav. property to be loaded eagerly</param>
        /// <returns>Detached set </returns>
        public IList<TEntity> Detach(IList<TEntity> entitySet, string[] relations = null)
        {
            IDictionary<string, PropertyInfo> info = null;
            Type t = typeof(TEntity);
            if (relations != null)
            {
                info = new Dictionary<string, PropertyInfo>();
                foreach (var r in relations)
                {
                    info[r] = t.GetProperty(r);
                }
            }
            foreach (var e in entitySet)
            {
                //load relation
                Detach(e, info, relations);
            }
            return entitySet;
        }

        private TEntity Detach(TEntity e, IDictionary<string, PropertyInfo> relationInfo, string[] relations)
        {
            IDictionary<PropertyInfo, object> toRejoin = null;
            if (relations != null)
            {
                //force loading 
                //var type = typeof(TEntity);
                toRejoin = new Dictionary<PropertyInfo, object>();
                foreach (var r in relations)
                {
                    PropertyInfo prop = relationInfo[r];//type.GetProperty(r);
                    if (prop != null && prop.CanRead)
                    {
                        var buffer = prop.GetValue(e);

                        //var a = typeof (buffer).GetConstructor();
                        if (buffer == null) continue;


                        if (prop.PropertyType.Namespace.IndexOf("Collection") >= 0)
                        {
                            toRejoin.Add(prop, buffer);
                        }
                        else if (prop.PropertyType.Namespace.IndexOf("DynamicProxies") >= 0 || prop.PropertyType.Namespace.IndexOf("QCViet.Persistence") >= 0)
                        {
                            //even when disable proxy 
                            //detaching an object make it lose navigation property :(
                            Db.Detach(buffer);
                            toRejoin.Add(prop, buffer);
                        }
                    }
                }
            }
            Db.Detach(e);
            try
            {
                if (toRejoin != null)
                {
                    foreach (var pair in toRejoin)
                    {
                        pair.Key.SetValue(e, pair.Value);
                    }
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

            return e;
        }

        public TEntity Detach(TEntity e, string[] relations = null)
        {
            if (relations != null)
            {
                Type t = typeof(TEntity);
                IDictionary<string, PropertyInfo> info = new Dictionary<string, PropertyInfo>();
                foreach (var r in relations)
                {
                    info[r] = t.GetProperty(r);
                }
                Detach(e, info, relations);
            }
            else
            {
                Detach(e, null, relations);
            }
            return e;
        }

        public void DisableProxy()
        {
            Db.Configuration.ProxyCreationEnabled = false;
        }

        public void DisableTrackChange()
        {
            Db.Configuration.AutoDetectChangesEnabled = false;
        }

        public void DisableLazyLoading()
        {
            Db.Configuration.LazyLoadingEnabled = false;
        }
        #endregion
    }
}
