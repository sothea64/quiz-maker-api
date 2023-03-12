using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using quiz_maker_models.Enums;
using quiz_maker_models.Helpers;
using quiz_maker_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace quiz_maker_api.DBHelpers
{
    public static class QuizMakerDbContextExt
    {
        static DateTime DefaultRowDate = new DateTime(2019, 1, 1);
        public static void RemovePluralizingTableNameConventionAndForeignKey(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }

        public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {

            var applyConfigurationMethodInfo = modelBuilder
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));
            var ret = typeof(QuizMakerDbContext).Assembly
                 .GetTypes()
                 .Select(t => (t, i: t.GetInterfaces().FirstOrDefault(i => i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
                 .Where(it => it.i != null)
                 .Select(it => (et: it.i.GetGenericArguments()[0], cfgObj: Activator.CreateInstance(it.t)))
                 .Select(it => applyConfigurationMethodInfo.MakeGenericMethod(it.et).Invoke(modelBuilder, new[] { it.cfgObj }))
                 .ToList();
            modelBuilder.RemovePluralizingTableNameConventionAndForeignKey();
            //  modelBuilder.ApplyNotAllowNull();
            modelBuilder.InitData();
        }

        public static void InitData(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Name = "Administrator",
                    Username = "admin",
                    HashPasswrd = SecurityHelper.GetMd5HashPassword("admin", "admin"),
                    UserRole = UserRole.Admin,
                    Active = true,
                    CreatedDate = DefaultRowDate,
                    ModifiedDate = DefaultRowDate,
                    CreatedBy = "Init Data"
                }
            );
        }
    }
}
