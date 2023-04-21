using Autofac;
using Autofac.Integration.WebApi;
using SchoolMS.Common;
using SchoolMS.DAL;
using SchoolMS.Repository;
using SchoolMS.Repository.Common;
using SchoolMS.Service;
using SchoolMS.Service.Common;
using SchoolMS.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace SchoolMS.WebApi.App_Start
{
    public class DIContainer
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<StudentService>().As<IStudentService>();
            builder.RegisterType<EFStudentRepository>().As<IStudentRepository>();
            builder.RegisterType<SchoolMSContext>().AsSelf().InstancePerLifetimeScope();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}