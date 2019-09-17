using System.Collections.Generic;
using Autofac;
using AutoMapper;
using LisasTours;
using LisasTours.Application.Queries;
using LisasTours.Controllers;
using LisasTours.Services;
using MediatR;
using Moq;

namespace UnitTests.TestInfrastructure
{
    internal class UnitTestBindings : Module
    {
        internal static IContainer CreateContainer(bool autoMock)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<UnitTestBindings>();

            if (autoMock)
            {

            }

            var container = builder.Build();
            return container;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CompaniesController>();

            builder.Register(_ => new Mock<IExporter>().Object).SingleInstance();
            builder.Register(_ => new Mock<ICompanyQueries>().Object).SingleInstance();
            builder.Register(_ => new Mock<IMediator>().Object).SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Startup).Assembly).As<Profile>();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}