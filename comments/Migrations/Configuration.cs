namespace CommentSystems.Migrations
{
    using CommentSystems.Constants;
    using CommentSystems.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            AddUsers(context);
        }


        private void AddUsers(ApplicationDbContext context)
        {
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("Password@123");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfGandalf,
                    UserName = "gandalf@lotr.com",
                    Email = "gandalf@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfLegolas,
                    UserName = "legolas@lotr.com",
                    Email = "legolas@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfAragorn,
                    UserName = "aragorn@lotr.com",
                    Email = "aragorn@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfFrodo,
                    UserName = "frodo@lotr.com",
                    Email = "frodo@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfMerry,
                    UserName = "merry@lotr.com",
                    Email = "merry@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfPippin,
                    UserName = "pippin@lotr.com",
                    Email = "pippin@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfGimli,
                    UserName = "gimli@lotr.com",
                    Email = "gimli@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfSam,
                    UserName = "sam@lotr.com",
                    Email = "sam@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Id = CommonConstants.UserIdOfBoromir,
                    UserName = "boromir@lotr.com",
                    Email = "boromir@lotr.com",
                    EmailConfirmed = true,
                    PasswordHash = password,
                    PhoneNumber = "08869879",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

        }
    }
}