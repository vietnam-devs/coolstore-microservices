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
  public abstract class ApplicationDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
     
  {
    private readonly PersistenceOption _persistenceOption = new PersistenceOption();

    protected ApplicationDbContext(
        DbContextOptions<TDbContext> options,
        IConfiguration configuration)
        : base(options)
    {
      IConfigurationSection section = configuration.GetSection("EfCore");
      _persistenceOption.FullyQualifiedPrefix = section.GetValue<string>("FullyQualifiedPrefix");
      _persistenceOption.ShortyQualifiedPrefix = section.GetValue<string>("ShortyQualifiedPrefix");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      List<Type> typeToRegisters = new List<Type>();

      IEnumerable<Assembly> ourModules = _persistenceOption.FullyQualifiedPrefix.LoadAssemblyWithPattern();

      typeToRegisters.AddRange(ourModules.SelectMany(m => m.DefinedTypes));

      RegisterEntities(builder, typeToRegisters);

      RegisterConvention(builder);

      base.OnModelCreating(builder);

      RegisterCustomMappings(builder, typeToRegisters);
    }

    private static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
    {
      // TODO: will optimize this more
      IEnumerable<Type> types = typeToRegisters.Where(x => typeof(IEntity).IsAssignableFrom(x) &&
                !x.GetTypeInfo().IsAbstract);

      foreach (Type type in types)
      {
        modelBuilder.Entity(type);
      }
    }

    private void RegisterConvention(ModelBuilder modelBuilder)
    {
      IEnumerable<Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType> types = modelBuilder.Model.GetEntityTypes().Where(entity => entity.ClrType.Namespace != null);

      foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in types)
      {
        modelBuilder.Entity(entityType.Name).ToTable(entityType.ClrType.Name.Pluralize());
      }
    }

    private static void RegisterCustomMappings(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
    {
      IEnumerable<Type> customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));

      foreach (Type builderType in customModelBuilderTypes)
      {
        if (builderType != null && builderType != typeof(ICustomModelBuilder))
        {
          ICustomModelBuilder builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
          builder.Build(modelBuilder);
        }
      }
    }
  }
}
