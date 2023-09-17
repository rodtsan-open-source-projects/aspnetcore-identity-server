using AspNetCore.Identity.Core.Interfaces;
using Autofac;
using System.Reflection;

namespace AspNetCore.Identity.Core;

public class DefaultCoreModule : Autofac.Module
{
      protected override void Load(ContainerBuilder builder)
      {
		builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
			.AsClosedTypesOf(typeof(ICommandHandler<,>))
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();

		builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
			.AsClosedTypesOf(typeof(IQueryHandler<,>))
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();

	}
}
