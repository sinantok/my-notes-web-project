using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccesLayer.EntityFramework
{
    public class DatabaseContext: DbContext
    {
        public DbSet<EUser> EUsers { get; set; }
        public DbSet<ENote> ENotes { get; set; }
        public DbSet<ECategory> ECategories { get; set; }
        public DbSet<EComment> EComments { get; set; }
        public DbSet<ELiked> ELikes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
    }
}
