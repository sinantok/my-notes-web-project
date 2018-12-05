using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccesLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext db;

        protected RepositoryBase()
        {
            CreateContext();
        }

        private static object LockSync = new object();

        private static void CreateContext()
        {
            if(db == null)
            {
                lock(LockSync)
                {
                    if (db == null)
                    {
                        db = new DatabaseContext(); //even in multi-use structures only 1 context 
                    }
                }
            }
        }
    }
}
