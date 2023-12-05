using FieldRent.Entity;
using Microsoft.EntityFrameworkCore;

namespace FieldRent.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }



                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "muhammed", Name = "Muhammed Koç", Email = "info@muhammed.com", Password = "123456", UserPrice=0, },
                        new User { UserName = "said", Name = "Said Darıcı", Email = "info@said.com", Password = "123456", UserPrice=0, }
                    );
                    context.SaveChanges();
                }




                    context.Fields.AddRange(
                        new Field
                        {
                            FieldCoordinate = "FirstArea",
                            FieldImage = "FirstArea",
                            FieldIsActive = true,
                        });




                if (!context.Maps.Any())
                {
                    context.Maps.AddRange(
                        new Map
                        {
                            MapCoordinate = "a1",
                            MapPrice = 100,
                            MapCondition = "Boş",
                            MapUrl = "a1",
                            MapIsActive = true,
                            MapStart = DateTime.Now.AddDays(-1),
                            MapStop = DateTime.Now.AddDays(-10),
                            Requests = context.Requests.Take(1).ToList(),
                            UserId = 1,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "b2",
                            MapPrice = 200,
                            MapCondition = "Nadas",
                            MapUrl = "b2",
                            MapIsActive = true,
                            MapStart = DateTime.Now.AddDays(-10),
                            MapStop = DateTime.Now.AddDays(-20),
                            Requests = context.Requests.Take(1).ToList(),
                            UserId = 2,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "c3",
                            MapPrice = 300,
                            MapCondition = "Nadas",
                            MapUrl = "c3",
                            MapIsActive = true,
                            MapStart = DateTime.Now.AddDays(-20),
                            MapStop = DateTime.Now.AddDays(-30),
                            Requests = context.Requests.Take(1).ToList(),
                            UserId = 2,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "c4",
                            MapPrice = 400,
                            MapCondition = "Nadas",
                            MapUrl = "c4",
                            MapIsActive = true,
                            MapStart = DateTime.Now.AddDays(-20),
                            MapStop = DateTime.Now.AddDays(-30),
                            Requests = context.Requests.Take(1).ToList(),
                            UserId = 2,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "c5",
                            MapPrice = 500,
                            MapCondition = "Nadas",
                            MapUrl = "c5",
                            MapIsActive = true,
                            MapStart = DateTime.Now.AddDays(-20),
                            MapStop = DateTime.Now.AddDays(-30),
                            Requests = context.Requests.Take(1).ToList(),
                            UserId = 2,
                            FieldId=1
                        }



                    );
                    context.SaveChanges();
                }


                if (!context.Requests.Any())
                {
                    context.Requests.AddRange(
                        new Request
                        {
                            RequestName = "AAA",
                            RequestPrice = 1,
                            RequestStart = DateTime.Now.AddDays(-10),
                            RequestStop = DateTime.Now.AddDays(-20),
                            RequestIsActive = true,

                            Maps = context.Maps.Take(1).ToList(),
                        },

                        new Request
                        {
                            RequestName = "BBB",
                            RequestPrice = 2,
                            RequestStart = DateTime.Now.AddDays(-20),
                            RequestStop = DateTime.Now.AddDays(-30),
                            RequestIsActive = true,

                            Maps = context.Maps.Take(1).ToList(),
                        },
                        new Request
                        {
                            RequestName = "CCC",
                            RequestPrice = 3,
                            RequestStart = DateTime.Now.AddDays(-30),
                            RequestStop = DateTime.Now.AddDays(-40),
                            RequestIsActive = true,

                            Maps = context.Maps.Take(2).ToList(),
                        }
                    );
                    context.SaveChanges();
                }







            }
        }
    }
}