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
                        });




                if (!context.Maps.Any())
                {
                    context.Maps.AddRange(
                        new Map
                        {
                            MapCoordinate = "a1",
                            MapPrice = 100,
                            MapCondition = "Boş",
                            MapIsActive = true,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "b2",
                            MapPrice = 200,
                            MapCondition = "Nadas",
                            MapIsActive = true,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "c3",
                            MapPrice = 300,
                            MapCondition = "Nadas",
                            MapIsActive = true,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "c4",
                            MapPrice = 400,
                            MapCondition = "Nadas",
                            MapIsActive = true,
                            FieldId=1
                        },
                        new Map
                        {
                            MapCoordinate = "c5",
                            MapPrice = 500,
                            MapCondition = "Nadas",
                            MapIsActive = true,
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


                        },

                        new Request
                        {
                            RequestName = "BBB",
                            RequestPrice = 2,


                        },
                        new Request
                        {
                            RequestName = "CCC",
                            RequestPrice = 3,


                        }
                    );
                    context.SaveChanges();
                }







            }
        }
    }
}