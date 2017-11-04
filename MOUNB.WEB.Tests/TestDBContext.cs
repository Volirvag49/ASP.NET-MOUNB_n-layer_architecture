using System;
using System.Collections.Generic;
using System.Data.Entity;
using MOUNB.DAL.EF;
using MOUNB.DAL.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MOUNB.WEB.Tests
{
    public class TestDBContext : ApplicationDBContext
    {
        public TestDBContext() : base("name=TestDBConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Suppress code first model migration check          
            Database.SetInitializer<TestDBContext>(new AlwaysCreateInitializer());

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }


        public void Seed(TestDBContext context)
        {
            // Добавление пользователей           
            List<User> users = new List<User>
            {
                new User{Name="Пользователь по умолчанию 1", Login="111111", Password="111111", Position="Администратор", Role = UserRole.Администратор },
                new User{Name="Пользователь по умолчанию 2", Login="222222", Password="222222", Position="Библиотекарь", Role = UserRole.Библиотекарь },
                new User{Name="Попова Татьяна Ильинична", Login="PopovaTatyana93", Password="vBzEy9XhH9ZN", Position="Тест", Role = UserRole.Администратор },
                new User{Name="Федотов Арсений Олегович", Login="FedotovArseniy255", Password="bfMNjaJaxToR", Position="Тест", Role = UserRole.Библиотекарь },
                new User{Name="Киселёва Ольга Александровна", Login="KiselevaOlga69", Password="JChTQoBWERBo", Position="Тест", Role = UserRole.Библиотекарь },
                new User{Name="Давыдов Варфоломей Викторович", Login="DavyidovVarfolomey46", Password="nzXCZ2ALdbiO", Position="Тест", Role = UserRole.Библиотекарь },
                new User{Name="Леонтьева Васса Ивановна", Login="LeontevaVassa93", Password="HdA3yJRfHYIc", Position="Тест", Role = UserRole.Библиотекарь },
                new User{Name="Дидиченко Арсений Александрович", Login="DidichenkoArseniy119", Password="dvJHlvclxXyh", Position="Тест", Role = UserRole.Библиотекарь },

                new User{Name="Андреева Елизавета Георгиевна ", Login="AndreevaElizaveta231", Password="f8PGfdV4gN9G", Position="Тест", Role = UserRole.Библиотекарь }

            };

            foreach (User user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<TestDBContext>
        {
            protected override void Seed(TestDBContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<TestDBContext>
        {
            protected override void Seed(TestDBContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        public class AlwaysCreateInitializer : DropCreateDatabaseAlways<TestDBContext>
        {
            protected override void Seed(TestDBContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }
    }
}
