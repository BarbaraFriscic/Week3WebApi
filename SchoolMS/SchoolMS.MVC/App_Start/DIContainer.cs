using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using SchoolMS.DAL;
using SchoolMS.Repository;
using SchoolMS.Repository.Common;
using SchoolMS.Service;
using SchoolMS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SchoolMS.MVC.App_Start
{
    public class DIContainer
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<StudentService>().As<IStudentService>();
            builder.RegisterType<EFStudentRepository>().As<IStudentRepository>();
            builder.RegisterType<SchoolService>().As<ISchoolService>();
            builder.RegisterType<EFSchoolRepository>().As<ISchoolRepository>();
            builder.RegisterType<SchoolMSContext>().AsSelf().InstancePerLifetimeScope();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}