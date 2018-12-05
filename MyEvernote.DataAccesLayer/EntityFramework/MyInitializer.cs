using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccesLayer.EntityFramework
{
    class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EUser admin = new EUser()
            {
                Name = "Sinan",
                Surname = "Tok",
                Email = "snntok@gmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "sinantok",
                Password = "123",
                ProfileImageFileName = "user.jpg",
                CreateOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUser = "sinantok"
            };

            EUser standartUser = new EUser()
            {
                Name = "tok",
                Surname = "Tok",
                Email = "snntok1@gmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "tok",
                Password = "123",
                CreateOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUser = "sinantok"
            };

            context.EUsers.Add(admin);
            context.EUsers.Add(standartUser);

            for (int h = 0; h < 8; h++)
            {
                EUser user = new EUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{h}",
                    Password = "123",
                    CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUser = $"user{h}",
                };

                context.EUsers.Add(user);
            }

            context.SaveChanges();

            //user list for using..
            List<EUser> userlist = context.EUsers.ToList();

            //adding fake categories
            for (int i = 0; i < 10; i++)
            {
                ECategory cat = new ECategory()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreateOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUser = "sinantok",
                };

                context.ECategories.Add(cat);

                //adding fake notes
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    EUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    ENote note = new ENote()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUser = owner.Username,
                    };

                    cat.Notes.Add(note);


                    //adding fake comments
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {

                        EUser commnentOwner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        EComment comment = new EComment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = commnentOwner,
                            CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUser = commnentOwner.Username,
                        };

                        note.Comments.Add(comment);
                    }

                    //adding fake likes

                    for (int p = 0; p < note.LikeCount; p++)
                    {
                        ELiked like = new ELiked()
                        {
                            LikedUser = userlist[p]
                        };

                        note.Likes.Add(like);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
