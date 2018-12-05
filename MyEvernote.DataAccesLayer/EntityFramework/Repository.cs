using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccesLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class  //T is only Class type
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = db.Set<T>();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        //to take sequentially in the database
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        //lists according to the required criteria
        public List<T> List(Expression<Func<T, bool>> where ) 
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if(obj is MyEntityBase)
            {
                DateTime now = DateTime.Now;
                MyEntityBase o = obj as MyEntityBase;

                o.CreateOn = now;
                o.ModifiedOn = now;
                o.ModifiedUser = App.Common.GetCurrentUsername();
            }

            return Save();
        }
           
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                DateTime now = DateTime.Now;
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = now;
                o.ModifiedUser = App.Common.GetCurrentUsername();
            }
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        //only one return
        public T Find(Expression<Func<T, bool>> where) 
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
