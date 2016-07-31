using System.Configuration;
using System.Data.Entity;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Skarpline.Data.Core;
using Skarpline.Data.Repositories;
using Skarpline.Data.Sql;
using Skarpline.Entities.Domain.Identity;
using Skarpline.Entities.Domain.Messages;
using Skarpline.Entities.Models.Identity;
using Skarpline.Entities.Models.Messages;
using Skarpline.Services;
using Skarpline.Services.Identity;
using Skarpline.Services.Users;
using StructureMap;

namespace Skarpline.Dependencies
{
    /// <summary>
    /// Inversion of Control container
    /// </summary>
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Register();
        }

        /// <summary>
        /// Initializes Automapper profiles and puts them to IoC Container
        /// </summary>
        /// <param name="container">IoC Container</param>
        public static void RegisterMappings(IContainer container)
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<User, UserModel>().ReverseMap();
                x.CreateMap<Message, MessageModel>()
                    .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.User.UserName));
                x.CreateMap<MessageModel, Message>();
                x.ConstructServicesUsing(container.GetInstance);
            });
        }

        /// <summary>
        /// Registers project dependencies and puts them to IoC Container
        /// </summary>
        private void Register()
        {
            //Core:
            For<Data.Core.IContext>().Add<ChatContext>().Ctor<string>("connectionString").Is(ConfigurationManager.ConnectionStrings["ChatContext"].ConnectionString);
            Forward<Data.Core.IContext, DbContext>();
            For<IUnitOfWork>().Add<UnitOfWork>().Ctor<Data.Core.IContext>("context").Is(i => i.GetInstance<Data.Core.IContext>());
            For<IUser>().Use<User>();
            For(typeof(RoleStore<IdentityRole>)).Use(typeof(RoleStore<IdentityRole>));
            For<IUserStore<User>>().Use<UserStore<User>>();
            For<IIdentityValidator<User>>().Use<ChatUserValidator<User>>();
            For<IIdentityValidator<User>>().Add<ChatUserValidator<User>>().Ctor<ChatUserManager>("manager").Is(i => i.GetInstance<ChatUserManager>());
            For<ChatUserManager>().Add<ChatUserManager>().Ctor<IUserStore<User>>("store").Is(i => i.GetInstance<IUserStore<User>>());
            For<RoleManager<IdentityRole, string>>().Add<RoleManager<IdentityRole, string>>().Ctor<IRoleStore<IdentityRole, string>>("store").Is(i => i.GetInstance<RoleStore<IdentityRole>>());
            Forward<ChatUserManager, UserManager<User>>();
            

            //Generics:
            For(typeof(IRepository<>)).Use(typeof(GenericRepository<>));
            For(typeof(IService<>)).Use(typeof(GenericService<>));

            //Services:
            For<IUserService>().Use<UserService>();
        }
    }
}
