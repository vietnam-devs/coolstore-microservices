using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VND.Fw.Domain;
using VND.Fw.Utils.Extensions;
using VND.FW.Infrastructure.EfCore.Options;

namespace VND.FW.Infrastructure.EfCore.Db
{
		public class ApplicationDbContext : DbContext
		{
				private readonly PersistenceOption _persistenceOption = new PersistenceOption();

				public ApplicationDbContext(
						DbContextOptions<ApplicationDbContext> options,
						IConfiguration configuration)
						: base(options)
				{
						var section = configuration.GetSection("Persistence");
						_persistenceOption.FullyQualifiedPrefix = section.GetValue<string>("FullyQualifiedPrefix");
						_persistenceOption.ShortyQualifiedPrefix = section.GetValue<string>("ShortyQualifiedPrefix");
				}

				protected override void OnModelCreating(ModelBuilder builder)
				{
						var typeToRegisters = new List<Type>();

						var ourModules = _persistenceOption.FullyQualifiedPrefix.LoadAssemblyWithPattern();

						typeToRegisters.AddRange(ourModules.SelectMany(m => m.DefinedTypes));

						RegisterEntities(builder, typeToRegisters);

						RegisterConvention(builder);

						base.OnModelCreating(builder);

						RegisterCustomMappings(builder, typeToRegisters);
				}

				private static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
				{
						// TODO: will optimize this more
						var types = typeToRegisters.Where(x => typeof(IEntity).IsAssignableFrom(x) &&
								!x.GetTypeInfo().IsAbstract);

						foreach (var type in types)
						{
								modelBuilder.Entity(type);
						}
				}

				private void RegisterConvention(ModelBuilder modelBuilder)
				{
						var types = modelBuilder.Model.GetEntityTypes().Where(entity => entity.ClrType.Namespace != null);

						foreach (var entityType in types)
						{
								modelBuilder.Entity(entityType.Name).ToTable(entityType.ClrType.Name.Pluralize());
						}
				}

				private static void RegisterCustomMappings(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
				{
						var customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));

						foreach (var builderType in customModelBuilderTypes)
						{
								if (builderType != null && builderType != typeof(ICustomModelBuilder))
								{
										var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
										builder.Build(modelBuilder);
								}
						}
				}
		}
}
