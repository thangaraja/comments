namespace CommentSystems.Migrations
{
    using CommentSystems.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CommentSystems.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CommentSystems.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("Password@123");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "Gandalf",
                    Email = "gandalf@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "Legolas",
                    Email = "legolas@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "Aragorn",
                    Email = "aragorn@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });
        }
    }
}