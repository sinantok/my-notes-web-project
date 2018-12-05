using MyEvernote.DataAccesLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        
        private Repository<EUser> repoUser = new Repository<EUser>();
        private Repository<ECategory> repoCategory = new Repository<ECategory>();
        private Repository<EComment> repoComment = new Repository<EComment>();
        private Repository<ENote> repoNote = new Repository<ENote>();

        public Test()
        {
            //DataAccesLayer.DatabaseContext db = new DataAccesLayer.DatabaseContext();
            //db.ECategories.ToList();

            //List<ECategory> categories = repoCategory.List(); //toplu list
            //List<ECategory> categoriesFilter = repoCategory.List(x => x.Id > 5);//koşullu listeleme
        }

        public void UpdateTest()
        {
            EUser user = repoUser.Find(x => x.Username == "tokk");

            if(user!=null)
            {
                user.Username = "tokkk";
                int result = repoUser.Update(user);
            }

        }

        public void CommnetTest()
        {
            EUser user = repoUser.Find(x => x.Id == 1);
            ENote note = repoNote.Find(x => x.Id == 3);

            EComment comment = new EComment()
            {
                Text = "Deneme Yorumu",
                CreateOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUser = "sinantok",
                Note = note,
                Owner = user,
            };
            int a = repoComment.Insert(comment);
        }
    }
}
