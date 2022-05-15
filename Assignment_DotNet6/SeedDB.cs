using Assignment_DotNet6.Data;
using Assignment_DotNet6.Entities;
using Microsoft.AspNetCore.Identity;

namespace Assignment_DotNet6
{
    public class SeedDB
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Doctor>>();

            context.Database.EnsureCreated();

            var users = userManager.Users;


            #region Create User
            if (!context.Doctors.Any() || !context.Doctors.Any())
            {
                try
                {
                    var user = new Doctor()
                    {
                        Name = "Doctor",
                        D_ID = Guid.NewGuid(),
                        Email = "Doctor@gmail.com",
                        UserName = "Doctor1",
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };
                    await userManager.CreateAsync(user, "Test@123");
                }
                catch (Exception xw)
                {
                }
            }
            #endregion
            // SeedDB.DeleteAllData(serviceProvider);
        }

        public static void DeleteAllData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataContext>();

            context.Database.EnsureCreated();
            context.Doctors.RemoveRange(context.Doctors);
            context.Patients.RemoveRange(context.Patients);
            context.Visits.RemoveRange(context.Visits);
            context.SaveChanges();


        }

    }
}
